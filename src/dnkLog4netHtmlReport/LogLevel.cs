using log4net.Core;

namespace dnkLog4netHtmlReport
{
	public static class LogLevel
	{
		// -2147483648	 ALL
		// 10000		 FINEST
		// 10000		 VERBOSE
		// 20000		 FINER
		// 20000		 TRACE
		// 30000		 DEBUG
		// 30000		 FINE
		// 40000		 INFO
		// 50000		 NOTICE
		public static readonly Level Pass = new Level(55000, "PASS");

		// 60000		 WARN
		// 70000		 ERROR
		public static readonly Level Fail = new Level(75000, "FAIL");

		// 90000		 CRITICAL
		// 80000		 SEVERE
		// 100000		 ALERT
		// 110000		 FATAL
		// 120000		 EMERGENCY
		// 120000		 log4net:DEBUG
		// 2147483647	 OFF
	}
}