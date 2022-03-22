using System;
using System.Reflection;

namespace dnk.log2html.Support.log4net
{
	public static class LogExtensions
	{
		public const string ScreenshotPathPropertyName = "ScreenshotPath";
		public const string BrowserPropertyName = "Browser";
		public const string TestCaseName = "TestCaseName";

		public static void Pass(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevel.Pass, message, ex);
		}

		public static void Retry(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevel.Retry, message, ex);
		}

		public static void Fail(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevel.Fail, message, ex);
		}

		public static void SetBrowser(string browser)
		{
			LogicalThreadContext.Properties[LogExtensions.BrowserPropertyName] = browser;
		}

		public static void SetTestCaseName(string testCaseName)
		{
			LogicalThreadContext.Properties[LogExtensions.TestCaseName] = testCaseName;
		}

	}
}