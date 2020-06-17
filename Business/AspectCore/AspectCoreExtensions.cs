using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore
{
	public static class AspectCoreExtensions
	{
		public static void ConfigAspectCore(this IServiceCollection services)
		{
			services.ConfigureDynamicProxy(config =>
			{
				//TestInterceptor拦截器类
				//拦截代理所有Service结尾的类
				config.Interceptors.AddTyped<TransactionalAttribute>();
			});
			//services.BuildAspectCoreServiceProvider();
		}
	}
}
