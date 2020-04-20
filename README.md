# ASP.NET Core 开发实战

## .NET Core概述
 - .NET Core能做什么(七龙珠)
 
 ![.NET Core能做什么](https://github.com/chenyanbo1024/ASP.NET.Core/blob/master/image/01-what-core-can-do.png ".NET Core能做什么")
 - .NET Core 的版本历史
 
 ![版本历史](https://github.com/chenyanbo1024/ASP.NET.Core/blob/master/image/01-version-history.png "版本历史")
 - .NET Core 开发工具介绍
   - Visual Studio（Community，Professional，Enterprise）
   - Visual Studio for Mac
   - Visual Studio Code
 
## Startup：理解程序启动的过程
 - Program.cs
   - Main方法：程序唯一入口
   - 调用了 **CreateHostBuilder** 方法：创建主机生成器，返回 **IHostBuilder** 
   - **IHostBuilder** ：包含6个方法(重点关注加粗的方法)
     - **ConfigureAppConfiguration**
     - ConfigureContainer
     - **ConfigureHostConfiguration**
     - **ConfigureServices**
     - UseServiceProviderFactory
     - UseServiceProviderFactory
     - 上面这6个方法是按照一定顺序执行的，并不是根据我们自己调用顺序来执行
 - Startup.cs
   - 首先，Startup类，不是必须的，我们也可以省略，但需要在Program类中添加对服务的注册等操作，将其单独作为一个Startup类，只是为了结构更加合理
   - Configure：服务的注册
   - ConfigureService：配置服务
   - 应用程序启动时，会调用shagn上面这两个方法
 - 启动过程执行顺序
   - ConfigureWebHostDefaults：注册了应用程序必要的组件（配置的组件、容器的组件等）
   - ConfigureHostConfiguration：配置应用程序启动时必要的配置
     - 应用程序启动时所需要监听的端口、URL
     - 嵌入我们自己的内容注入到框架中
   - ConfigureAppConfiguration：让我们自己嵌入自己的配置的文件，供应用程序读取
   - ConfigureServices、ConfigureLogging、Startup、Startup.ConfigureServices
     - 往容器中注入我们应用的组件
  - Startup.Configure：注入中间件，处理HttpContext整个请求过程

## 依赖注入：良好架构的起点
 - 为什么要使用依赖注入框架
   - 借助依赖注入框架，我们可以轻松管理类之间的依赖，帮助我们在构建应用时遵循设计原则，确保代码的可维护性和可扩展性。
   - ASP .NET Cored的整个架构中，依赖注入框架提供了对象创建和生命周期管理的核心能力，各个组件相互鞋协作，也是由依赖注入框架的能力来实现的
   - 博客园好文：https://www.cnblogs.com/jesse2013/p/di-in-aspnetcore.html
 - 组件包
   - 应用了 “接口实现分离”这一经典的设计模式
   - 抽象包：Microsoft.Extensions.DependencyInjection.Abstractions
   - 具体实现包：Microsoft.Extensions.DependencyInjection.Abstractions
 - 核心类型
   - IServiceCollection：服务的注册
   - ServiceDescriptor：每个服务注册时的信息
   - IServiceProvider：容器，注册的服务都可以通过容器获取到，由 ServiceCollection Build 产生
   - IServiceScope：子容器的生命周期
 - 生命周期(SeriviceLifetime)
   - 单例 Singleton：整个应用程序生命周期以内只创建一个实例 
   - 作用域 Scoped：每个请求都创建新的实例，在同一请求中，只有同一实例
   - 瞬时(暂时) Transient：每个注入都会创建一个新实例
 - 服务注册
   - 常用注册：AddSingleton、AddScoped、AddTransient
   - 尝试注册：TryAddSingleton、TryAddScoped、TryAddTransient
   - 移除和替换：Replace、RemoveAll
   - 泛型注册：AddSingleton(typeof(IService<>), typeof(Service<>));
 - 注意点
   - 避免通过静态属性的方式访问容器对象
   - 避免在服务内部使用 GetService 方式来获取实例
   - 避免使用静态属性存储单例，应该使用容器管理单例对象
   - 避免在服务中实例化依赖对象，应该使用依赖注入来获得依赖对象
   - 避免向单例的类型注入范围的类型
   
## 依赖注入：理解作用域与对象释放行为
 - IServiceScope：作用域由它承载
 - 实现 IDisposable 接口类型的释放
   - DI容器只负责释放由其创建的对象实例
   - DI容器在其或者其子容器释放时，释放由其创建的对象实例
 - 注意点
   - 避免在根容器中获取实现了 IDisposable 的瞬时任务
   - 避免手动创建实现了 IDisposable 对象，应该使用容器来管理对象的生命周期
   - 坑：实现了 IDisposable 接口的服务，如果时注册瞬时的，又在根容器去做操作，它会一直保持到应用程序退出的时候，才能够被回收掉

## 依赖注入：使用 Autofac 增强容器能力
 - 什么时候需要第三方容器组件
   - 大多数情况下，默认的容器组件就可以实现我们的需求
   - 基于名称的注入:需要一个服务通过名称来区分不同实现的时候
   - 基于属性的注入:直接把服务注册到某个类的属性中
   - 子容器
   - 基于动态代理AOP
 - 核心扩展点
   - interface IServiceProviderFactory<TContainerBuilder>
   - 第三方容器都是通过这个作为核心扩展点
 - Autofac
   - .NET社区中最老牌的容器之一
   - 两个包
     - AutofacExtensions.DependencyInjection
     - Autofac.Extras.DynamicProxy：动态代理包
   - 在.Net Core中的使用
     - Program.cs中使用 UseServiceProviderFactory注册第三方容器
     - Startup.cs中添加 ConfigureContainer(ContainerBuilder builder)
     - 注册
       - 普通注册:builder.RegisterType<TService>().As<ITService>();
       - 命名注册
         - 同一服务注册多次，同时使用不同的名称来区分时，可使用命名注册
         - builder.RegisterType<TService>().Named<ITService>("serviceName");
         - 获取服务:在根容器中使用Resolve这一组方法即可获取
       - 属性注册:builder.RegisterType<TService>().As<ITService>().PropertiesAutowired;
       - AOP
         - IInterceptor
           - Autofac面向切面编程最重要的一个接口
           - 通过该接口，我们可以把逻辑在不改变原有类的基础上注入到方法的切面上
           - Proceed()
         - 切面的启用
           - 注册拦截器到容器中
           - builder.RegisterType<TService>().As<ITService().PropertiesAutowired().InterceptedBy(typeof(TInterceptor)).EnableInterfaceInterceptors();
         - 拦截器的类型：接口拦截器（常用）、类拦截器                                                                                                                             
       - 子容器
         - Autofac支持我们将服务注册到特定的子容器中，这样其他子容器是获取不到当前子容器的这个服务
         - builder.RegisterType<TService>().InstancePerMatchingLifetimeScope("子容器名称")   
 
    - ConfigureServices
      - 默认的容器注入
      - 但最终注入的这些服务都会被Autofac接替

## 配置框架：让服务无缝适应各种环境
 - 核心组件包
   - “接口实现设计分离”设计模式
   - Microsoft.Extensions.Configuration. Abstractions：抽象包
   - Microsoft.Extensions.Configuration：实现包
 - 配置框架
   - key-value 字符串键值对的方式抽象了配置，使我们只需要关注键值对的内容即可
   - 支持从各种不同的数据源读取配置，例如从命令行读取、从文件读取等等
 - 配置框架核心类型
   - IConfiguration
   - IConfigurationRoot:配置的根、读取配置的动作都是从这个接口中读取
   - IConfigurationSection:当配置不只是简单的键值对时，可用section来进行分组，在json中很常用
   - IConfigurationBuilder:构建配置的核心
 - 配置框架扩展点
   - IConfigurationSource
   - IConfigurationProvider
## 配置框架：使用命令行配置提供程序接收命令行参数
 - 支持的命令格式
   - 无前缀的 key=value 模式
   - 双中横线模式 --key=value 或 --key value
   - 正斜杠模式 /key=value 或 /key value
   - 备注： 等号分隔符和空格分隔符不能混用
 - 命令替换模式
   - 必须以单划线 (-) 或双划线 (--) 开头
   - 映射字典不能包含重复 Key
## 配置框架：使用环境变量配置提供程序接收环境变量
 - 环境变量包：Configuration.EnvironmentVariables
 - 适合场景
   - 在Docker中运行
   - 在 Kubernetes 中运行时
   - 需要设置 ASP.NET Core 的一些内置特殊配置时
 - 特性
   - 对于配置的分层键，支持用双下横线 “__” 代替 “:”:获取环境变量时，依然使用":",环境变量则使用"__"来进行分层
   - 支持根据前缀加载:可通过前缀过滤的方式只加载环境变量中前缀为指定的前缀才会加载，而不是加载整个系统中的整个环境变量
## 配置框架：使用文件配置提供程序读取配置文件
 - 文件配置提供程序
   - Microsoft.Extensions.Configuration.Ini
   - Microsoft.Extensions.Configuration.Json
   - Microsoft.Extensions.Configuration.NewtonsoftJson
   - Microsoft.Extensions.Configuration.Xml
   - Microsoft.Extensions.Configuration.UserSecrets
 - 特性
   - 指定文件可选、必选：optional参数
   - 指定是否监视文件的变更：reloadOnchange参数
   - 后添加的配置会覆盖原配置
## 配置框架：跟踪配置变更实现配置热更新
## 配置框架：使用强类型对象承载配置数据
## 配置框架：自定义配置数据源与配置中心方案
## 选项框架：使用选项框架解耦服务与配置
## 选项框架：选项数据的热更新
## 选项框架：为选项数据添加验证
## 日志框架：聊聊记日志的最佳姿势
## 日志作用域：解决不同请求之间的日志干扰
## 结构化日志组件 Serilog：记录对查询分析友好的日志
## 中间件：掌控请求处理过程的关键
## 异常处理中间件：区分真异常与逻辑异常
## 静态文件中间件：前后端分离开发合并部署骚操作
## 文件提供程序：让你可以将文件放在任何地方
## 路由与终结点：如何规划好你的 WebAPI
