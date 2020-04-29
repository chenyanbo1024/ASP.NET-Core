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
        /// ����Ψһ���
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// ���� IHostBulider ��Ӧ�ó��������ĺ���
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    Console.WriteLine("ִ�з�����ConfigureHostConfiguration");
                })
                .ConfigureServices(services =>
                {
                    Console.WriteLine("ִ�з�����ConfigureServices");
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    Console.WriteLine("ִ�з�����ConfigureLogging");
                })
                .ConfigureAppConfiguration((hostBuilderContext, configurationBinder) =>
                {
                    Console.WriteLine("ִ�з�����ConfigureAppConfiguration");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Console.WriteLine("ִ�з�����ConfigureWebHostDefaults");
                    webBuilder.UseStartup<Startup>();
                });

        // 1. IHostBuilder ����6������
        //      �ֱ�Ϊ��Build��ConfigureAppConfiguration��ConfigureContainer��ConfigureHostConfiguration��ConfigureServices��UseServiceProviderFactory
        //      �ص��ע��ConfigureAppConfiguration��ConfigureHostConfiguration��ConfigureServices

        // 2. ��������������һ���ĵ���˳�򣬲�����������д���˳��������

        // 3. ConfigureWebHostDefaults��ע����Ӧ�ó����Ҫ�ļ���������������õ���������������
        //     ConfigureHostConfiguration����������Ӧ�ó�������ʱ��Ҫ�����ã�����Ӧ�ó�������ʱ����Ҫ�����Ķ˿ڣ�URL ��ַ��������̿���Ƕ��һЩ�Լ����õ����ݣ�ע�뵽���õĿ����
        //     ConfigureAppConfiguration������Ƕ���Լ��������ļ�����Ӧ�ó����ȡ
        //     ConfigureServices, ConfigureLogging, Startup, Startup.ConfigureServices��
        //     Startup.Configure������ע���м�������� HttpContext �����������

        // 4. Startup
        //      �Ǳ���ģ���ʡ�ԣ��ô���ṹ���������
        //      ConfigureServices��Configure��Ӧ�ó�������ʱ���������������

    }
}
