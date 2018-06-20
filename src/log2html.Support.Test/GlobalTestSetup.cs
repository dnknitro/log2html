using System;
using System.IO;
using NUnit.Framework;

namespace dnk.log2html.Support.Test
{
	[SetUpFixture]
	public class GlobalTestSetup
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
			dnk.log2html.Report.Configure(
				Path.Combine(Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName, "Results"),
				new dnk.log2html.ReportMetaData
				{
					ReportName = "Test Execution Report TEST",
					ReportEnvironment = "DEV"
				}
			);
		}
	}
}