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
			Report.Configure(
				Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.Trim('\\')).Parent.Parent.Parent.FullName, "Results"),
				new ReportMetaData
				{
					ReportName = "log2html.Support.Test Execution Report",
					ReportEnvironment = "DEV"
				}
			);
		}
	}
}