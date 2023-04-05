using dnkUtils;

namespace dnk.log2html.Test
{
    public class TestData
	{
		[Test]
		[Explicit]
		public void SetHardcodedTestData()
		{
			var reportMetaData = new ReportMetaData
			{
				ReportName = "log2html.Test Execution Report",
				ReportEnvironment = "DEV"
			};
			var reportFile = new ReportFile(new ReportTemplate(reportMetaData));


			var reportRecords = ResourceUtils.ReadStringFromEmbeddedResource("log2html.Test.TestData.txt", GetType().Assembly);
			reportFile.FileContent.Replace("{\"EndOfReportData\":true}", reportRecords + "{\"EndOfReportData\":true}");

			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "log2html", "ReportTemplate.html"), reportFile.FileContent.ToString());
			Report.Pass("SetHardcodedTestData!");
		}
	}
}