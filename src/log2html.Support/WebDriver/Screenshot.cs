﻿using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace dnk.log2html.Support.WebDriver;

public class Screenshot : IReportEntryVisitor
{
    public Screenshot(params IWebDriver[] webDrivers) => _webDrivers = webDrivers;

    private readonly IWebDriver[] _webDrivers;

    public void Visit(ReportEntry reportEntry, ReportFile reportFile)
    {
        var takesScreenshots = _webDrivers.Cast<ITakesScreenshot>().Where(x => x != null).ToArray();

        if (!takesScreenshots.Any())
        {
            return;
        }

        var screenshotFilePaths = takesScreenshots.Select(takesScreenshot =>
        {
            var screenshot = takesScreenshot.GetScreenshot();
            var screenshotFilePath = Path.GetTempFileName();
            screenshot.SaveAsFile(screenshotFilePath);
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