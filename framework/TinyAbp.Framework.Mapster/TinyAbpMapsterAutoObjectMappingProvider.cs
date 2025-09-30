using Mapster;
using Volo.Abp.ObjectMapping;

namespace TinyAbp.Framework.Mapster;

/// <summary>
/// TinyAbp Mapster 自动对象映射提供程序
/// 实现 IAutoObjectMappingProvider 接口，提供自动对象映射功能
/// </summary>
public class TinyAbpMapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
{
    /// <summary>
    /// 映射对象到目标类型
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(object source) => source.Adapt<TDestination>();

    /// <summary>
    /// 映射对象到现有目标对象
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="destination">目标对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) =>
        source.Adapt(destination);
}
