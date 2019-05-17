//
// WorkspaceFilesCache.cs
//
// Author:
//       Marius Ungureanu <maungu@microsoft.com>
//
// Copyright (c) 2019 Microsoft Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using Newtonsoft.Json;

namespace MonoDevelop.Ide.TypeSystem
{
	class WorkspaceFilesCache
	{
		const int format = 1;

		FilePath cacheDir;

		Dictionary<FilePath, ProjectCache> cachedItems = new Dictionary<FilePath, ProjectCache> ();
		Dictionary<string, FileLock> writeLockMap = new Dictionary<string, FileLock> ();
		bool loaded;

		public void Load (Solution solution)
		{
			if (loaded)
				return;

			if (solution == null || Runtime.PeekService<RootWorkspace> () == null)
				return;

			if (solution.FileName.IsNullOrEmpty)
				return;

			cacheDir = solution.GetPreferencesDirectory ().Combine ("project-cache");
			Directory.CreateDirectory (cacheDir);

			lock (cachedItems) {
				if (loaded)
					return;

				loaded = true;
				LoadCache (solution);
			}
		}

		void LoadCache (Solution sol)
		{
			var solConfig = sol.GetConfiguration (IdeServices.Workspace.ActiveConfiguration);
			if (solConfig == null)
				return;

			var serializer = new JsonSerializer ();
			foreach (var project in sol.GetAllProjects ()) {
				var config = solConfig.GetMappedConfiguration (project);

				var projectFilePath = project.FileName;
				var cacheFilePath = GetProjectCacheFile (project, config);

				try {
					if (!File.Exists (cacheFilePath))
						continue;

					using (var sr = File.OpenText (cacheFilePath)) {
						var value = (ProjectCache)serializer.Deserialize (sr, typeof (ProjectCache));

						if (format != value.Format)
							continue;

						cachedItems [projectFilePath] = value;

					}
				} catch (Exception ex) {
					LoggingService.LogError ("Could not deserialize project cache", ex);
					TryDeleteFile (cacheFilePath);
					continue;
				}
			}
		}

		// We only care about invalid characters on Windows, as unix-systems only have `/` as invalid character.
		static readonly char [] invalidChars = Platform.IsWindows ? Path.GetInvalidFileNameChars () : Array.Empty<char> ();
		string GetProjectCacheFile (Project proj, string configuration)
		{
			return cacheDir.Combine (proj.FileName.FileNameWithoutExtension + "-" + Sanitize (configuration) + ".json");

			string Sanitize(string value)
			{
				if (invalidChars.Length == 0)
					return value;

				int lastIndex = 0;
				while ((lastIndex = value.IndexOfAny (invalidChars, lastIndex)) != -1) {
					value = value.Replace (value [lastIndex], '_');
				}

				return value;
			}
		}

		static void TryDeleteFile (string file)
		{
			try {
				File.Delete (file);
			} catch (Exception ex) {
				LoggingService.LogError ("Could not delete cache file", ex);
			}
		}

		public void Update (ProjectConfiguration projConfig, Project proj, MonoDevelopWorkspace.ProjectDataMap projectMap, ProjectCacheInfo info)
		{
			Update (projConfig, proj, projectMap, info.SourceFiles, info.AnalyzerFiles, info.References, info.ProjectReferences);
		}

