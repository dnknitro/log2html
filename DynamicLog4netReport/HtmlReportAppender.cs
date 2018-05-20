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
		//public DynamicAppender()
		//{
		//	_fileAppender = new FileAppender();
		//	_fileAppender.File = 
		//}

		//private readonly FileAppender _fileAppender;

		private static readonly object _fileWriteLock = new object();
		private static readonly StringBuilder _fileContent = new StringBuilder();
		private static int _logEntryID = 1;
		private static int _indexToWrite;
		private static readonly string ReportFolder = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Results");

		private static readonly Lazy<string> ReportPath = new Lazy<string>(() =>
		{
			_fileContent.Append(ResourceUtils.ReadStringFromEmbeddedResource("dnk.DynamicLog4netReport.ReportTemplate.html", typeof(HtmlReportAppender).Assembly));
			Directory.CreateDirectory(ReportFolder);
			var reportPath = Path.GetFullPath(Path.Combine(ReportFolder, $"Report {DateTime.Now:yyyy-MM-dd_hh.mm.ss.fff}.html"));
			_indexToWrite = _fileContent.ToString().IndexOf("{\"EndOfReportData\":true}", StringComparison.Ordinal);
			return reportPath;
		});

		private static Random _r = new Random();

		protected override void Append(LoggingEvent loggingEvent)
		{
			Thread.Sleep(_r.Next(100, 1000));
			lock (_fileWriteLock)
			{
				var cle = new CustomLoggingEvent(_logEntryID++, loggingEvent);
				//var json = JsonConvert.SerializeObject(cle,) + ",\r\n			";
				var json = JsonConvert.SerializeObject(cle, Formatting.Indented) + ",\r\n";

				var reportPath = ReportPath.Value;
				_fileContent.Insert(_indexToWrite, json);
				File.WriteAllText(reportPath, _fileContent.ToString(), Encoding.UTF8);
				_indexToWrite += json.Length;
			}
		}
	}
}