using System;
using log4net;
using log4net.Core;

namespace dnk.DynamicLog4netReport
{
	internal class CustomLoggingEvent
	{
		public CustomLoggingEvent(LoggingEvent loggingEvent)
		{
			Message = loggingEvent.RenderedMessage;
			ThreadName = loggingEvent.ThreadName;
			StackTrace = loggingEvent.ExceptionObject?.StackTrace;
			TimeStampUtc = loggingEvent.TimeStampUtc;
			ScreenshotPath = LogicalThreadContext.Properties[LogExtensions.ScreenshotPathPropertyName]?.ToString();
		}

		public string Message { get; set; }
		public string ThreadName { get; set; }
		public string StackTrace { get; set; }
		public DateTime TimeStampUtc { get; set; }
		public string ScreenshotPath { get; set; }
	}
}