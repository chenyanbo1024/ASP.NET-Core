using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace geektime_netcore
{
    public class Program
    {
        /// <summary>
        /// 程序唯一入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 返回 IHostBulider ：应用程序启动的核心
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    Console.WriteLine("执行方法：ConfigureHostConfiguration");
                })
                .ConfigureServices(services =>
                {
                    Console.WriteLine("执行方法：ConfigureServices");
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    Console.WriteLine("执行方法：ConfigureLogging");
                })
                .ConfigureAppConfiguration((hostBuilderContext, configurationBinder) =>
                {
                    Console.WriteLine("执行方法：ConfigureAppConfiguration");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Console.WriteLine("执行方法：ConfigureWebHostDefaults");
                    webBuilder.UseStartup<Startup>();
                });

        // 1. IHostBuilder 共有6个方法
        //      分别为：Build、ConfigureAppConfiguration、ConfigureContainer、ConfigureHostConfiguration、ConfigureServices、UseServiceProviderFactory
        //      重点关注：ConfigureAppConfiguration、ConfigureHostConfiguration、ConfigureServices

        // 2. 几个方法的是有一定的调用顺序，并不是由我们写入的顺序来调用

        // 3. ConfigureWebHostDefaults：注册了应用程序必要的几个组件，比如配置的组件、容器组件等
        //     ConfigureHostConfiguration：用于配置应用程序启动时必要的配置，比如应用程序启动时所需要监听的端口，URL 地址在这个过程可以嵌入一些自己配置的内容，注入到配置的框架中
        //     ConfigureAppConfiguration：用于嵌入自己的配置文件，供应用程序读取
        //     ConfigureServices, ConfigureLogging, Startup, Startup.ConfigureServices：
        //     Startup.Configure：用于注入中间件，处理 HttpContext 整个请求过程

        // 4. Startup
        //      非必须的，可省略，让代码结构更合理而已
        //      ConfigureServices、Configure：应用程序启动时会调用这两个方法

    }
}
