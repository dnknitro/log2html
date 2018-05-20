using dnk.DynamicLog4netReport;
using log4net;
using log4net.Core;
using NUnit.Framework;

namespace DynamicLog4netReport.Test
{
	public class DynamicAppenderTestFixture
	{
		[Test]
		public void TestAppend()
		{
			var log = LogManager.GetLogger(GetType().Name);
			log.Info("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.FailSkip("Fail Skip test");
			log.Screenshot(Level.Debug, "Debug");
		}
	}
}