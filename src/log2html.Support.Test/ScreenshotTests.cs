using System.Reflection;
using dnk.log2html.Support.WebDriver;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace dnk.log2html.Support.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class ScreenshotTests
	{
		[Test]
		public void TestScreenshot()
		{
			var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
			var webDriver = new ChromeDriver();
			TestWrapper.RunTest(webDriver, () =>
			{
				webDriver.Navigate().GoToUrl("http://google.com");
				Report.Info(prefix + "Log without screenshot");
				Report.Info(prefix + "Log with screenshot", new Screenshot(webDriver));
			});
		}
	}
}