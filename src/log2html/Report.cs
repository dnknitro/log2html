using System;
using System.IO;
using log4net;
using log4net.Config;

namespace dnk.log2html
{
    public class Report
    {
        public static ILog Log => _log.Value;

        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() =>
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            var logger = LogManager.GetLogger(typeof(Report));
            logger.Debug("Created logger");
            return logger;
        });

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