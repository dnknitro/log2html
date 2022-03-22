//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;
//using NUnit.Framework;
//using NUnit.Framework.Interfaces;
//using NUnit.Framework.Internal;
//using NUnit.Framework.Internal.Builders;

//namespace dnk.log2html.Support.NUnit
//{
//	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
//	public class NUnitFixtureBuilderAttribute : NUnitAttribute, IFixtureBuilder
//	{
//		private static IServiceProvider _serviceProvider;

//		static NUnitFixtureBuilderAttribute()
//		{
//			//_serviceProvider = serviceProvider;
//		}

//		public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
//		{
//			//var nUnitTestFixtureBuilder = new NUnitTestFixtureBuilder();
//			//var testSuite = nUnitTestFixtureBuilder.BuildFrom(typeInfo, new TestAttributePreFilter());

//			var nUnitTestCaseBuilder = new NUnitTestCaseBuilder();

//			var testSuite = new TestFixture(typeInfo, GetConstructorParameters(typeInfo.Type));
//			foreach (var method in typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Instance))
//				if (method.GetCustomAttributes<TestAttribute>(false).Any())
//				{
//					var testMethod = nUnitTestCaseBuilder.BuildTestMethod(method, testSuite, null);
//					testSuite.Add(testMethod);
//				}

//			yield return testSuite;
//		}

//		private static object[] GetConstructorParameters(Type type)
//		{
//			var constructors = type.GetConstructors();
//			if (constructors.Length != 1) throw new NotImplementedException("GetConstructorParameters not implemented for classes with multiple constructors");
//			var constructor = constructors.Single();
//			var parameterInfos = constructor.GetParameters();
//			if (!parameterInfos.Any()) return null;
//			var parameterValues = new List<object>();
//			foreach (var parameterInfo in parameterInfos)
//			{
//				//var parameterValue = _serviceProvider.GetRequiredService(parameterInfo.ParameterType);
//				object parameterValue = null;
//				parameterValues.Add(parameterValue);
//			}

//			return parameterValues.ToArray();
//		}
//	}
//}