using dnk.log2html;
using dnk.log2html.Support.NUnit;

namespace log2html.nuget.test;

[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        new NUnitReportImpl(new ReportMetaData
        {
            ReportName = "log2html.nuget.test",
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