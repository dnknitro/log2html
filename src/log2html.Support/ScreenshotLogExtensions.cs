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

namespace dnk.log2html.Support
{
	public static class ScreenshotLogExtensions
	{
		public static void DebugScreenshot(this IWebDriver webDriver, string message, Exception ex = null)
		{
			webDriver.LogScreenshot(Level.Debug, message, ex);
		}

		public static void InfoScreenshot(this IWebDriver webDriver, string message, Exception ex = null)
		{
			webDriver.LogScreenshot(Level.Info, message, ex);
		}

		public static void WarnScreenshot(this IWebDriver webDriver, string message, Exception ex = null)
		{
			webDriver.LogScreenshot(Level.Warn, message, ex);
		}

		public static void ErrorScreenshot(this IWebDriver webDriver, string message, Exception ex = null)
		{
			webDriver.LogScreenshot(Level.Error, message, ex);
		}

		public static void LogScreenshot(this IWebDriver webDriver, Level level, string message, Exception ex = null)
		{
			var log = LogManager.GetLogger(nameof(ScreenshotLogExtensions));
			log.LogScreenshot(webDriver, level, message, ex);
		}

		public static void DebugScreenshot(this ILog log, IWebDriver webDriver, string message, Exception ex = null)
		{
			log.LogScreenshot(webDriver, Level.Debug, message, ex);
		}

		public static void InfoScreenshot(this ILog log, IWebDriver webDriver, string message, Exception ex = null)
		{
			log.LogScreenshot(webDriver, Level.Info, message, ex);
		}

		public static void WarnScreenshot(this ILog log, IWebDriver webDriver, string message, Exception ex = null)
		{
			log.LogScreenshot(webDriver, Level.Warn, message, ex);
		}

		public static void ErrorScreenshot(this ILog log, IWebDriver webDriver, string message, Exception ex = null)
		{
			log.LogScreenshot(webDriver, Level.Error, message, ex);
		}


		public static void LogScreenshot(this ILog log, IWebDriver webDriver, Level level, string message, Exception ex = null)
		{
			string targetScreenshotFile = null;
			if (webDriver != null)
				try
				{
					var tempScreenshotFile = TakeScreenshot(webDriver);
					if (File.Exists(tempScreenshotFile))
					{
						var targetScreenshotRelativeFolder = Report.ReportFileNameOnly;
						var targetScreenshotAbsoluteFolder = Path.Combine(Report.ReportFolder, targetScreenshotRelativeFolder);
						Directory.CreateDirectory(targetScreenshotAbsoluteFolder);

						var screenshotFileName = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}_{Thread.CurrentThread.ManagedThreadId}.png";
						File.Move(tempScreenshotFile, Path.Combine(targetScreenshotAbsoluteFolder, screenshotFileName));

						targetScreenshotFile = Path.Combine(targetScreenshotRelativeFolder, screenshotFileName).Replace("\\", "/");
					}
				}
				catch (Exception screenshotException)
				{
					log.Debug("Failed to take screenshot", screenshotException);
				}

			const string ScreenshotPathPropertyName = "ScreenshotPath";
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = targetScreenshotFile;
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