using System;
using System.Threading;

namespace dnk.log2html
{
	public class ReportEntry
	{
		private static int _id;

		public ReportEntry()
		{
			Interlocked.Increment(ref _id);
			ID = _id;
			ThreadName = Thread.CurrentThread.Name;
			TimeStampUtc = DateTime.UtcNow;
		}

		public int ID { get; }
		public string Level { get; set; }
		public int LevelValue { get; set; }
		public string Message { get; set; }

		public string ThreadName { get; set; }

		public string StackTrace { get; set; }
		public DateTime TimeStampUtc { get; set; }
		public string ScreenshotPath { get; set; }
		public string Exception { get; set; }

		public string TestCaseName { get; set; }
		public string Browser { get; set; }
		//public string TestClassFullName { get; }
		//public string TestMethodName { get; }
	}
}