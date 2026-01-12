using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Client.Core.Modularity;

/// <summary>
/// 模块管理器的实现类，负责加载、实例化和执行应用程序中的模块
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 接收模块目录和服务集合作为依赖项
/// 2. 实现 Run 方法执行核心流程：加载 -> 注册 -> 初始化
/// 3. 实现 LoadAssembly 方法处理DLL加载
/// 4. 按照模块生命周期管理模块的注册和初始化
/// </remarks>
internal class ModuleManager(IModuleCatalog catalog, IServiceCollection services) : IModuleManager
{
    /// <summary>
    /// 运行模块管理器，启动模块加载和初始化过程
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 输出开始加载模块程序集的日志
    /// 2. 遍历模块目录中的所有模块信息
    /// 3. 对每个模块调用 LoadAssembly 方法加载程序集
    /// 4. 输出执行模块服务注册的日志
    /// 5. 遍历模块目录中的所有模块信息
    /// 6. 对于每个具有模块类型的模块，创建实例并注册服务
    /// 7. 构建服务提供者
    /// 8. 对每个模块实例调用 OnInitialized 方法进行初始化
    /// </remarks>
    public void Run()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine(">>> 1. 开始加载模块程序集 (Loading & Discovery)...");

        foreach (var info in catalog.Modules)
        {
            LoadAssembly(info);
        }

        Console.WriteLine(">>> 2. 执行模块服务注册 (RegisterTypes)...");
        // 用于保存实例化后的模块，供下一步使用
        var moduleInstances = new List<IModule>();

        foreach (var info in catalog.Modules)
        {
            if (info.ModuleType != null)
            {
                // 使用反射创建模块实例
                var module = (IModule)Activator.CreateInstance(info.ModuleType);
                moduleInstances.Add(module);

                // 调用契约方法：注册
                module.RegisterTypes(services);
                Console.WriteLine($"   -> {info.ModuleName} : 服务已注册");
            }
        }

        Console.WriteLine(">>> 3. 构建容器并初始化模块 (OnInitialized)...");
        // 构建 ServiceProvider (模拟 Prism 的各种 Adapter 构建完成)
        var provider = services.BuildServiceProvider();

        foreach (var module in moduleInstances)
        {
            // 调用契约方法：初始化
            module.OnInitialized(provider);
        }

        Console.WriteLine("--------------------------------------------------");
    }

    /// <summary>
    /// 加载指定模块信息的程序集
    /// </summary>
    /// <param name="info">模块信息</param>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查模块类型是否为空且程序集路径不为空
    /// 2. 如果满足条件，使用 Assembly.LoadFrom 从路径加载程序集
    /// 3. 在程序集中查找实现 IModule 接口的非抽象类
    /// 4. 如果找到类型，更新模块信息的类型和名称
    /// 5. 捕获异常并输出错误日志
    /// </remarks>
    private void LoadAssembly(IModuleInfo info)
    {
        // 如果只有路径（来自目录扫描），需要反射加载 Assembly
        if (info.ModuleType == null && !string.IsNullOrEmpty(info.AssemblyPath))
        {
            try
            {
                // 加载 DLL
                var assembly = Assembly.LoadFrom(info.AssemblyPath);

                // 查找该 DLL 中实现了 IModule 的类
                var type = assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                if (type != null)
                {
                    info.ModuleType = type;
                    info.ModuleName = type.Name;
                    Console.WriteLine($"[Scanner] 从 DLL 发现模块: {info.ModuleName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] 无法加载模块 DLL: {info.AssemblyPath}. {ex.Message}");
            }
        }
    }
}