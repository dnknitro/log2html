using log4net;

namespace dnk.log2html
{
	public class Report
	{
		public static string ReportFolder = "";
		public static string ReportFileNameOnly = "";

		public static ReportMetaData ReportMetaData = new ReportMetaData
		{
			ReportName = "Please call HtmlReportAppender.Configure() in your OneTimeSetUp SetUpFixture"
		};

		public static void Configure(string reportFolder, ReportMetaData reportMetaData)
		{
			ReportFolder = reportFolder;
			ReportMetaData = reportMetaData;
		}

		public static void SetBrowser(string browser)
		{
			LogicalThreadContext.Properties[LogExtensions.BrowserPropertyName] = browser;
		}

		public static void SetTestCaseName(string testCaseName)
		{
			LogicalThreadContext.Properties[LogExtensions.TestCaseName] = testCaseName;
		}
	}
}