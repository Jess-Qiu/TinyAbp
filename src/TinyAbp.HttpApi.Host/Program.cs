using Serilog;
using TinyAbp.HttpApi.Host;

// 显示应用程序启动横幅
Console.WriteLine(
    """
     _____ ___ _  ___   __    _   ___ ___   ___                                  _   
    |_   _|_ _| \| \ \ / /   /_\ | _ ) _ \ | __| _ __ _ _ __  _____ __ _____ _ _| |__
      | |  | || .` |\ V /   / _ \| _ \  _/ | _| '_/ _` | '  \/ -_) V  V / _ \ '_| / /
      |_| |___|_|\_| |_|   /_/ \_\___/_|   |_||_| \__,_|_|_|_\___|\_/\_/\___/_| |_\_\
                                                                                     
    """
);

// 创建Web应用程序构建器并注册日志、依赖注入容器和监听地址
var builder = WebApplication
    .CreateBuilder(args)
    .RegisterSerilogLogger()
    .RegisterAutofacContainer()
    .RegisterListenAddress();

// 记录当前主机启动环境
Log.Information($"当前主机启动环境 - {builder.Environment.EnvironmentName}");

try
{
    // 添加应用程序模块
    await builder.Services.AddApplicationAsync<TinyAbpHttpApiHostModule>();

    // 构建Web应用程序
    var app = builder.Build();

    // 初始化应用程序
    await app.InitializeApplicationAsync();

    // 运行应用程序
    await app.RunAsync();
}
catch (Exception ex)
{
    // 记录致命错误
    Log.Fatal(ex.Message);
}
finally
{
    // 关闭并刷新日志
    await Log.CloseAndFlushAsync();
}
