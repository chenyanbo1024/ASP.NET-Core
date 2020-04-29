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
            #region ע�᲻ͬ�������ڵķ���

            // Transient���� ˲ʱ���� ע��Ϊ ˲ʱģʽ
            services.AddTransient<IMyTransientService, MyTransientService>();

            // Scope���� ��������� ע��Ϊ ������ģʽ
            services.AddScoped<IMyScopedService, MyScopedService>();

            // Singleton���� �������� ע��Ϊ ����ģʽ
            services.AddSingleton<IMySingletonService, MySingletonService>();

            #endregion

            #region ��ʽע��
            services.AddSingleton<IOrderService>(new OrderService());   //ֱ��ע��ʵ��
            services.AddSingleton<IOrderService,OrderServiceEx>();
            #endregion

            #region ����ע��
            //��������
            //1.�ӿ�������ͬ���򲻻����ע��
            //2.��ͬ���ͽӿڣ���ͬʵ���࣬��ע��
            services.TryAddSingleton<IOrderService, OrderService>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            #endregion

            #region �Ƴ����滻ע��
            services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            services.RemoveAll<IOrderService>();
            #endregion

            #region ����ע��
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
