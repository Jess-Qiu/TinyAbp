using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace TinyAbp.Framework.Mapster;

/// <summary>
/// TinyAbp Framework Mapster 模块
/// 配置和注册 Mapster 对象映射服务
/// </summary>
public class TinyAbpFrameworkMapsterModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册 Mapster 相关服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        // 扫描当前程序集，注册 Mapster 配置
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        // 注册全局 TypeAdapterConfig 配置为单例
        context.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

        // 注册 Mapster IMapper 接口
        context.Services.AddTransient<IMapper, Mapper>();

        // 替换默认的对象映射器为 TinyAbp 自定义实现
        context.Services.Replace(
            ServiceDescriptor.Transient<IObjectMapper, TinyAbpMapsterObjectMapper>()
        );

        // 替换自动对象映射提供程序为 TinyAbp 自定义实现
        context.Services.Replace(
            ServiceDescriptor.Transient<
                IAutoObjectMappingProvider,
                TinyAbpMapsterAutoObjectMappingProvider
            >()
        );

        await base.ConfigureServicesAsync(context);
    }
}
