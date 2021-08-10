using dnk.log2html.Support.nunit;
using NUnit.Framework;

namespace dnk.log2html.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var reportMetaData = new ReportMetaData
			{
				ReportName = "log2html.Test Execution Report",
				ReportEnvironment = "DEV"
			};
			var report = new ReportImpl(
				new ReportFile(new ReportTemplate(reportMetaData)),
				new ReportEntryFactory(new NUnitTestCaseName())
			);
			Report.Configure(report);
		}
	}
}