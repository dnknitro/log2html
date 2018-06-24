using System.Reflection;
using log4net;
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
				var log = LogManager.GetLogger(GetType());
				webDriver.Navigate().GoToUrl("http://google.com");
				log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
				log.LogScreenshot(webDriver, LogLevel.Pass, "Log with screenshot");
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
					var log = LogManager.GetLogger(GetType());
					webDriver.Navigate().GoToUrl("http://google.com");
					log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
					Assert.Fail("Test Fail with Screenshot");
				});
			});
		}
	}
}