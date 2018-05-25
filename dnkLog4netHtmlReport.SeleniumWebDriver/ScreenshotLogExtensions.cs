using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Core;
using OpenQA.Selenium;

namespace dnkLog4netHtmlReport.SeleniumWebDriver
{
	public static class ScreenshotLogExtensions
	{
		public static void LogWithScreenshot(this ILog log, IWebDriver webDriver, Level level, string message, Exception ex = null)
		{
			string targetScreeshotFile = null;
			try
			{
				var tempScreenshotFile = TakeScreenshot(webDriver);
				if (File.Exists(tempScreenshotFile))
				{
					var targetScreenshotRelativeFolder = Config.ReportFileNameOnly;
					var targetScreenshotAbsoluteFolder = Path.Combine(Config.ReportFolder, targetScreenshotRelativeFolder);
					Directory.CreateDirectory(targetScreenshotAbsoluteFolder);

					var screenshotFileName = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}_{Thread.CurrentThread.ManagedThreadId}.png";
					File.Move(tempScreenshotFile, Path.Combine(targetScreenshotAbsoluteFolder, screenshotFileName));

					targetScreeshotFile = Path.Combine(targetScreenshotRelativeFolder, screenshotFileName).Replace("\\", "/");
				}
			}
			catch (Exception screenshotException)
			{
				LogManager.GetLogger(typeof(ScreenshotLogExtensions)).Debug("Failed to take screenshot", screenshotException);
			}

			const string ScreenshotPathPropertyName = "ScreenshotPath";
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = targetScreeshotFile;
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, level, message, ex);
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = null;
		}

		/// <returns>Path to screenshot in system %temp% folder</returns>
		private static string TakeScreenshot(params IWebDriver[] webDriver)
		{
			if (!webDriver.Any())
				return null;

			//const string RemoteAutomationFolderName = "Automation";
			//var screenshotFileName = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}_{Thread.CurrentThread.ManagedThreadId}.png";

			var screenshotTempFiles = webDriver.Cast<ITakesScreenshot>().Select(delegate(ITakesScreenshot x)
			{
				var screenshot = x.GetScreenshot();
				var screenshotTempFile = Path.GetTempFileName();
				screenshot.SaveAsFile(screenshotTempFile, ScreenshotImageFormat.Png);
				return screenshotTempFile;
			}).ToList();

			string tempScreeenshotFile;
			if (screenshotTempFiles.Count > 1)
			{
				tempScreeenshotFile = Path.GetTempFileName();
				var finalScreenshot = MergeImages(screenshotTempFiles.Select(Image.FromFile).ToList());
				finalScreenshot.Save(tempScreeenshotFile);
			}
			else
			{
				tempScreeenshotFile = screenshotTempFiles.Single();
			}
			return tempScreeenshotFile;
		}

		private static Image MergeImages(List<Image> images)
		{
			var totalWidth = images.Max(x => x.Width);
			var totalHeight = images.Sum(x => x.Height);

			var totalImage = new Bitmap(totalWidth, totalHeight);
			using (var g = Graphics.FromImage(totalImage))
			{
				var verticalLocation = 0;
				foreach (var image in images)
				{
					g.DrawImage(image, 0, verticalLocation);
					verticalLocation += image.Height;
				}
			}
			return totalImage;
		}
	}
}