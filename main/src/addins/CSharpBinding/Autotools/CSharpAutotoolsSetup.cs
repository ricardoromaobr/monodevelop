
using System;
using System.IO;
using MonoDevelop.Autotools;
using MonoDevelop.Projects;
using MonoDevelop.CSharp.Project;

namespace CSharpBinding.Autotools
{
	class CSharpAutotoolsSetup : ISimpleAutotoolsSetup
	{
		public string GetCompilerCommand (Project project, string configuration)
		{
			if (CanDeploy (project) && project is DotNetProject dp)
				return "mcs";
			throw new Exception ("Not a deployable project.");
		}

		public string GetCompilerFlags ( Project project, string configuration )
		{
			if ( !this.CanDeploy ( project ) )
				throw new Exception ( "Not a deployable project." );
			
			DotNetProjectConfiguration config = 
				project.Configurations [configuration] as DotNetProjectConfiguration;

			if ( config == null ) return "";
			
			CSharpCompilerParameters parameters = (CSharpCompilerParameters) config.CompilationParameters;
			ICSharpProject projectParameters = config.ParentItem as ICSharpProject;
			
			StringWriter writer = new StringWriter();
			
			writer.Write(" -noconfig");
			writer.Write(" -codepage:utf8");
			
			if (parameters.UnsafeCode) {
				writer.Write(" -unsafe");
			}
			writer.Write(" -warn:" + parameters.WarningLevel);
			if(parameters.Optimize)
				writer.Write(" -optimize+");
			else
				writer.Write(" -optimize-");

			if(parameters.NoWarnings != null && parameters.NoWarnings != "") {
				writer.Write(" \"-nowarn:" + parameters.NoWarnings + '"');
			}

			if(config.DebugSymbols) {
				writer.Write(" -debug");
				//Check whether we have a DEBUG define
				bool hasDebugDefine = false;
				foreach (string define in parameters.DefineSymbols.Split (';')) {
					if (String.Compare (define, "DEBUG") == 0) {
						hasDebugDefine = true;
						break;
					}
				}
				if (!hasDebugDefine)
					writer.Write (" -define:DEBUG");
			}

			var langVersion = config.Properties.GetValue<string> ("LangVersion");
			if (!string.IsNullOrEmpty (langVersion)) {
				writer.Write (" -langversion:" + langVersion + " ");
			}
			
			
			// TODO check path and add to extradist...
			//if (parameters.Win32Icon != null && parameters.Win32Icon.Length > 0) {
			//	writer.Write(" \"-win32icon:" + compilerparameters.Win32Icon + "\"");
			//}
			
			if (parameters.DefineSymbols.Length > 0) {
				writer.Write (string.Format (" \"-define:{0}\"", parameters.DefineSymbols.TrimEnd(';')));
			}
				
			if (projectParameters.MainClass != null && projectParameters.MainClass != "") {
				writer.Write (" \"-main:" + projectParameters.MainClass + '"');
			}

			if (config.SignAssembly)
				writer.Write (" \"-keyfile:" + project.GetRelativeChildPath (config.AssemblyKeyFile) + '"');
			if (config.DelaySign)
				writer.Write (" -delaySign");

			// TODO check paths and add to extradist?
			//if (parameters.GenerateXmlDocumentation) {
			//	writer.WriteLine(" \"-doc:" + Path.ChangeExtension(exe, ".xml") + '"');
			//}
		
			return writer.ToString();
		}
		
		public bool CanDeploy ( Project project )
		{
			DotNetProject csproj = project as DotNetProject;
			if ( csproj != null )
				if ( csproj.LanguageName == "C#" ) return true;
			return false;
		}
	}
}
