using dnk.DynamicLog4netReport;
using NUnit.Framework;

namespace DynamicLog4netReport.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			HtmlReportAppender.Configure(new ReportMetaData
			{
				ReportName = "Test Execution Report",
				ReportCategory = "Smoke",
				ReportEnvironment = "PROD"
			});
		}

		//[OneTimeTearDown]
		//public void OneTimeTearDown()
		//{
		//}
	}
}