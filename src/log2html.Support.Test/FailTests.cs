using OpenQA.Selenium.Chrome;
using System.Reflection;

namespace dnk.log2html.Support.Test;

[Parallelizable(ParallelScope.Children)]
public class FailTests
{
    [Test]
    public void TestFail()
    {
        var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
        Action testDelegate = () =>
        {
            ChromeDriver webDriver = new();
            try
            {
                TestWrapper.RunTest(webDriver, () =>
                {
                    webDriver.Navigate().GoToUrl("http://google.com");
                    Report.Info(prefix + "Log without screenshot");
                    throw new InvalidOperationException("Test Fail with Screenshot");
                });
            }
            finally
            {
                webDriver?.Dispose();
            }
        };
        var exception = testDelegate.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldStartWith("Test Fail with Screenshot");
    }
}