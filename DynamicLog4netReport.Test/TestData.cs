using System;
using System.IO;
using System.Text;
using dnkUtils;
using log4net;
using NUnit.Framework;

namespace dnk.DynamicLog4netReport.Test
{
	public class TestData
	{
		[Test]
		[Explicit]
		public void SetHardcodedTestData()
		{
			var reportRecords = ResourceUtils.ReadStringFromEmbeddedResource("dnk.DynamicLog4netReport.Test.TestData.txt", this.GetType().Assembly);
			StringBuilder reportTemplateContent = null;
			HtmlReportAppender.Configure(
				HtmlReportAppender.ReportFolder,
				HtmlReportAppender.ReportMetaData,
				content =>
				{
					reportTemplateContent = content;
					content.Replace("{\"EndOfReportData\":true}", reportRecords + "{\"EndOfReportData\":true}");
				}
			);
			LogManager.GetLogger(GetType()).Pass("SetHardcodedTestData!");
			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "DynamicLog4netReport", "ReportTemplate.html"), reportTemplateContent.ToString());
		}
	}
}