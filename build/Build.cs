using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using dnkLog4netHtmlReport.build.Helpers;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;

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

		[Parameter]
		public string Version
		{
			get; set;
		}

		private void NugetPack(string projectRelativeFolder)
		{
			Environment.CurrentDirectory = RootDirectory;
			var projectFolder = RootDirectory / projectRelativeFolder;
			var nupkgFiles = Directory.GetFiles(projectFolder, "*.nupkg");
			if(Version == null)
			{
				var highestVersion = nupkgFiles.Select(x => Regex.Replace(x, @".+\.(\d+\.\d+\.\d+\.\d+)\.nupkg", "$1")).OrderBy(x => x).LastOrDefault();
				var versionParts = highestVersion.Split('.').ToList();
				var bumpedVersion = int.Parse(versionParts.Last());
				bumpedVersion++;
				Version = string.Join(".", versionParts.Take(versionParts.Count - 1).Concat(new List<string> { bumpedVersion.ToString() }));
			}
			ProcessHelper.StartProcess(ToolsLocationHelper.NuGetPath, $"pack -Version {Version}", projectFolder);
			nupkgFiles.ForEach(x => File.Delete(x));
		}

		private void NugetPushLocal(string projectRelativeFolder)
		{
			Environment.CurrentDirectory = RootDirectory;
			var projectFolder = RootDirectory / projectRelativeFolder;
			var nupkgFile = Directory.GetFiles(projectFolder, "*.nupkg").Single();
			ProcessHelper.StartProcess(ToolsLocationHelper.NuGetPath, $"push {nupkgFile} 123453 -Source http://localhost/NuGet/api/v2/package", projectFolder);
		}

		Target Pack => _ => _
			.DependsOn(Compile)
			.Executes(() =>
			{
				NugetPack(@"src\dnkLog4netHtmlReport");
				NugetPack(@"src\dnkLog4netHtmlReport.SeleniumWebDriver");
			});


		Target PushLocal => _ => _
			.DependsOn(Pack)
			.Executes(() =>
			{
				NugetPushLocal(@"src\dnkLog4netHtmlReport");
				NugetPushLocal(@"src\dnkLog4netHtmlReport.SeleniumWebDriver");
			});
	}
}
