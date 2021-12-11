# log2html

## Description
Library to generate an HTML report based on [log4net](https://logging.apache.org/log4net/) records during application or tests ([NUnit](http://nunit.org/)) execution.

## Example
[ReportExample.html](https://dnknitro.net/log2html/ReportExample.html)

## Installation
Please see [log2html @ nuget.org](https://www.nuget.org/packages/log2html/) for installation instructions.

## Configuration
At the beginning of application/tests execution global report configuration should be invoked:
```C#
var reportMetaData = new ReportMetaData
{
	ReportName = "log2html.Test Execution Report",
	ReportEnvironment = "DEV"
};
var report = new ReportImpl(
	new ReportFile(new ReportTemplate(reportMetaData)),
	new ReportEntryFactory(new NUnitTestCaseName()),
	new NUnitTestStorage()
);
Report.Configure(report);

```

In the example above report HTML report folder location is specified:
```C#
Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName, "Results")
```
which is located 3 parent folders up the folder tree related to `\Bin\Debug` folder.
If the Debug folder location is `C:\Projects\MySolution\MyProject.Tests\Bin\Debug` then the Results folder location will be `C:\Projects\MySolution\Results`.

Additionally "browser" can be specified for tests which use Selenium WebDriver:
```C#
[Test]
public void TestAppend1()
{
  new ReportContext(testCaseName, "FireFox");
  ...
}
```

## Usage
```C#
Report.Pass("Hello World!");
```

To add browser screenshot (when using with Selenium WebDriver) to the report record an additional package [log2html.Support](https://www.nuget.org/packages/log2html.Support/) should be installed.
```C#
Report.Info("Log with screenshot", new dnk.log2html.Support.WebDriver.Screenshot(webDriver));
```
