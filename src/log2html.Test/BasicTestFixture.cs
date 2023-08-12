using System;
using System.Reflection;
using Bogus;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace dnk.log2html.Test
{
    [Parallelizable(ParallelScope.Children)]
    public class BasicTestFixture
    {
        [Test]
        [TestCase(1, null)]
        [TestCase(2, "Oops it did it again")]
        [TestCase(3, "One more")]
        [TestCase(4, null)]
        [TestCase(5, null)]
        [TestCase(6, "Final fail!")]
        public void TestAppend1(int index, string failMessage)
        {
            var prefix = $"{MethodBase.GetCurrentMethod()?.Name}-{index}: ";
            Report.Info(prefix + "Lorem ipsum dolor sit amet, <b>consectetur</b> adipisicing elit,<br/>sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            if (!string.IsNullOrWhiteSpace(failMessage))
            {
                try
                {
                    throw new InvalidOperationException(failMessage);
                }
                catch (Exception e)
                {
                    Report.Fail(e.Message, e);
                }
            }
            else
            {
                Report.Pass("No Fails!");
            }
        }

        [Test]
        [TestCase("CustomTestCaseNameSetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestNameSetVeryLongNoSpacesCustomTestName")]
        [TestCase("<b style='color: red'>CustomTestCaseName</b> Test Case Name")]
        public void CustomTestCaseName(string testCaseName)
        {
            new ReportContext(testCaseName, "FireFox");

            var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
            Report.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            Report.Fail(prefix + "Fail Skip test");
        }

        [Test]
        [TestCase(ReportLevel.Debug)]
        [TestCase(ReportLevel.Info)]
        [TestCase(ReportLevel.Pass)]
        [TestCase(ReportLevel.Warn)]
        [TestCase(ReportLevel.Fail)]
        public void TestAppend3(ReportLevel level)
        {
            var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";

            Report.Log(level, $"{prefix} Test Log Level");
        }

        [Test]
        public void TestAppend4()
        {
            var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";

            var persons = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                persons.Add(new Bogus.Faker().Person);
            }
            Report.Info(JsonConvert.SerializeObject(persons, Formatting.Indented).AsCode("json"));
            Report.Info(prefix + "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            Report.Pass(prefix + "Fail Skip test");
        }
    }
}