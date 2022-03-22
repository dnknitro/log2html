using System.Threading.Tasks;

namespace dnk.log2html.Support.Test
{
	public class ApiTest2 : BaseApiTest
	{
		protected override Task TestBody()
		{
			Report.Pass("Testing test name 2");
			return Task.CompletedTask;
		}
	}
}