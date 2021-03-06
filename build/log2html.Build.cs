﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using dnk.log2html.build.Helpers;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;

namespace dnk.log2html.build
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
					@"log2html\Properties\AssemblyInfo.cs",
					@"log2html.Support\Properties\AssemblyInfo.cs"
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
			var nupkgFiles = Directory.GetFiles(projectFolder, "*.nupkg").ToList();
			var versionWasNull = Version == null;
			if (versionWasNull)
			{
				var highestVersion = nupkgFiles.Select(x => Regex.Replace(x, @".+\.(\d+\.\d+\.\d+\.\d+)\.nupkg", "$1")).OrderBy(x => x).LastOrDefault();
				var versionParts = highestVersion.Split('.').ToList();
				var bumpedVersion = int.Parse(versionParts.Last());
				bumpedVersion++;
				Version = string.Join(".", versionParts.Take(versionParts.Count - 1).Concat(new List<string> {bumpedVersion.ToString()}));
			}
			else
			{
				nupkgFiles.ForEach(x => File.Delete(x));
			}
			ProcessHelper.StartProcess(ToolsLocationHelper.NuGetPath, $"pack -Version {Version}", projectFolder);
			if (versionWasNull)
				nupkgFiles.ForEach(x => File.Delete(x));
		}

	    const string localNugetApiKey = "123453";
	    const string localNugetSource = "http://localhost/NuGet/api/v2/package";
	    const string remoteNugetSource = "https://api.nuget.org/v3/index.json";

		private void NugetPushLocal(string projectRelativeFolder, string source, string apiKey = null)
		{
			Environment.CurrentDirectory = RootDirectory;
			var projectFolder = RootDirectory / projectRelativeFolder;
			var nupkgFile = Directory.GetFiles(projectFolder, "*.nupkg").Single();
		    var apiKeyPart = string.IsNullOrEmpty(apiKey) ? string.Empty : apiKey;
			ProcessHelper.StartProcess(ToolsLocationHelper.NuGetPath, $"push {nupkgFile} {apiKeyPart} -Source {source}", projectFolder);
		}

		Target Pack => _ => _
			.DependsOn(Compile)
			.Executes(() =>
			{
				NugetPack(@"src\log2html");
				NugetPack(@"src\log2html.Support");
			});

		Target PushLocal => _ => _
			.DependsOn(Pack)
			.Executes(() =>
			{
				NugetPushLocal(@"src\log2html", localNugetSource, localNugetApiKey);
				NugetPushLocal(@"src\log2html.Support", localNugetSource, localNugetApiKey);
			});

		Target PushRemote => _ => _
			.DependsOn(Pack)
			.Executes(() =>
		    {
		        Logger.Info($"don't forget to run 'nuget setApiKey secret_key' (key can be generated @ nuget.org)");
			    NugetPushLocal(@"src\log2html", remoteNugetSource);
			    NugetPushLocal(@"src\log2html.Support", remoteNugetSource);
			});
	}
}
