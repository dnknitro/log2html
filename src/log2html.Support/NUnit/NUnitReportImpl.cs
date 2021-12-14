namespace dnk.log2html.Support.NUnit
{
	public class NUnitReportImpl : ReportImpl
	{
		public NUnitReportImpl(ReportTemplate reportTemplate)
			: base(
				new ReportFile(reportTemplate),
				new ReportEntryFactory(new NUnitTestCaseName()),
				new NUnitTestStorage()
			)
		{
		}

		public NUnitReportImpl(ReportMetaData reportMetaData)
			: base(
				new ReportFile(new ReportTemplate(reportMetaData)),
				new ReportEntryFactory(new NUnitTestCaseName()),
				new NUnitTestStorage()
			)
		{
		}
	}
}