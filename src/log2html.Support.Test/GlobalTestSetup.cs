using dnk.log2html.Support.NUnit;

namespace dnk.log2html.Support.Test;

[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        new NUnitReportImpl(new ReportMetaData
        {
            ReportName = "log2html.Support.Test Execution Report",
            ReportEnvironment = "DEV"
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Report.AddLevelsSummary();
        Report.Open();
    }
}