namespace dnk.log2html.Support.Test;

public class ApiTest1 : BaseApiTest
{
    protected override Task TestBody()
    {
        Report.Pass("Testing test name 1");
        return Task.CompletedTask;
    }
}