using dnk.log2html.Support.NUnit;
using NUnit.Framework;

namespace dnk.log2html.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			new NUnitReportImpl(new ReportMetaData
			{
				ReportName = "log2html.Test Execution Report",
				ReportEnvironment = "DEV"
			});
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			Report.Open();
		}
	}
}