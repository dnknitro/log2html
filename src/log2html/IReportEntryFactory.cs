using System;
using dnkUtils.Diagnostics;

namespace dnk.log2html
{
	public interface IReportEntryFactory
	{
		ReportEntry Create(ReportLevel level, string message, Exception ex = null);
	}

	public class ReportEntryFactory : IReportEntryFactory
	{
		public ReportEntryFactory(ITestCaseName testCaseNameProvider)
		{
			_testCaseNameProvider = testCaseNameProvider;
		}

		private readonly ITestCaseName _testCaseNameProvider;

		public ReportEntry Create(ReportLevel level, string message, Exception ex = null)
		{
			var testCaseName = !string.IsNullOrWhiteSpace(ReportContext.Current?.TestCaseName)
				? ReportContext.Current.TestCaseName
				: _testCaseNameProvider?.GetName() ?? "Test Case Name not set";
			if (testCaseName?.Contains("AdhocContext.AdhocTestMethod") == true) testCaseName = "Main";

			var reportEntry = new ReportEntry
			{
				Message = message,
				Exception = ex?.ToNiceString(),
				StackTrace = ex?.StackTrace,
				Level = level.ToString().ToUpper(),
				LevelValue = (int) level,
				TestCaseName = testCaseName,
				Browser = ReportContext.Current?.Browser,
			};

			return reportEntry;
		}
	}
}