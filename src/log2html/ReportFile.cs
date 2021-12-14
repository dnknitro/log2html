using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace dnk.log2html
{
	public class ReportFile
	{
		public static string DefaultReportFileNameOnly = $"Report_{DateTime.Now:yyyy-MM-dd_HH.mm.ss.fff}";

		// ReSharper disable PossibleNullReferenceException
		public static string DefaultReportFolder = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.Trim('\\')).Parent.Parent.Parent.FullName, "Results");
		// ReSharper restore PossibleNullReferenceException

		public ReportFile(ReportTemplate reportTemplate, string reportFileNameOnly = null, string reportFolder = null)
		{
			ReportFileNameOnly = reportFileNameOnly ?? DefaultReportFileNameOnly;
			// ReSharper disable once PossibleNullReferenceException
			ReportFolder = reportFolder ?? DefaultReportFolder;
			//_reportFolder = reportFolder;
			ReportFilePath = Path.GetFullPath(Path.Combine(ReportFolder, ReportFileNameOnly + ".html"));

			FileContent.Append(reportTemplate.GetTemplate());

			Directory.CreateDirectory(ReportFolder);

			_indexToWrite = FileContent.ToString().IndexOf("{\"EndOfReportData\":true}", StringComparison.Ordinal);
		}

		public StringBuilder FileContent { get; } = new StringBuilder();

		public string ReportFileNameOnly { get; }
		public string ReportFolder { get; }
		public string ReportFilePath { get; }

		private static readonly object _fileWriteLock = new object();
		private int _indexToWrite;

		public void Append(ReportEntry reportEntry, params IReportEntryVisitor[] reportEntryVisitors)
		{
			foreach (var reportEntryVisitor in reportEntryVisitors)
				reportEntryVisitor.Visit(reportEntry, this);

			lock (_fileWriteLock)
			{
				var json = JsonConvert.SerializeObject(reportEntry /*Formatting.Indented*/) + $",{Environment.NewLine}			";

				FileContent.Insert(_indexToWrite, json);
				File.WriteAllText(ReportFilePath, FileContent.ToString(), Encoding.UTF8);
				_indexToWrite += json.Length;
			}
		}
	}
}