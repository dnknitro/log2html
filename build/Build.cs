using System.IO;
using System.Text.RegularExpressions;
using dnkLog4netHtmlReport.build.Helpers;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;

namespace dnkLog4netHtmlReport.build
{
	class Build : NukeBuild
	{
		// Console application entry point. Also defines the default target.
		public static int Main() => Execute<Build>(x => x.Compile);

		// Auto-injection fields:

		[GitVersion] readonly GitVersion GitVersion;
		// Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

		// [GitRepository] readonly GitRepository GitRepository;
		// Parses origin, branch name and head from git config.

		// [Parameter] readonly string MyGetApiKey;
		// Returns command-line arguments and environment variables.

		// [Solution] readonly Solution Solution;
		// Provides access to the structure of the solution.

		Target Clean => _ => _
			.OnlyWhen(() => false) // Disabled for safety.
			.Executes(() =>
			{
				FileSystemTasks.DeleteDirectories(PathConstruction.GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
				FileSystemTasks.EnsureCleanDirectory(OutputDirectory);
			});

		Target Compile => _ => _
			.Executes(() =>
			{
				NuGetTasks.NuGetRestore(SolutionFile, config => config.SetToolPath(ToolsLocationHelper.NuGetPath));

				MSBuildTasks.MSBuild(s => 
					MSBuildTasks.DefaultMSBuildCompile
					.SetVerbosity(MSBuildVerbosity.Minimal)
					.SetTargets("Build")
					.SetNodeReuse(true)
					.SetMaxCpuCount(4));
			});

		Target SetAssemblyVersion => _ => _
			.Executes(() =>
			{
				var assemblyInfos = new[]
				{
					@"dnkLog4netHtmlReport\Properties\AssemblyInfo.cs",
					@"dnkLog4netHtmlReport.SeleniumWebDriver\Properties\AssemblyInfo.cs"
				};

				foreach(var assemblyInfo in assemblyInfos)
				{
					var assemblyInfoFile = RootDirectory / assemblyInfo;
					var content = File.ReadAllText(assemblyInfoFile);
					content = Regex.Replace(content, @"AssemblyVersion\(\s*"".+""\s*\)", $@"AssemblyVersion(""{GitVersion.GetNormalizedAssemblyVersion()}"")");
					content = Regex.Replace(content, @"AssemblyFileVersion\(\s*"".+""\s*\)", $@"AssemblyFileVersion(""{GitVersion.GetNormalizedFileVersion()}"")");
					content = Regex.Replace(content, @"AssemblyInformationalVersion\(\s*"".+""\s*\)", $@"AssemblyInformationalVersion(""{GitVersion.InformationalVersion}"")");
					File.WriteAllText(assemblyInfoFile, content);
				}
			});

		Target Pack => _ => _
			.Executes(() =>
			{
				ProcessHelper.StartProcess("nuget.exe", "pack -Version 1.0.0.4", RootDirectory / @"src\dnkLog4netHtmlReport");
			});
	}
}
