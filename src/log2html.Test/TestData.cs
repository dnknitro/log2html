using dnkUtils;

namespace dnk.log2html.Test;

public class TestData
{
    [Test]
    [Explicit]
    public void SetHardcodedTestData()
    {
        ReportMetaData reportMetaData = new()
        {
            ReportName = "log2html.Test Execution Report",
            ReportEnvironment = "DEV"
        };
        ReportTemplate reportTemplate = new(reportMetaData);
        var contents = reportTemplate.GetTemplate();

        var reportRecords = ResourceUtils.ReadStringFromEmbeddedResource("log2html.Test.TestData.txt", GetType().Assembly);
        const string endOfReportData = "{ \"EndOfReportData\": true }";

        contents.Replace(endOfReportData, reportRecords + endOfReportData);

        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "log2html", "ReportTemplate.html"), contents);
        Report.Pass("SetHardcodedTestData!");
    }
}