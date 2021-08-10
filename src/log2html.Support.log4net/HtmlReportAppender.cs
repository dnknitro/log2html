using log4net;

namespace dnk.log2html.Support.log4net
{
	/// <summary>
	///     Used in log4net.config
	/// </summary>
	public class HtmlReportAppender : AppenderSkeleton
	{
		protected override void Append(LoggingEvent loggingEvent)
		{
			var customLoggingEvent = new CustomLoggingEvent();
			customLoggingEvent.Level = loggingEvent.Level.Name;
			customLoggingEvent.LevelValue = loggingEvent.Level.Value;
			customLoggingEvent.Message = loggingEvent.RenderedMessage;
			customLoggingEvent.ThreadName = loggingEvent.ThreadName;
			customLoggingEvent.StackTrace = loggingEvent.ExceptionObject?.StackTrace;
			customLoggingEvent.TimeStampUtc = loggingEvent.TimeStampUtc;
			customLoggingEvent.Exception = loggingEvent.ExceptionObject?.ToNiceString();

			customLoggingEvent.ScreenshotPath = LogicalThreadContext.Properties[LogExtensions.ScreenshotPathPropertyName]?.ToString();
			customLoggingEvent.Browser = LogicalThreadContext.Properties[LogExtensions.BrowserPropertyName]?.ToString();

			customLoggingEvent.TestCaseName = LogicalThreadContext.Properties[LogExtensions.TestCaseName]?.ToString()
			                                  //?? TestContext.CurrentContext?.Test?.FullName 
			                                  ?? "TestCaseNA";

			if (customLoggingEvent.TestCaseName.Contains("AdhocContext.AdhocTestMethod")) customLoggingEvent.TestCaseName = "Main";
			//TestClassFullName = TestContext.CurrentContext?.Test?.ClassName ?? "ClassNameNA";
			//TestMethodName = TestContext.CurrentContext?.Test?.MethodName ?? "MethodNameNA";


			HtmlReport.Append(customLoggingEvent);
		}
	}
}