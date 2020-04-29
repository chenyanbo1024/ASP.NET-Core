using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Services
{
    public class MyService
    {
    }

    // 单例服务
    public interface IMySingletonService { }
    public class MySingletonService : IMySingletonService { }

    // 作用域服务
    public interface IMyScopedService { }
    public class MyScopedService : IMyScopedService { }

    //瞬时服务
    public interface IMyTransientService { }
    public class MyTransientService : IMyTransientService { }

}
