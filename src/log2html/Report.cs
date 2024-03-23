using System;

namespace dnk.log2html;

public static class Report
{
    public static ReportImpl ReportInstance { get; private set; }

    public static void Configure(ReportImpl report) => ReportInstance = report;

    public static void Debug(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Debug, message, ex, reportEntryVisitors);

    public static void Pass(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Pass, message, ex, reportEntryVisitors);

    public static void Info(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Info, message, ex, reportEntryVisitors);

    public static void Retry(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Retry, message, ex, reportEntryVisitors);

    public static void Warn(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Warn, message, ex, reportEntryVisitors);

    public static void Fail(string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Fail, message, ex, reportEntryVisitors);

    public static void Debug(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Debug, message, null, reportEntryVisitors);

    public static void Pass(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Pass, message, null, reportEntryVisitors);

    public static void Info(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Info, message, null, reportEntryVisitors);

    public static void Retry(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Retry, message, null, reportEntryVisitors);

    public static void Warn(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Warn, message, null, reportEntryVisitors);

    public static void Fail(string message, params IReportEntryVisitor[] reportEntryVisitors) => Log(ReportLevel.Fail, message, null, reportEntryVisitors);

    public static void Log(ReportLevel level, string message, Exception ex = null, params IReportEntryVisitor[] reportEntryVisitors) => ReportInstance.Log(level, message, ex, reportEntryVisitors);

    public static void Log(ReportEntry reportEntry, params IReportEntryVisitor[] reportEntryVisitors) => ReportInstance.Log(reportEntry, reportEntryVisitors);

    public static void AddLevelsSummary() => ReportInstance.AddLevelsSummary();

    public static void Open() => ReportInstance.Open();
}