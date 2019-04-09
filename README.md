# log2html

## Description
Library to generate an HTML report based on [log4net](https://logging.apache.org/log4net/) records during application or tests ([NUnit](http://nunit.org/)) execution.

## Example
[ReportExample.html](https://nitro.duckdns.org/log2html/ReportExample.html)

## Installation
It can be installed through [nuget.org](https://www.nuget.org/packages/log2html/).
During package installation it adds two files to the project:
 * *log4net.config* is an XML file containing configuration for log4net library. Its property "Copy to Output Directory" is set to "Copy if newer". The vital part of the file is HtmlReportAppender which logs log4net records into an HTML report.
 * *GlobalTestSetup.cs* is a NUnit OneTimeSetUp implementation which sets up HTML report title, category, and environment values. If project log2html is installed into is console application - call to `log2html.Config.Configure()` method should be made at the beginning of the Main() method.
 * Also *Properties\AssemblyInfo.cs* file is updated: `[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]` is added to the end of the file

## Configuration
At the beginning of application/tests execution global report configuration should be invoked:
```C#
var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
log2html.Config.Configure(
	Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName, "Results"),
	new ReportMetaData
	{
		ReportName = "Test Execution Report TEST",
		ReportCategory = "Test",
		ReportEnvironment = "DEV"
	}
);
```

In the example above report HTML report folder location is specified:
```C#
Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName, "Results")
```
which is located 3 parant folders up the folder tree related to `\Bin\Debug` folder.
If the Debug folder location is `C:\Projects\MySolution\MyProject.Tests\Bin\Debug` then the Results folder location will be `C:\Projects\MySolution\Results`.

Additionally "browser" can be specified for tests which use Selenium WebDriver:
```C#
[Test]
public void TestAppend1()
{
	Config.SetBrowser("IE");
  ...
}
```

## Usage
To add record to the report simply create an instance of log4net logger `var log = LogManager.GetLogger("myLoggerName");` and then call `log.Info("My log entry");`.
Besides standard log4net levels the library adds a few [extension methods](https://github.com/dnknitro/log2html/blob/master/src/log2html/LogExtensions.cs) `Pass()` and `Fail()` which can be used just like standard levels: `log.Pass("Foo found!");`.

To add browser screenshot (when using with Selenium WebDriver) to the report record an additional package [log2html.Support](https://www.nuget.org/packages/log2html.Support/) should be installed. 
Then method `LogWithScreenshot()` can be invoked with desired Log Level and message:
```C#
log.LogScreenshot(webDriver, LogLevel.Pass, "Log with screenshot");
```
