using System;

namespace dnk.log2html
{
	public class ReportContext : IDisposable
	{
		public ReportContext(string testCaseName = null, string browser = null)
		{
			TestCaseName = testCaseName;
			Browser = browser;

			_reportContext = this;
		}

		public bool IsCustomTestCaseName { get; set; }
		public string TestCaseName { get; set; }
		public string Browser { get; set; }

		[ThreadStatic] private static ReportContext _reportContext;

		public static ReportContext Current => _reportContext;


		public void Dispose()
		{
			_reportContext = null;
		}
	}
}