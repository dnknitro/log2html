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
			using (var webDriver = new ChromeDriver())
			{
				webDriver.Navigate().GoToUrl("http://google.com");
				var prefix = MethodBase.GetCurrentMethod().Name + ": ";
				Report.SetBrowser("Chrome");
			    Report.Log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			    Report.Log.LogScreenshot(webDriver, LogLevel.Pass, "Log with screenshot");
			}
		}
	}
}