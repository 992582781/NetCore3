using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tool;
using RongKang.UnitOfWork;
using RongKangBusiness.Extensions;

namespace RongKangBusiness
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//��ȡcontenex
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped(typeof(IRepository.IRepository<>), typeof(Repository.Repository<>));

            #region ע��cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            //services.ConfigAspectCorebak();
            services.AddMemoryCache();
            services.AddControllersWithViews()
                .AddControllersAsServices();//����Ҫд;
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            //����κ�Autofacģ���ע�ᡣ
            //������ConfigureServices֮����õģ�����
            //�ڴ˴�ע�Ὣ������ConfigureServices��ע������ݡ�
            //�ڹ�������ʱ������á�UseServiceProviderFactory��new AutofacServiceProviderFactory��������`���򽫲�����ôˡ�

            builder.RegisterModule(new AutofacModuleRegister(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath,
                new List<string>()
                   { //�������캯��ע��
                               "RongKang.Repository.dll","RongKang.UnitOfWork.dll"
                   }));


            //builder.RegisterType<IHttpContextAccessor>()
            //       .As<HttpContextAccessor>()
            //       .AsImplementedInterfaces()//��ʼ����ע��
            //       .InstancePerLifetimeScope();//��Ϊÿһ����������ô���һ����һ�Ĺ����ʵ��


            builder.RegisterDynamicProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            IContext.Accessor = accessor;//ȫ��ʹ��
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
