using System;
using System.IO;
using Nuke.Common;
using Nuke.Common.IO;

namespace dnkLog4netHtmlReport.build.Helpers
{
	class ToolsLocationHelper
	{
		public static PathConstruction.AbsolutePath BuildTempDirectory => (PathConstruction.AbsolutePath)Path.Combine(Environment.CurrentDirectory, ".tmp");

		public static string NuGetPath => BuildTempDirectory / "nuget.exe";

		public static PathConstruction.AbsolutePath UserProfileFolder => (PathConstruction.AbsolutePath)EnvironmentInfo.Variable("userprofile");


		private static readonly Lazy<string> _vsTestExe = new Lazy<string>(() =>
		{
			var output = ProcessHelper.StartProcess(@"tools\vswhere.exe", @"-latest -products * -requires Microsoft.VisualStudio.PackageGroup.TestTools.Core -property installationPath");
			return Path.Combine(output, @"Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe");
		});

		public static string VsTestExe => _vsTestExe.Value;

		#region OpenCover

		private static readonly string _openCoverVersion = "4.6.519";

		private static readonly Lazy<string> _openCoverExe = new Lazy<string>(() =>
		{
			ProcessHelper.StartProcess(NuGetPath, $@"install OpenCover -Version {_openCoverVersion} -OutputDirectory {BuildTempDirectory}");
			return BuildTempDirectory / $@"opencover.{_openCoverVersion}\tools\OpenCover.Console.exe";
		});

		public static string OpenCoverExe => _openCoverExe.Value;

		#endregion OpenCover

		#region Coverage Report Generator

		private static readonly string _coverageReportGeneratorVersion = "3.1.2";

		private static readonly Lazy<string> _coverageReportGeneratorExe = new Lazy<string>(() =>
		{
			ProcessHelper.StartProcess(NuGetPath, $@"install ReportGenerator -Version {_coverageReportGeneratorVersion} -OutputDirectory {BuildTempDirectory}");
			return BuildTempDirectory / $@"ReportGenerator.{_coverageReportGeneratorVersion}\tools\ReportGenerator.exe";
		});

		public static string CoverageReportGeneratorExe => _coverageReportGeneratorExe.Value;

		#endregion Coverage Report Generator
	}
}