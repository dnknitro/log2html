using System;
using dnkUtils.Diagnostics;

namespace dnk.log2html;

public interface IReportEntryFactory
{
	ReportEntry Create(ReportLevel level, string message, Exception ex = null);
	ITestCaseName TestCaseNameProvider { get; }
}

public class ReportEntryFactory : IReportEntryFactory
{
	public ReportEntryFactory(ITestCaseName testCaseNameProvider)
	{
		TestCaseNameProvider = testCaseNameProvider;
	}

	public ITestCaseName TestCaseNameProvider { get; }

	public ReportEntry Create(ReportLevel level, string message, Exception ex = null) =>
		new()
		{
			Message = message,
			Exception = ex?.ToNiceString(),
			StackTrace = ex?.StackTrace,
			Level = level.ToString().ToUpper(),
			LevelValue = (int) level,
			TestCaseName = TestCaseNameProvider.NormalizeTestCaseName(ReportContext.Current?.TestCaseName),
			Browser = ReportContext.Current?.Browser
		};
}