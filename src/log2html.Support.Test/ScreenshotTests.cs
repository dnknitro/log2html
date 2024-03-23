using dnk.log2html.Support.WebDriver;
using OpenQA.Selenium.Chrome;
using System.Reflection;

namespace dnk.log2html.Support.Test;

[Parallelizable(ParallelScope.Children)]
public class ScreenshotTests
{
    [Test]
    public void TestScreenshot()
    {
        var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
        ChromeDriver webDriver = new();
        TestWrapper.RunTest(webDriver, () =>
        {
            webDriver.Navigate().GoToUrl("http://google.com");
            Report.Info(prefix + "Log without screenshot");
            Report.Info(prefix + "Log with screenshot", new Screenshot(webDriver));
        });
    }
}