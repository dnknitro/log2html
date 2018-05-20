using System.IO;
using log4net.Core;
using Newtonsoft.Json;

namespace dnk.DynamicLog4netReport
{
	public class PatternLayoutConverter : log4net.Layout.Pattern.PatternLayoutConverter
	{
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			var cle = new CustomLoggingEvent(loggingEvent);
			writer.Write(JsonConvert.SerializeObject(cle));
		}
	}
}