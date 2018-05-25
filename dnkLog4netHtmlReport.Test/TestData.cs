using System;
using System.IO;
using System.Text;
using dnkUtils;
using log4net;
using NUnit.Framework;

namespace dnkLog4netHtmlReport.SeleniumWebDriver.Test
{
	public class TestData
	{
		[Test]
		[Explicit]
		public void SetHardcodedTestData()
		{
			var reportRecords = ResourceUtils.ReadStringFromEmbeddedResource("dnkLog4netHtmlReport.Test.TestData.txt", GetType().Assembly);
			StringBuilder reportTemplateContent = null;
			HtmlReportAppender.Configure(
				content =>
				{
					reportTemplateContent = content;
					content.Replace("{\"EndOfReportData\":true}", reportRecords + "{\"EndOfReportData\":true}");
				}
			);
			LogManager.GetLogger(GetType()).Pass("SetHardcodedTestData!");
			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "dnkLog4netHtmlReport", "ReportTemplate.html"), reportTemplateContent.ToString());
		}
	}
}