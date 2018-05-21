using System.Collections.Generic;
using System.Reflection;
using dnk.DynamicLog4netReport;
using log4net;
using log4net.Core;
using NUnit.Framework;

namespace DynamicLog4netReport.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class DynamicAppenderTestFixture
	{
		[Test]
		public void TestAppend1()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			log.SetBrowser("IE");
			log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.Fail(prefix + "Fail Skip test");
			log.Screenshot(Level.Debug, prefix + "Debug");
		}

		[Test]
		public void TestAppend2()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			log.SetBrowser("FireFox");
			log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.Fail(prefix + "Fail Skip test");
		}
		[Test]
		public void TestAppend3()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			log.SetBrowser("Chrome");
			log.Warn(prefix + "Warn test");
			log.Screenshot(Level.Debug, prefix + "Debug");

			var levels = new List<Level>()
			{
				Level.Off,
				Level.All,
				Level.Verbose,
				Level.Finer,
				Level.Trace,
				Level.Fine,
				Level.Debug,
				Level.Info,
				Level.Notice,
				Level.Finest,
				Level.Error,
				Level.Severe,
				Level.Critical,
				Level.Alert,
				Level.Fatal,
				Level.Emergency,
				Level.Log4Net_Debug,
				Level.Warn
			};
			foreach (var level in levels)
			{
				log.Info($"{level.Value} = {level.Name}");
			}
		}

		[Test]
		public void TestAppend4()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			log.SetBrowser("Chrome");
			log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.Pass(prefix + "Fail Skip test");
		}
	}
}