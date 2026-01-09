using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Tools.Core.Modularity;

// 4. 模块管理器：负责干活（加载、实例化、执行）
internal class ModuleManager(IModuleCatalog catalog, IServiceCollection services) : IModuleManager
{
    // DI 容器构建器

    // 核心流程：加载 -> 注册 -> 初始化
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

    // 辅助逻辑：处理 DLL 加载
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