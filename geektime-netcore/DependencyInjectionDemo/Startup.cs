using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DependencyInjectionDemo.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DependencyInjectionDemo
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
            #region 注册不同生命周期的服务

            // Transient：将 瞬时服务 注册为 瞬时模式
            services.AddTransient<IMyTransientService, MyTransientService>();

            // Scope：将 作用域服务 注册为 作用域模式
            services.AddScoped<IMyScopedService, MyScopedService>();

            // Singleton：将 单例服务 注册为 单例模式
            services.AddSingleton<IMySingletonService, MySingletonService>();

            #endregion

            #region 花式注册
            services.AddSingleton<IOrderService>(new OrderService());   //直接注册实例
            services.AddSingleton<IOrderService,OrderServiceEx>();
            #endregion

            #region 尝试注册
            //两者区别
            //1.接口类型相同，则不会进行注册
            //2.相同类型接口，相同实现类，则不注册
            services.TryAddSingleton<IOrderService, OrderService>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            #endregion

            #region 移除和替换注册
            services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            services.RemoveAll<IOrderService>();
            #endregion

            #region 泛型注册
            services.AddSingleton(typeof(IGenericService<>),typeof(GenericService<>));
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
