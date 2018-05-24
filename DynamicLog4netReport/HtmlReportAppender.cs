using System;
using System.IO;
using System.Text;
using System.Threading;
using dnkUtils;
using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;

namespace dnk.DynamicLog4netReport
{
	public class HtmlReportAppender : AppenderSkeleton
	{
		private static readonly object _fileWriteLock = new object();
		private static readonly StringBuilder _fileContent = new StringBuilder();
		private static int _logEntryID = 1;
		private static int _indexToWrite;
		public static string ReportFolder = "";
		private static Action<StringBuilder> _reportTemplateContentVisitor;

		public static ReportMetaData ReportMetaData = new ReportMetaData()
		{
			ReportName = "Please call HtmlReportAppender.Configure() in your OneTimeSetUp SetUpFixture"
		};

		public static void Configure(string reportFolder, ReportMetaData reportMetaData, Action<StringBuilder> reportTemplateContentVisitor = null)
		{
			ReportFolder = reportFolder;
			ReportMetaData = reportMetaData;
			_reportTemplateContentVisitor = reportTemplateContentVisitor;
		}

		private static readonly Lazy<string> ReportPath = new Lazy<string>(() =>
		{
			_fileContent.Append(ResourceUtils.ReadStringFromEmbeddedResource("dnk.DynamicLog4netReport.ReportTemplate.html", typeof(HtmlReportAppender).Assembly));
			_reportTemplateContentVisitor?.Invoke(_fileContent);
			var reportMetaDataJson = JsonConvert.SerializeObject(ReportMetaData);
			_fileContent.Replace("var reportMetaData = {};", $"var reportMetaData = {reportMetaDataJson};");
			Directory.CreateDirectory(ReportFolder);
			var reportPath = Path.GetFullPath(Path.Combine(ReportFolder, $"Report {DateTime.Now:yyyy-MM-dd_hh.mm.ss.fff}.html"));
			_indexToWrite = _fileContent.ToString().IndexOf("{\"EndOfReportData\":true}", StringComparison.Ordinal);
			return reportPath;
		});

		private static readonly Random _r = new Random();

		protected override void Append(LoggingEvent loggingEvent)
		{
			Thread.Sleep(_r.Next(100, 1000));
			lock (_fileWriteLock)
			{
				var cle = new CustomLoggingEvent(_logEntryID++, loggingEvent);
				var json = JsonConvert.SerializeObject(cle/*Formatting.Indented*/) + $",{Environment.NewLine}			";

				var reportPath = ReportPath.Value;
				_fileContent.Insert(_indexToWrite, json);
				File.WriteAllText(reportPath, _fileContent.ToString(), Encoding.UTF8);
				_indexToWrite += json.Length;
			}
		}
	}
}