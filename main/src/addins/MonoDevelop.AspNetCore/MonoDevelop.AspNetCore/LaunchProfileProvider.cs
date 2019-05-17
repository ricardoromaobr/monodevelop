﻿//
// LaunchProfileProvider.cs
//
// Author:
//       José Miguel Torres <jostor@microsoft.com>
//
// Copyright (c) 2018 Microsoft
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
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoDevelop.AspNetCore
{
	internal class LaunchProfileProvider
	{
		DotNetProject project;
		readonly object fileLocker = new object ();
		public IDictionary<string, JToken> GlobalSettings { get; private set; }
		internal JObject ProfilesObject { get; private set; }
		internal string launchSettingsJsonPath => Path.Combine (project.BaseDirectory, "Properties", "launchSettings.json");
		const string DefaultGlobalSettings = @"{
    						""windowsAuthentication"": false,
    						""anonymousAuthentication"": true
  							}";

		public LaunchProfileProvider (Project project)
		{
			this.project = (DotNetProject) project;
			GlobalSettings = new Dictionary<string, JToken> ();
		}

		public void LoadLaunchSettings ()
		{
			if (!File.Exists (launchSettingsJsonPath)) {
				CreateAndAddDefaultLaunchSettings ();
				return; 
			}

			var launchSettingsJson = TryParse ();

			GlobalSettings.Clear ();
			foreach (var token in launchSettingsJson) {
				if (token.Key == "profiles") {
					ProfilesObject = token.Value as JObject;
					continue;
				}
				GlobalSettings.Add (token.Key, token.Value);
			}
		}

		JObject TryParse ()
		{
			try {
				return JObject.Parse (File.ReadAllText (launchSettingsJsonPath));
			}
			catch {
				return new JObject ();
			}
		}

		public void SaveLaunchSettings (IDictionary<string, Dictionary<string, object>> profilesData)
		{
			var doc = new JObject ();
			var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

			if (profilesData == null)
				return;

			ProfilesObject = JObject.Parse (JsonConvert.SerializeObject (profilesData, Formatting.Indented, settings));

			foreach (var set in GlobalSettings) {
				doc.Add (set.Key, set.Value);
			}

			doc.Add ("profiles", ProfilesObject);

			try {
				string jsonDocString = doc.ToString (Formatting.Indented);
				string propertiesDirectory = Path.GetDirectoryName (launchSettingsJsonPath);
				Directory.CreateDirectory (propertiesDirectory);

				lock (fileLocker) {
					File.WriteAllText (launchSettingsJsonPath, jsonDocString);
				}
			} catch (IOException ioe) {
				string message = GettextCatalog.GetString ("Failed to write {0}", launchSettingsJsonPath);
				ReportError (message, ioe);
			} catch (UnauthorizedAccessException uae) {
				string message = GettextCatalog.GetString ("Failed to write {0}. Unable to access file or access is denied", launchSettingsJsonPath);
				ReportError (message, uae);
			}
		}

		void ReportError (string message, Exception ex)
		{
			LoggingService.LogError (message, ex);
			MessageService.ShowError (message);
		}

		public LaunchProfileData CreateDefaultProfile ()
		{
			var defaultProfile = new LaunchProfileData {
				Name = project.DefaultNamespace,
				CommandName = "Project",
				LaunchBrowser = true,
				EnvironmentVariables = new Dictionary<string, string> (StringComparer.Ordinal),
				OtherSettings = new Dictionary<string, object> (StringComparer.Ordinal)
			};

			defaultProfile.EnvironmentVariables.Add ("ASPNETCORE_ENVIRONMENT", "Development");
			defaultProfile.OtherSettings.Add ("applicationUrl", "https://localhost:5001;http://localhost:5000");

			return defaultProfile;
		}

		void CreateAndAddDefaultLaunchSettings ()
		{
			GlobalSettings.Add ("iisSettings", JToken.Parse (DefaultGlobalSettings));
			var profilesData = new Dictionary<string, LaunchProfileData> ();
			profilesData.Add (project.DefaultNamespace, CreateDefaultProfile ());
			SaveLaunchSettings (profilesData.ToSerializableForm ());
		}
	}
}
