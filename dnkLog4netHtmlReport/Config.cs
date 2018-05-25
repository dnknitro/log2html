﻿using log4net;

namespace dnkLog4netHtmlReport
{
	public class Config
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
	}
}