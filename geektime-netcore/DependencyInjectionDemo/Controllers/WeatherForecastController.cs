using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DependencyInjectionDemo.Services;

namespace DependencyInjectionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 为了对比每次获取到的服务是否一致，所以每个服务获取两次
        /// HashCode可以判断是否为同一对象
        /// </summary>
        [HttpGet]
        [Route("GetService")]
        public int GetService(
            [FromServices]IMySingletonService singletonService1,
            [FromServices]IMySingletonService singletonService2,
            [FromServices]IMyScopedService scopedService1,
            [FromServices]IMyScopedService scopedService2,
            [FromServices]IMyTransientService transientService1,
            [FromServices]IMyTransientService transientService2)
        {
            Console.WriteLine($"singleton1:{singletonService1.GetHashCode()}");
            Console.WriteLine($"singleton2:{singletonService2.GetHashCode()}");
            Console.WriteLine($"scoped1:{scopedService1.GetHashCode()}");
            Console.WriteLine($"scoped2:{scopedService2.GetHashCode()}");
            Console.WriteLine($"transient1:{transientService1.GetHashCode()}");
            Console.WriteLine($"transient2:{transientService2.GetHashCode()}");
            Console.WriteLine($"========请求结束========");

            //第一次打印：
            //singleton1: 3129430
            //singleton2: 3129430
            //scoped1: 37901460
            //scoped2: 37901460
            //transient1: 4436986
            //transient2: 10553853
            //========请求结束========

            //第二次打印：
            //singleton1: 3129430
            //singleton2: 3129430
            //scoped1: 39086322
            //scoped2: 39086322
            //transient1: 36181605
            //transient2: 28068188
            //========请求结束========

            // 结论：
            // 1.singleton：在应用程序创建至销毁整个生命周期中都是同一个服务
            // 2.scoped：在同一请求中为同一服务，不用请求不同服务
            // 3.transient：每获取一次，创建一个新的服务


            return 0;
        }
    }
}
