using System;
using System.IO;
using NUnit.Framework;

namespace dnk.log2html.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
			//var configFile = new FileInfo(Path.Combine(baseDirectory, "log4net.config"));
			//XmlConfigurator.Configure(configFile);
			Report.Configure(
				Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName, "Results"),
				new ReportMetaData
				{
					ReportName = "log2html.Test Execution Report",
					ReportEnvironment = "DEV"
				}
			);
		}
	}
}