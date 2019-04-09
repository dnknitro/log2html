using System;
using log4net;
using log4net.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace dnk.log2html.Support
{
	public class TestWrapper
	{
		public static void Test(Action testAction)
		{
			Test((string) null, null, testAction);
		}

		public static void Test(string browserName, Action testAction)
		{
			Test(browserName, null, testAction);
		}

		public static void Test(IWebDriver webDriver, Action testAction)
		{
			Test(webDriver, null, testAction);
		}

		public static void Test(IWebDriver webDriver, string testCaseName, Action testAction)
		{
			string browserName = null;
			if (webDriver is RemoteWebDriver remoteWebDriver && remoteWebDriver.Capabilities.HasCapability("browserName")) 
				browserName = remoteWebDriver.Capabilities.GetCapability("browserName").ToString();

			Test(browserName, webDriver, testCaseName, testAction);
		}

		public static void Test(string browserName, string testCaseName, Action testAction)
		{
			Test(browserName, null, testCaseName, testAction);
		}

		private static void Test(string browserName, IWebDriver webDriver, string testCaseName, Action testAction)
		{
			if (!string.IsNullOrWhiteSpace(browserName))
				Report.SetBrowser(browserName);
			if (!string.IsNullOrWhiteSpace(testCaseName))
				Report.SetTestCaseName(testCaseName);

			try
			{
				testAction();
			}
			catch (Exception e)
			{
				LogManager.GetLogger(nameof(TestWrapper)).LogScreenshot(webDriver, Level.Error, "Test case failed", e);
				throw;
			}
			finally
			{
				webDriver?.Dispose();
			}
		}
	}
}