using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace dnk.log2html;

public class ReportImpl
{
	public ReportImpl(ReportFile reportFile, IReportEntryFactory reportEntryFactory, ITestStorage testStorage)
	{
		_reportFile = reportFile;
		ReportEntryFactory = reportEntryFactory;
		ReportContext.Configure(testStorage);
		Report.Configure(this);
	}

	private readonly ReportFile _reportFile;
	public ITestCaseName TestCaseNameProvider => ReportEntryFactory.TestCaseNameProvider;
	public IReportEntryFactory ReportEntryFactory { get; }

	private readonly ConcurrentBag<ReportEntry> _reportEntries = new();

	public void Debug(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportLevel.Debug, message, ex, reportEntryVisitors);
	}

	public void Pass(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportLevel.Pass, message, ex, reportEntryVisitors);
	}

	public void Info(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportLevel.Info, message, ex, reportEntryVisitors);
	}

	public void Warn(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportLevel.Warn, message, ex, reportEntryVisitors);
	}

	public void Fail(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportLevel.Fail, message, ex, reportEntryVisitors);
	}

	public void Log(ReportLevel level, string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors)
	{
		Log(ReportEntryFactory.Create(level, message, ex), reportEntryVisitors);
	}

	public void Log(ReportEntry reportEntry, params IReportEntryVisitor[] reportEntryVisitors)
	{
		foreach (var reportEntryVisitor in reportEntryVisitors)
			reportEntryVisitor.Visit(reportEntry, _reportFile);

		_reportEntries.Add(reportEntry);

		_reportFile.Append(reportEntry);
	}

	public void AddLevelsSummary()
	{
		var levelsSummary = _reportEntries
			.GroupBy(x => x.TestCaseName)
			.Select(x => x.Max(y => y.LevelValue))
			.GroupBy(x => x)
			.Select(x => new
			{
				LevelValue = x.Key,
				Level = ((ReportLevel) x.Key).ToString(),
				Count = x.Count()
			})
			.ToArray();
		_reportFile.Replace("{Server Side LevelsBrowsers}", JsonConvert.SerializeObject(levelsSummary));
	}

	public void Open()
	{
		var psi = new ProcessStartInfo(_reportFile.ReportFilePath) {UseShellExecute = true};
		Process.Start(psi);
	}
}