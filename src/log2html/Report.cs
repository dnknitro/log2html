using System;
using log4net;

namespace dnk.log2html
{
	public class Report
	{
		private static string _reportFolder;
		private static string _reportFileNameOnly;

		public static ReportMetaData ReportMetaData = new ReportMetaData
		{
			ReportName = "Please call HtmlReportAppender.Configure() in your OneTimeSetUp SetUpFixture"
		};

		public static string ReportFolder
		{
			get
			{
				if (_reportFolder == null)
					throw new NullReferenceException($"{nameof(ReportFolder)} is null. Please call Report.Configure(...) first.");
				return _reportFolder;
			}
			private set => _reportFolder = value;
		}

		public static string ReportFileNameOnly
		{
			get
			{
				if (_reportFileNameOnly == null)
					throw new NullReferenceException($"{nameof(ReportFileNameOnly)} is null. Please call Report.Configure(...) first.");
				return _reportFileNameOnly;
			}
			private set => _reportFileNameOnly = value;
		}

		public static void Configure(string reportFolder, ReportMetaData reportMetaData)
		{
			ReportFileNameOnly = $"Report_{DateTime.Now:yyyy-MM-dd_HH.mm.ss.fff}";
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