using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

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
				DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
				EnsureCleanDirectory(OutputDirectory);
			});

	Target Restore => _ => _
			.DependsOn(Clean)
			.Executes(() =>
			{
				DotNetRestore(s => DefaultDotNetRestore);
			});

	Target Compile => _ => _
			.DependsOn(Restore)
			.Executes(() =>
			{
				DotNetBuild(s => DefaultDotNetBuild);
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
}
