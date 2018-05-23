using System;
using System.Reflection;
using log4net;
using log4net.Core;

namespace dnk.DynamicLog4netReport
{
	public static class LogExtensions
	{
		public const string ScreenshotPathPropertyName = "ScreenshotPath";
		public const string BrowserPropertyName = "Browser";

		// -2147483648	 ALL
		// 10000		 FINEST
		// 10000		 VERBOSE
		// 20000		 FINER
		// 20000		 TRACE
		// 30000		 DEBUG
		// 30000		 FINE
		// 40000		 INFO
		// 50000		 NOTICE
		private static readonly Level pass = new Level(55000, "PASS");
		// 60000		 WARN
		// 70000		 ERROR
		private static readonly Level fail = new Level(75000, "FAIL");
		// 80000		 SEVERE
		// 90000		 CRITICAL
		// 100000		 ALERT
		// 110000		 FATAL
		// 120000		 EMERGENCY
		// 120000		 log4net:DEBUG
		// 2147483647	 OFF

		public static void Pass(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, pass, message, ex);
		}

		public static void Fail(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, fail, message, ex);
		}

		public static void Screenshot(this ILog log, Level level, string message, Exception ex = null)
		{
			var screenshotPath = "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif";
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = screenshotPath;
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, level, message, ex);
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = null;
		}

		public static void SetBrowser(this ILog log, string browser)
		{
			LogicalThreadContext.Properties[BrowserPropertyName] = browser;			
		}
	}
}