using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Test.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace Services.Test;

/// <summary>
/// 测试服务
/// </summary>
public class TestService : ApplicationService, ITestService
{
    private readonly IDistributedCache<string> _cache;

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

    public async Task<string> PostCacheItemAsync(string key, string value)
    {
        await _cache.SetAsync(key, value);

        return $"已设置 {key} 缓存: {await _cache.GetAsync(key)}";
    }
}
