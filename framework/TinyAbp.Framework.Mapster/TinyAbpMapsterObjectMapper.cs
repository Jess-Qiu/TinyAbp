using MapsterMapper;
using Volo.Abp.ObjectMapping;

namespace TinyAbp.Framework.Mapster;

/// <summary>
/// TinyAbp Mapster 对象映射器
/// 实现 IObjectMapper 接口，提供对象映射功能
/// </summary>
public class TinyAbpMapsterObjectMapper : IObjectMapper
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="autoObjectMappingProvider">自动对象映射提供程序</param>
    public TinyAbpMapsterObjectMapper(IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        AutoObjectMappingProvider = autoObjectMappingProvider;
    }

    /// <summary>
    /// 自动对象映射提供程序
    /// </summary>
    public IAutoObjectMappingProvider AutoObjectMappingProvider { get; set; }

    /// <summary>
    /// 映射对象
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(TSource source) =>
        AutoObjectMappingProvider.Map<TSource, TDestination>(source);

    /// <summary>
    /// 映射对象到现有目标
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="destination">目标对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) =>
        AutoObjectMappingProvider.Map<TSource, TDestination>(source, destination);
}
