namespace dnk.log2html.Support.NUnit
{
	public class NUnitReportImpl : ReportImpl
	{
		public NUnitReportImpl(ReportTemplate reportTemplate, string reportFileNameOnly = null, string reportFolder = null)
			: base(
				new ReportFile(reportTemplate, reportFileNameOnly, reportFolder),
				new ReportEntryFactory(new NUnitTestCaseName()),
				new NUnitTestStorage()
			)
		{
		}

		public NUnitReportImpl(ReportMetaData reportMetaData, string reportFileNameOnly = null, string reportFolder = null)
			: base(
				new ReportFile(new ReportTemplate(reportMetaData), reportFileNameOnly, reportFolder),
				new ReportEntryFactory(new NUnitTestCaseName()),
				new NUnitTestStorage()
			)
		{
		}
	}
}