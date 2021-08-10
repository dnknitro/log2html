using System.Reflection;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace dnk.log2html.Support.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class BasicTestFixture
	{
		[Test]
		public void TestScreenshot()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var webDriver = new ChromeDriver();
			TestWrapper.Test(webDriver, () =>
			{
				webDriver.Navigate().GoToUrl("http://google.com");
				Report.Info(prefix + "Log without screenshot");
				Report.Info(prefix + "Log with screenshot", new dnk.log2html.Support.WebDriver.Screenshot(webDriver));
			});
		}

		[Test]
		public void TestFail()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			Assert.Throws<AssertionException>(() =>
			{
				var webDriver = new ChromeDriver();
				TestWrapper.Test(webDriver, () =>
				{
					webDriver.Navigate().GoToUrl("http://google.com");
					Report.Info(prefix + "Log without screenshot");
					Assert.Fail("Test Fail with Screenshot");
				});
			});
		}
	}
}