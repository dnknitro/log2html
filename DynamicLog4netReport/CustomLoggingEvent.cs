using System;
using log4net;
using log4net.Core;
using NUnit.Framework;

namespace dnk.DynamicLog4netReport
{
	internal class CustomLoggingEvent
	{
		public CustomLoggingEvent(int id, LoggingEvent loggingEvent)
		{
			ID = id;
			Level = loggingEvent.Level.Name;
			LevelValue = loggingEvent.Level.Value;
			Message = loggingEvent.RenderedMessage;
			ThreadName = loggingEvent.ThreadName;
			StackTrace = loggingEvent.ExceptionObject?.StackTrace;
			TimeStampUtc = loggingEvent.TimeStampUtc;
			ScreenshotPath = LogicalThreadContext.Properties[LogExtensions.ScreenshotPathPropertyName]?.ToString();

			FullTestCaseName = TestContext.CurrentContext?.Test?.FullName ?? "TestCaseNA";
			//TestClassFullName = TestContext.CurrentContext?.Test?.ClassName ?? "ClassNameNA";
			//TestMethodName = TestContext.CurrentContext?.Test?.MethodName ?? "MethodNameNA";
		}

		public int ID { get; }
		public string Level { get; }
		public int LevelValue { get; }
		public string Message { get; }
		public string ThreadName { get; }
		public string StackTrace { get; }
		public DateTime TimeStampUtc { get; }
		public string ScreenshotPath { get; }

		public string FullTestCaseName { get; }
		//public string TestMethodName { get; }
		//public string TestClassFullName { get; }
	}
}