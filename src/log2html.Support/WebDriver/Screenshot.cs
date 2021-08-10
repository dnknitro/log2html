using System;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace dnk.log2html.Support.WebDriver
{
	public class Screenshot : IReportEntryVisitor
	{
		public Screenshot(params IWebDriver[] webDrivers)
		{
			_webDrivers = webDrivers;
		}

		private readonly IWebDriver[] _webDrivers;

		public void Visit(ReportEntry reportEntry, ReportFile reportFile)
		{
			if (!_webDrivers.Any())
				return;

			var screenshotFilePaths = _webDrivers.Take(1).Cast<ITakesScreenshot>().Select(delegate(ITakesScreenshot x)
			{
				var screenshot = x.GetScreenshot();
				var screenshotFilePath = Path.GetTempFileName();
				screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
				return screenshotFilePath;
			}).ToArray();

			for (var i = 0; i < screenshotFilePaths.Length; i++)
			{
				var screenshotFilePath = screenshotFilePaths[i];
				if (File.Exists(screenshotFilePath))
				{
					var targetScreenshotRelativeFolder = reportFile.ReportFileNameOnly;
					var targetScreenshotAbsoluteFolder = Path.Combine(reportFile.ReportFolder, targetScreenshotRelativeFolder);
					Directory.CreateDirectory(targetScreenshotAbsoluteFolder);

					var screenshotFileName = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}_{Thread.CurrentThread.ManagedThreadId}.png";
					File.Move(screenshotFilePath, Path.Combine(targetScreenshotAbsoluteFolder, screenshotFileName));

					screenshotFilePaths[i] = Path.Combine(targetScreenshotRelativeFolder, screenshotFileName).Replace("\\", "/");
				}
			}

			reportEntry.ScreenshotPath = string.Join(";", screenshotFilePaths);
		}
	}
}