using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 为模块化系统提供扩展方法的静态类
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义 BuildModuleCatalog 扩展方法，用于构建模块目录
/// 2. 定义 RunModularity 扩展方法，用于运行模块化系统
/// 3. 配置依赖注入容器以支持模块化功能
/// </remarks>
public static class ModuleExtensions
{
    /// <summary>
    /// 构建模块目录并配置相关服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>模块目录实例</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 尝试向服务集合添加服务集合自身作为单例
    /// 2. 获取模块目录单例实例
    /// 3. 将模块目录注册为单例服务
    /// 4. 将模块管理器注册为单例服务
    /// 5. 返回模块目录实例
    /// </remarks>
    public static IModuleCatalog BuildModuleCatalog(this IServiceCollection services)
    {
        services.TryAddSingleton(services);
        var moduleCatalog = ModuleCatalog.Instance;
        services.TryAddSingleton<IModuleCatalog>(moduleCatalog);
        services.TryAddSingleton<IModuleManager, ModuleManager>();
        return moduleCatalog;
    }

    /// <summary>
    /// 运行模块化系统
    /// </summary>
    /// <param name="catalog">模块目录实例</param>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 构建服务提供者
    /// 2. 从服务提供者获取模块管理器
    /// 3. 调用模块管理器的 Run 方法
    /// 4. 返回服务集合以支持链式调用
    /// </remarks>
    public static IServiceCollection RunModularity(this IModuleCatalog catalog, IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IModuleManager>().Run();
        return services;
    }
}