using System;
using System.Reflection;
using log4net;
using log4net.Core;

namespace dnk.DynamicLog4netReport
{
	public static class LogExtensions
	{
		public const string ScreenshotPathPropertyName = "ScreenshotPath";

		private static readonly Level failSkip = new Level(10, "FailSkip");

		public static void FailSkip(this ILog log, string message, Exception ex = null)
		{
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, failSkip, message, ex);
		}

		//public static void Error(this ILog log, string message, Exception ex = null, string pathToScreenshot = null)
		//{
		//	var led = new LoggingEventData()
		//	{
		//		Level = Level.Error
		//	};
		//	var le = new LoggingEvent(MethodBase.GetCurrentMethod().DeclaringType, log.Logger.Repository, led);
		//	log.Logger.Log(le);
		//}

		public static void Screenshot(this ILog log, Level level, string message, Exception ex = null)
		{
			var screenshotPath = "actual path to file here";
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = screenshotPath; 
			log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, level, message, ex);
			LogicalThreadContext.Properties[ScreenshotPathPropertyName] = null;
		}
	}
}