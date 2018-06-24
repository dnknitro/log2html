using log4net;

namespace $rootnamespace$
{
	public class Logger
	{
		public static readonly ILog Log = LogManager.GetLogger(nameof(Logger));
	}
}