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

    /// <summary>
    /// Wraps code string with code tags for farther processing on UI using https://highlightjs.org/
    /// </summary>
    public static string AsCode(this string code, string language = null)
    {
        var languageClass = !string.IsNullOrWhiteSpace(language) ? $" class=\"language-{language}\"" : "";
        return $"<pre><code {languageClass}>{code}</code></pre>";
    }
}