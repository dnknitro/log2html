using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace dnk.DynamicLog4netReport
{
	public class NUnitActionAttribute : Attribute, ITestAction
	{
		public void BeforeTest(ITest test)
		{
			
		}

		public void AfterTest(ITest test)
		{
		}

		public ActionTargets Targets => ActionTargets.Suite;
	}
}