		public void Update (ProjectConfiguration projConfig, Project proj, MonoDevelopWorkspace.ProjectDataMap projectMap,
			ImmutableArray<ProjectFile> files,
			ImmutableArray<FilePath> analyzers,
			ImmutableArray<MonoDevelopMetadataReference> metadataReferences,
			ImmutableArray<Microsoft.CodeAnalysis.ProjectReference> projectReferences)
		{
			if (!loaded)
				return;

			var paths = new string [files.Length];
			var actions = new string [files.Length];

			for (int i = 0; i < files.Length; ++i) {
				paths [i] = files [i].FilePath;
				actions [i] = files [i].BuildAction;
			}

			var projectRefs = new ReferenceItem [projectReferences.Length];
			for (int i = 0; i < projectReferences.Length; ++i) {
				var pr = projectReferences [i];
				var mdProject = projectMap.GetMonoProject (pr.ProjectId);
				projectRefs [i] = new ReferenceItem {
					FilePath = mdProject.FileName,
					Aliases = pr.Aliases.ToArray (),
				};
			}

			var item = new ProjectCache {
				Format = format,
				Analyzers = analyzers.Select(x => (string)x).ToArray (),
				Files = paths,
				BuildActions = actions,
				MetadataReferences = metadataReferences.Select(x => {
					var ri = new ReferenceItem {
						FilePath = x.FilePath,
						Aliases = x.Properties.Aliases.ToArray (),
					};
					return ri;
				}).ToArray (),
				ProjectReferences = projectRefs,
			};

			var cacheFile = GetProjectCacheFile (proj, projConfig.Id);

			FileLock fileLock = AcquireWriteLock (cacheFile);
			try {
				lock (fileLock) {
					var serializer = new JsonSerializer ();
					using (var fs = File.Open (cacheFile, FileMode.Create))
					using (var sw = new StreamWriter (fs)) {
						serializer.Serialize (sw, item);
					}
				}
			} finally {
				ReleaseWriteLock (cacheFile, fileLock);
			}
		}

		public bool TryGetCachedItems (Project p, MonoDevelopMetadataReferenceManager provider, MonoDevelopWorkspace.ProjectDataMap projectMap,
			out ProjectCacheInfo info)
		{
			info = new ProjectCacheInfo ();
			return TryGetCachedItems (p, provider, projectMap, out info.SourceFiles, out info.AnalyzerFiles, out info.References, out info.ProjectReferences);
		}

		public bool TryGetCachedItems (Project p, MonoDevelopMetadataReferenceManager provider, MonoDevelopWorkspace.ProjectDataMap projectMap,
			out ImmutableArray<ProjectFile> files,
			out ImmutableArray<FilePath> analyzers,
			out ImmutableArray<MonoDevelopMetadataReference> metadataReferences,
			out ImmutableArray<Microsoft.CodeAnalysis.ProjectReference> projectReferences)
		{
			files = ImmutableArray<ProjectFile>.Empty;
			analyzers = ImmutableArray<FilePath>.Empty;
			metadataReferences = ImmutableArray<MonoDevelopMetadataReference>.Empty;
			projectReferences = ImmutableArray<Microsoft.CodeAnalysis.ProjectReference>.Empty;

			ProjectCache cachedData;
			lock (cachedItems) {
				if (!cachedItems.TryGetValue (p.FileName, out cachedData)) {
					return false;
				}
			}

			var filesBuilder = ImmutableArray.CreateBuilder<ProjectFile> (cachedData.Files.Length);
			for (int i = 0; i < cachedData.Files.Length; ++i) {
				filesBuilder.Add(new ProjectFile (cachedData.Files [i], cachedData.BuildActions [i]) {
					Project = p,
				});
			}

			files = filesBuilder.MoveToImmutable ();

			var analyzersBuilder = ImmutableArray.CreateBuilder<FilePath> (cachedData.Analyzers.Length);
			foreach (var analyzer in cachedData.Analyzers)
				analyzersBuilder.Add (analyzer);
			analyzers = analyzersBuilder.MoveToImmutable ();

			var mrBuilder = ImmutableArray.CreateBuilder<MonoDevelopMetadataReference> (cachedData.MetadataReferences.Length);
			foreach (var item in cachedData.MetadataReferences) {
				var aliases = item.Aliases != null ? item.Aliases.ToImmutableArray () : default;
				var reference = provider.GetOrCreateMetadataReference (item.FilePath, new Microsoft.CodeAnalysis.MetadataReferenceProperties(aliases: aliases));
				mrBuilder.Add (reference);
			}
			metadataReferences = mrBuilder.MoveToImmutable ();

			var sol = p.ParentSolution;
			var solConfig = sol.GetConfiguration (IdeServices.Workspace.ActiveConfiguration);
			var allProjects = sol.GetAllProjects ().ToDictionary (x => x.FileName, x => x);

			var prBuilder = ImmutableArray.CreateBuilder<Microsoft.CodeAnalysis.ProjectReference> (cachedData.ProjectReferences.Length);
			foreach (var item in cachedData.ProjectReferences) {
				if (!allProjects.TryGetValue (item.FilePath, out var mdProject))
					return false;

				var aliases = item.Aliases != null ? item.Aliases.ToImmutableArray () : default;
				var pr = new Microsoft.CodeAnalysis.ProjectReference (projectMap.GetOrCreateId (mdProject, null), aliases.ToImmutableArray ());
				prBuilder.Add (pr);
			}
			projectReferences = prBuilder.MoveToImmutable ();
			return true;
		}

