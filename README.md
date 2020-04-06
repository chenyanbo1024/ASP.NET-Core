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
