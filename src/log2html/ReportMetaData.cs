using System;

namespace dnk.log2html;

public class ReportMetaData
{
    public string ReportName { get; set; }
    public string ReportEnvironment { get; set; }
    public DateTime ReportStartDateTime { get; } = DateTime.Now;
    public string ReportTitle => $"{ReportStartDateTime:HH:mm} {ReportName} {ReportEnvironment}";
}