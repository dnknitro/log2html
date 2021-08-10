using NUnit.Framework;

namespace dnk.log2html.Support.nunit
{
	public class NUnitTestCaseName : ITestCaseName
	{
		public string GetName() => TestContext.CurrentContext?.Test?.Name  ?? "NUnitTestCaseName N/A";
	}
}