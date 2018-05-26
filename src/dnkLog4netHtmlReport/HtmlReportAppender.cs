using System;
using System.IO;
using System.Text;
using dnkUtils;
using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;

namespace dnkLog4netHtmlReport
{
	public class HtmlReportAppender : AppenderSkeleton
	{
		private static readonly object _fileWriteLock = new object();
		private static readonly StringBuilder _fileContent = new StringBuilder();
		private static int _logEntryID = 1;
		private static int _indexToWrite;
		private static Action<StringBuilder> _reportTemplateContentVisitor;

		private static readonly Lazy<string> ReportPath = new Lazy<string>(() =>
		{
			_fileContent.Append(ResourceUtils.ReadStringFromEmbeddedResource("dnkLog4netHtmlReport.ReportTemplate.html", typeof(HtmlReportAppender).Assembly));
			_reportTemplateContentVisitor?.Invoke(_fileContent);
			var reportMetaDataJson = JsonConvert.SerializeObject(Config.ReportMetaData);
			_fileContent.Replace("var reportMetaData = {};", $"var reportMetaData = {reportMetaDataJson};");
			Directory.CreateDirectory(Config.ReportFolder);
			Config.ReportFileNameOnly = $"Report_{DateTime.Now:yyyy-MM-dd_hh.mm.ss.fff}";
			var reportPath = Path.GetFullPath(Path.Combine(Config.ReportFolder, Config.ReportFileNameOnly + ".html"));
			_indexToWrite = _fileContent.ToString().IndexOf("{\"EndOfReportData\":true}", StringComparison.Ordinal);
			return reportPath;
		});

		public static void Configure(Action<StringBuilder> reportTemplateContentVisitor)
		{
			_reportTemplateContentVisitor = reportTemplateContentVisitor;
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			lock (_fileWriteLock)
			{
				var cle = new CustomLoggingEvent(_logEntryID++, loggingEvent);
				var json = JsonConvert.SerializeObject(cle /*Formatting.Indented*/) + $",{Environment.NewLine}			";

				var reportPath = ReportPath.Value;
				_fileContent.Insert(_indexToWrite, json);
				File.WriteAllText(reportPath, _fileContent.ToString(), Encoding.UTF8);
				_indexToWrite += json.Length;
			}
		}
	}
}