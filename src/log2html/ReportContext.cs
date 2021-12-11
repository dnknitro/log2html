using System;

namespace dnk.log2html
{
	public class ReportContext
	{
		public static void Configure(ITestStorage testStorage)
		{
			_testStorage = testStorage;
		}

		private static ITestStorage _testStorage;

		public ReportContext(string testCaseName = null, string browser = null)
		{
			TestCaseName = testCaseName;
			Browser = browser;

			if (_testStorage == null)
				throw new NullReferenceException("ReportContext is not configured. Please call ReportContext.Configure first");
			_testStorage.Set(ReportContextKey, this);
		}

		public string TestCaseName { get; set; }
		public string Browser { get; set; }

		private const string ReportContextKey = "ReportContext";
		public static ReportContext Current => _testStorage.Get<ReportContext>(ReportContextKey);
	}
}