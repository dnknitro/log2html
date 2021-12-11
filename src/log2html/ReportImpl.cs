using System;

namespace dnk.log2html
{
	public class ReportImpl
	{
		public ReportImpl(ReportFile reportFile, IReportEntryFactory reportEntryFactory, ITestStorage testStorage)
		{
			_reportFile = reportFile;
			_reportEntryFactory = reportEntryFactory;
			ReportContext.Configure(testStorage);
		}

		private readonly ReportFile _reportFile;
		private readonly IReportEntryFactory _reportEntryFactory;

		public void Debug(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Debug, message, ex, reportEntryVisitors);
		public void Pass(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Pass, message, ex, reportEntryVisitors);
		public void Info(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Info, message, ex, reportEntryVisitors);
		public void Warn(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Warn, message, ex, reportEntryVisitors);
		public void Fail(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Fail, message, ex, reportEntryVisitors);

		public void Log(ReportLevel level, string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(_reportEntryFactory.Create(level, message, ex), reportEntryVisitors);

		public void Log(ReportEntry reportEntry, params IReportEntryVisitor[] reportEntryVisitors) => _reportFile.Append(reportEntry, reportEntryVisitors);
    }
}