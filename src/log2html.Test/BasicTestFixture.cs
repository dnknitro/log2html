using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using log4net.Core;
using NUnit.Framework;

namespace dnk.log2html.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class BasicTestFixture
	{
		[Test]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		public void TestAppend1(int index)
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			Config.SetBrowser("IE");
			log.Info(prefix + "Lorem ipsum dolor sit amet, <b>consectetur</b> adipisicing elit,<br/>sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			if (index > 4)
				try
				{
					throw new InvalidOperationException("test exception");
				}
				catch (Exception e)
				{
					log.Fail(e.Message, e);
				}
			else
				log.Pass("No Fails!");
		}

		[Test]
        [TestCase("SetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestName")]
        [TestCase("<b style='color: red'>Custom</b> Test Case Name")]

		public void CustomTestCaseName(string testCaseName)
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			Config.SetBrowser("FireFox");
			Config.SetTestCaseName(testCaseName);
			log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.Fail(prefix + "Fail Skip test");
		}

		[Test]
		public void TestAppend3()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			Config.SetBrowser("Chrome");
			log.Warn(prefix + "Warn test");

			var levels = new List<Level>
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
				log.Info($"{level.Value} = {level.Name}");
		}

		[Test]
		public void TestAppend4()
		{
			var prefix = MethodBase.GetCurrentMethod().Name + ": ";
			var log = LogManager.GetLogger(GetType().Name);
			Config.SetBrowser("Chrome");
			log.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
			log.Pass(prefix + "Fail Skip test");
		}
	}
}