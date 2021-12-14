//using System;
//using dnk.log2html.Support.NUnit;
//using Microsoft.Extensions.DependencyInjection;

//namespace dnk.log2html.Support
//{
//    /*
//	new ServiceCollection()
//		.AddLog2HtmlReportNUnit("log2html.Test Execution Report", "N/A")
//		.BuildServiceProvider()
//		.ConfigureServiceProvider();
//	*/

//    public static class DependencyInjectionSupport
//	{
//		public static IServiceProvider ServiceProvider { get; private set; }

//		public static ServiceCollection AddLog2HtmlReportNUnit(this ServiceCollection services, string reportName, string reportEnvironment)
//		{
//			services.AddSingleton(services2 => new ReportMetaData
//			{
//				ReportName = reportName,
//				ReportEnvironment = reportEnvironment
//			});
//			services.AddSingleton<ReportImpl>();
//			services.AddSingleton<ReportTemplate>();
//			services.AddSingleton<ReportFile>();
//			services.AddSingleton<IReportEntryFactory, ReportEntryFactory>();
//			services.AddScoped<ITestCaseName, NUnitTestCaseName>();
//			services.AddSingleton<ITestStorage, NUnitTestStorage>();
//			return services;
//		}

//		public static IServiceProvider ConfigureServiceProvider(this IServiceProvider serviceProvider)
//		{
//			ServiceProvider = serviceProvider;
//			Report.Configure(ServiceProvider.GetRequiredService<ReportImpl>());
//			ReportContext.Configure(ServiceProvider.GetRequiredService<ITestStorage>());
//			return ServiceProvider;
//		}
//	}
//}