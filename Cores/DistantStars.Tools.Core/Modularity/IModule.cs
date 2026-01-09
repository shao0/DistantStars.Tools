using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 1. 模块接口：所有模块必须实现它
/// </summary>
public interface IModule
{
    /// <summary>
    ///  阶段A：注册服务 (告诉容器我有什功能)
    /// </summary>
    /// <param name="services"></param>
    void RegisterTypes(IServiceCollection services);

    /// <summary>
    /// 阶段B：初始化 (服务已就绪，开始干活)
    /// </summary>
    /// <param name="provider"></param>
    void OnInitialized(IServiceProvider provider);
}