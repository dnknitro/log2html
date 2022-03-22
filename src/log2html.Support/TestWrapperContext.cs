using System;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace dnk.log2html.Support;

public record TestWrapperContext
{
	public TestWrapperContext(Action testAction)
	{
		TestAction = () =>
		{
			testAction();
			return Task.CompletedTask;
		};
	}

	public TestWrapperContext(Func<Task> testAction)
	{
		TestAction = testAction;
	}

	public Func<Task> TestAction { get; set; }
	public int MaxRetries { get; set; } = 1;
	public int RetryCounter { get; set; } = 0;
	public string TestCaseName { get; set; }
	public string BrowserName { get; set; }
	public IWebDriver WebDriver { get; init; }
}