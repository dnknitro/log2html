using log4net.Appender;
using log4net.Core;

namespace dnk.DynamicLog4netReport
{
	public class DynamicAppender : RollingFileAppender
	{
		//public DynamicAppender()
		//{
		//	_fileAppender = new FileAppender();
		//	_fileAppender.File = 
		//}

		//private readonly FileAppender _fileAppender;

		protected override void Append( LoggingEvent loggingEvent )
		{
			base.Append(loggingEvent);
		}
	}
}
