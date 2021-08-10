namespace dnk.log2html
{
	public interface IReportEntryVisitor
	{
		void Visit(ReportEntry reportEntry, ReportFile reportFile);
	}
}