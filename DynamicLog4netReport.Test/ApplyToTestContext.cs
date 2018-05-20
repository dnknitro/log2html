using log4net;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace DynamicLog4netReport.Test
{
	public class ApplyToTestContext : IApplyToContext
	{
		public void ApplyToContext(TestExecutionContext context)
		{
			LogManager.GetLogger(context.CurrentTest.Name).Info("Starting test");
		}
	}
}