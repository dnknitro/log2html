using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace dnk.log2html.Support.Test
{
	public abstract class BaseApiTest
	{
		[Test]
		public async Task Test()
		{
			await RunTest(TestBody, GetType().Name);
		}

		protected virtual Task TestBody()
		{
			throw new NotImplementedException();
		}

		protected async Task RunTest(Func<Task> testAction, string? customTestName = null)
		{
			await TestWrapper.RunTest(new TestWrapperContext(testAction) {TestCaseName = customTestName});
		}
	}
}
