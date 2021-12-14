//using System;
//using System.Linq;
//using System.Reflection;
//using NUnit.Framework;
//using NUnit.Framework.Interfaces;

//namespace dnk.log2html.Support.NUnit
//{
//	public class TestAttributePreFilter : IPreFilter
//	{
//		public bool IsMatch(Type type)
//		{
//			throw new NotImplementedException();
//		}

//		public bool IsMatch(Type type, MethodInfo method)
//		{
//			return method.GetCustomAttributes<TestAttribute>(false).Any();
//		}
//	}
//}