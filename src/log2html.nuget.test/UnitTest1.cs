using Bogus;
using dnk.log2html;

namespace log2html.nuget.test;

public class Tests
{
    [Test]
    public void TestLotsOfRecordsWithFailTowardsTheEnd()
    {
        var faker = new Faker();
        for (var i = 0; i < 100; i++)
        {
            Report.Info($"<b>Starting test #<span style='font-size: 150%;'>{i + 1}</span>...</b>");
            Report.Info(faker.Company.CompanyName());
        }

        Report.Pass("Pass maybe?");
        Report.Fail("Oh no :(");
        Report.Retry("Oops");
        for (var i = 0; i < 50; i++)
        {
            Report.Debug($"bug debug {i + 1}");
        }
        Report.Pass("100% this time");
    }

    [Test]
    public void TestAllReportLevels()
    {
        var reportLevels = Enum.GetValues<ReportLevel>();

        foreach (var reportLevel in reportLevels)
        {
            Report.Log(reportLevel, $"{reportLevel}\t{(int)reportLevel}");
        }
    }
}