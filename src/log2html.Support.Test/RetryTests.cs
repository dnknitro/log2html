using System;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace dnk.log2html.Support.Test
{
	[Parallelizable(ParallelScope.Children)]
	public class RetryTests
	{
		[Test]
		public async Task TestOneTry()
		{
			var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
			var retryCounter = 0;

			void TestAction()
			{
				retryCounter++;
				Report.Pass(prefix + $" retryCounter={retryCounter}");
			}

			await TestWrapper.RunTest(new TestWrapperContext(TestAction) {MaxRetries = 3, TestCaseName = "Test to check PASS status"});
			retryCounter.ShouldBe(1);
		}

		[Test]
		public async Task TestRetry()
		{
			var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
			var retryCounter = 0;

			void TestAction()
			{
				retryCounter++;
				Report.Pass(prefix + $" retryCounter={retryCounter}");
				if (retryCounter < 3) throw new InvalidOperationException("Retry test exception");
			}

			await TestWrapper.RunTest(new TestWrapperContext(TestAction) {MaxRetries = 3, TestCaseName = "Test to check RETRY status"});
			retryCounter.ShouldBe(3);
		}

		[Test]
		public void TestMaxRetry()
		{
			var prefix = MethodBase.GetCurrentMethod()?.Name + ": ";
			var retryCounter = 0;

			void TestAction()
			{
				retryCounter++;
				Report.Pass(prefix + $" retryCounter={retryCounter}");
				throw new InvalidOperationException("Retry test exception");
			}

			Action shouldThrowAction = () => TestWrapper.RunTest(new TestWrapperContext(TestAction) {MaxRetries = 3, TestCaseName = "Test to check FAIL status after max retries reached"}).GetAwaiter().GetResult();
			var invalidOperationException = shouldThrowAction.ShouldThrow<InvalidOperationException>();
			invalidOperationException.Message.ShouldBe("Retry test exception");
			retryCounter.ShouldBe(3);
		}
	}
}