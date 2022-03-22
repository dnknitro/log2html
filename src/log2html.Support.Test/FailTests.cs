using System;
using System.Reflection;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using Shouldly;

namespace dnk.log2html.Support.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class FailTests
	{
		[Test]
		public void TestFail()
		{
			var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
			Action testDelegate = () =>
			{
				var webDriver = new ChromeDriver();
				TestWrapper.RunTest(webDriver, () =>
				{
					webDriver.Navigate().GoToUrl("http://google.com");
					Report.Info(prefix + "Log without screenshot");
					throw new InvalidOperationException("Test Fail with Screenshot");
				});
			};
			var exception = testDelegate.ShouldThrow<InvalidOperationException>();
			exception.Message.ShouldStartWith("Test Fail with Screenshot");
		}
	}
}