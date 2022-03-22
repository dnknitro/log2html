using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Screenshot = dnk.log2html.Support.WebDriver.Screenshot;

namespace dnk.log2html.Support;

public class TestWrapper
{
	public static void RunTest(Action testAction)
	{
		RunTest(new TestWrapperContext(testAction)).GetAwaiter().GetResult();
	}

	public static void RunTest(IWebDriver webDriver, Action testAction)
	{
		RunTest(new TestWrapperContext(testAction) {WebDriver = webDriver}).GetAwaiter().GetResult();
	}

	public static async Task RunTest(TestWrapperContext testWrapperContext)
	{
		if (string.IsNullOrEmpty(testWrapperContext.BrowserName)
		    && testWrapperContext.WebDriver is RemoteWebDriver remoteWebDriver
		    && remoteWebDriver.Capabilities.HasCapability("browserName")
		   )
			testWrapperContext.BrowserName = remoteWebDriver.Capabilities.GetCapability("browserName").ToString();

		// ReSharper disable once ObjectCreationAsStatement
		new ReportContext(testWrapperContext.TestCaseName, testWrapperContext.BrowserName);

		try
		{
			await TestWithRetries(testWrapperContext);
		}
		catch (Exception e)
		{
			Report.Fail($"Test '{Report.ReportInstance.TestCaseNameProvider.NormalizeTestCaseName(testWrapperContext.TestCaseName)}' failed", e, new Screenshot(testWrapperContext.WebDriver));
			throw;
		}
		finally
		{
			testWrapperContext.WebDriver?.Dispose();
		}
	}

	private static async Task TestWithRetries(TestWrapperContext testWrapperContext)
	{
		for (;;)
			try
			{
				await testWrapperContext.TestAction();
				break;
			}
			catch (Exception retryException)
			{
				testWrapperContext.RetryCounter++;
				if (testWrapperContext.RetryCounter >= testWrapperContext.MaxRetries)
				{
					if (testWrapperContext.MaxRetries > 1)
						Report.Info($"Max Retries reached ({testWrapperContext.RetryCounter}/{testWrapperContext.MaxRetries})");
					throw;
				}

				Report.Retry($"Retrying because of exception {testWrapperContext.RetryCounter}/{testWrapperContext.MaxRetries}: {retryException.Message}", retryException);
			}
	}
}