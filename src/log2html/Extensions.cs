namespace dnk.log2html;

public static class Extensions
{
	public static string NormalizeTestCaseName(this ITestCaseName testCaseNameProvider, string customTestCaseName)
	{
		var testCaseName = !string.IsNullOrWhiteSpace(customTestCaseName)
			? ReportContext.Current.TestCaseName
			: testCaseNameProvider?.GetName() ?? "Test Case Name not set";
		if (testCaseName?.Contains("AdhocContext.AdhocTestMethod") == true) testCaseName = "Main";
		return testCaseName;
	}
}