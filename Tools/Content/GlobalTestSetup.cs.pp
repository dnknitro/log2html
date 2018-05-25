using System;
using System.IO;
using NUnit.Framework;

namespace $rootnamespace$
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
			dnkLog4netHtmlReport.Config.Configure(
				Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.FullName, "Results"),
				new dnkLog4netHtmlReport.ReportMetaData
				{
					ReportName = "Test Execution Report TEST",
					ReportCategory = "Test",
					ReportEnvironment = "DEV"
				}
			);
		}
	}
}