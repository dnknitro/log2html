using dnk.log2html.Support.NUnit;
using NUnit.Framework;

namespace dnk.log2html.Support.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var reportMetaData = new ReportMetaData
			{
				ReportName = "log2html.Support.Test Execution Report",
				ReportEnvironment = "DEV"
			};
			var report = new ReportImpl(
				new ReportFile(new ReportTemplate(reportMetaData)),
				new ReportEntryFactory(new NUnitTestCaseName()),
				new NUnitTestStorage()
			);
			Report.Configure(report);
		}
	}
}