		/// <summary>
		/// Clears the in-memory cache for this project now that the loaded cache information has been used.
		/// Updates for the cache are written to disk.
		/// </summary>
		public bool OnCacheInfoUsed (Project p)
		{
			lock (cachedItems) {
				return cachedItems.Remove (p.FileName);
			}
		}

		void ReleaseWriteLock (string cacheFile, FileLock fileLock)
		{
			lock (writeLockMap) {
				fileLock.ReferenceCount--;
				if (fileLock.ReferenceCount == 0)
					writeLockMap.Remove (cacheFile);
			}
		}

		FileLock AcquireWriteLock (string cacheFile)
		{
			lock (writeLockMap) {
				FileLock fileLock = null;
				if (writeLockMap.TryGetValue (cacheFile, out fileLock)) {
					fileLock.ReferenceCount++;
					return fileLock;
				}

				fileLock = new FileLock {
					ReferenceCount = 1
				};
				writeLockMap [cacheFile] = fileLock;
				return fileLock;
			}
		}

		[Serializable]
		class ProjectCache
		{
			public int Format;

			public ReferenceItem [] ProjectReferences;
			public ReferenceItem[] MetadataReferences;
			public string [] Files;
			public string [] BuildActions;
			public string [] Analyzers;
		}

		[Serializable]
		internal class ReferenceItem
		{
			public string FilePath;
			public string [] Aliases = Array.Empty<string> ();
		}

		class FileLock
		{
			public int ReferenceCount;
		}
	}

	internal class ProjectCacheInfo : IEquatable<ProjectCacheInfo>
	{
		public ImmutableArray<ProjectFile> SourceFiles;
		public ImmutableArray<FilePath> AnalyzerFiles;
		public ImmutableArray<MonoDevelopMetadataReference> References;
		public ImmutableArray<Microsoft.CodeAnalysis.ProjectReference> ProjectReferences;

		public override bool Equals (object obj)
		{
			if (obj is ProjectCacheInfo cacheInfo)
				return Equals (cacheInfo);
			return false;
		}

		public bool Equals (ProjectCacheInfo other)
		{
			if (other == null)
				return false;

			if (AnalyzerFiles.Length != other.AnalyzerFiles.Length ||
				SourceFiles.Length != other.SourceFiles.Length ||
				References.Length != other.References.Length ||
				ProjectReferences.Length != other.ProjectReferences.Length) {
				return false;
			}

			for (int i = 0; i < SourceFiles.Length; ++i) {
				var file = SourceFiles [i];
				var otherFile = other.SourceFiles [i];
				if (file.FilePath != otherFile.FilePath ||
					file.BuildAction != otherFile.BuildAction) {
					return false;
				}
			}

			for (int i = 0; i < AnalyzerFiles.Length; ++i) {
				if (AnalyzerFiles [i] != other.AnalyzerFiles [i]) {
					return false;
				}
			}

			for (int i = 0; i < References.Length; ++i) {
				var reference = References [i];
				var otherReference = other.References [i];
				if (reference.FilePath != otherReference.FilePath ||
					reference.Properties.Aliases.Length != otherReference.Properties.Aliases.Length) {
					return false;
				}

				for (int j = 0; j < reference.Properties.Aliases.Length; ++j) {
					if (reference.Properties.Aliases [j] != otherReference.Properties.Aliases [j]) {
						return false;
					}
				}
			}

			for (int i = 0; i < ProjectReferences.Length; ++i) {
				var reference = ProjectReferences [i];
				var otherReference = other.ProjectReferences [i];
				if (reference.ProjectId != otherReference.ProjectId ||
					reference.Aliases.Length != otherReference.Aliases.Length) {
					return false;
				}

				for (int j = 0; j < reference.Aliases.Length; ++j) {
					if (reference.Aliases [j] != otherReference.Aliases [j]) {
						return false;
					}
				}
			}

			return true;
		}
	}
}
