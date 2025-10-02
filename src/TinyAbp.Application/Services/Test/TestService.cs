using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Test.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace Services.Test;

/// <summary>
/// 测试服务
/// 实现测试相关的业务逻辑
/// </summary>
public class TestService : ApplicationService, ITestService
{
    private readonly IDistributedCache<string> _cache;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache">分布式缓存</param>
    public TestService(IDistributedCache<string> cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 获取应用程序名称
    /// </summary>
    /// <returns></returns>
    public string GetApplicationName() => "Hello TinyAbp";

    /// <summary>
    /// 异常测试
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void GetExceptionTest()
    {
        throw new Exception("This is a test exception from TinyAbp.");
    }

    /// <summary>
    /// 获取用户名
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetUserName(string name) => $"My name is {name}!";

    /// <summary>
    /// 你是谁
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    [HttpGet("hello-who/{who}")]
    public string HelloWho(string who) => $"Hello, {who}!";

    /// <summary>
    /// Hello World
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello-world")]
    public string HelloWorld() => "Hello TinyAbp";

    /// <summary>
    /// 测试对象映射
    /// </summary>
    /// <returns></returns>
    public IActionResult GetObjectMapping()
    {
        var output = ObjectMapper.Map(
            new TestInput { Name = "TinyAbp", Name1 = "Hello" },
            new TestOutput()
        );

        return new ObjectResult(output);
    }

    /// <summary>
    /// 设置缓存项
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <returns>设置结果</returns>
    public async Task<string> PostCacheItemAsync(string key, string value)
    {
        await _cache.SetAsync(key, value);

        return $"已设置 {key} 缓存: {await _cache.GetAsync(key)}";
    }

    /// <summary>
    /// 测试分布式锁
    /// </summary>
    /// <returns></returns>
    public async Task PostCheckDistributedLock() { }
}
