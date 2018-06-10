using System;
using System.IO;
using System.Text;
using dnkUtils;
using log4net;
using NUnit.Framework;

namespace dnk.log2html.Test
{
	public class TestData
	{
		[Test]
		[Explicit]
		public void SetHardcodedTestData()
		{
			var reportRecords = ResourceUtils.ReadStringFromEmbeddedResource("log2html.Test.TestData.txt", GetType().Assembly);
			StringBuilder reportTemplateContent = null;
			HtmlReportAppender.Configure(
				content =>
				{
					reportTemplateContent = content;
					content.Replace("{\"EndOfReportData\":true}", reportRecords + "{\"EndOfReportData\":true}");
				}
			);
			LogManager.GetLogger(GetType()).Pass("SetHardcodedTestData!");
			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "log2html", "ReportTemplate.html"), reportTemplateContent.ToString());
		}
	}
}