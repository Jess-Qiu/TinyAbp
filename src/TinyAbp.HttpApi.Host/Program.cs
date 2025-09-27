// 主程序入口点
using TinyAbp.Framework.AspNetCore.Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args).RegisterAutofacContainer().RegisterListenAddress();

var app = builder.Build();

app.Run();
