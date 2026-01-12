using System;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Client.Core.Modularity;

/// <summary>
/// 定义模块的基本接口，所有模块都必须实现此接口
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义 RegisterTypes 方法用于注册模块的服务类型
/// 2. 定义 OnInitialized 方法用于初始化模块功能
/// 3. 确保模块按生命周期正确执行注册和初始化步骤
/// </remarks>
public interface IModule
{
    /// <summary>
    /// 注册服务类型到依赖注入容器
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <remarks>
    /// 伪代码:
    /// 1. 遍历模块所需的所有服务类型
    /// 2. 将每个服务类型注册到服务集合中
    /// 3. 配置服务的生命周期（单例、作用域、瞬态等）
    /// </remarks>
    void RegisterTypes(IServiceCollection services);

    /// <summary>
    /// 模块初始化方法，在服务注册完成后调用
    /// </summary>
    /// <param name="provider">服务提供者</param>
    /// <remarks>
    /// 伪代码:
    /// 1. 获取所需的服务实例
    /// 2. 执行模块特定的初始化逻辑
    /// 3. 配置模块运行时参数
    /// </remarks>
    void OnInitialized(IServiceProvider provider);
}