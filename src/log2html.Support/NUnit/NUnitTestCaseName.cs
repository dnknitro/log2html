using NUnit.Framework;

namespace dnk.log2html.Support.NUnit
{
	public class NUnitTestCaseName : ITestCaseName
	{
		public string GetName() => TestContext.CurrentContext.Test.Name;
	}
}