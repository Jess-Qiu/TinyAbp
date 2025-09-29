using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Services;

public interface ITestService : IApplicationService, ITransientDependency
{
    /// <summary>
    /// 获取应用程序名称
    /// </summary>
    /// <returns></returns>
    public string GetApplicationName();

    /// <summary>
    /// 测试异常抛出
    /// </summary>
    public void GetExceptionTest();

    /// <summary>
    /// Hello World (使用 HttpGet("hello-world"))
    /// </summary>
    /// <returns></returns>
    public string HelloWorld();

    /// <summary>
    /// Hello Who
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public string HelloWho(string who);

    /// <summary>
    /// GetUserName HttpGet("get-username/{name}")
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetUserName(string name);
}
