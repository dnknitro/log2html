using System;
using System.IO;
using NUnit.Framework;

namespace dnk.DynamicLog4netReport.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			HtmlReportAppender.Configure(
				Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Results"),
				new ReportMetaData
				{
					ReportName = "Test Execution Report TEST",
					ReportCategory = "Test",
					ReportEnvironment = "DEV"
				}
			);
		}
	}
}