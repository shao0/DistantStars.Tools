using System.Collections.Generic;
using System.IO;

namespace DistantStars.Client.Core.Modularity;

/// <summary>
/// 模块目录的实现类，负责存储和管理应用程序中的所有模块清单
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 创建单例 ModuleCatalog 实例
/// 2. 维护内部模块信息列表
/// 3. 实现 AddModule 方法支持代码注册
/// 4. 实现 AddModuleFromPath 方法支持路径注册
/// 5. 实现 AddScanRegisterFromPath 方法支持目录扫描注册
/// </remarks>
internal class ModuleCatalog : IModuleCatalog
{
    /// <summary>
    /// 私有构造函数，防止外部实例化
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 确保此类只能通过 Instance 属性访问
    /// 2. 防止外部代码创建多个实例
    /// </remarks>
    private ModuleCatalog(){}

    /// <summary>
    /// 获取模块目录的单例实例
    /// </summary>
    /// <value>ModuleCatalog 单例实例</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 创建并返回 ModuleCatalog 的唯一实例
    /// 2. 确保整个应用程序中只有一个模块目录
    /// </remarks>
    public static ModuleCatalog Instance { get; } = new();

    /// <summary>
    /// 获取所有已注册的模块信息集合
    /// </summary>
    /// <value>模块信息集合</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 返回内部存储的模块信息列表
    /// 2. 通过只读属性提供访问接口
    /// </remarks>
    public IEnumerable<IModuleInfo> Modules => _Modules;
    
    /// <summary>
    /// 内部存储模块信息的列表
    /// </summary>
    /// <value>ModuleInfo 列表</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 初始化一个空的模块信息列表
    /// 2. 用于存储所有注册的模块信息
    /// </remarks>
    List<ModuleInfo> _Modules { get; } = [];

    /// <summary>
    /// 添加指定类型的模块到目录中
    /// </summary>
    /// <typeparam name="T">模块类型，必须实现 IModule 接口</typeparam>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 创建新的 ModuleInfo 对象
    /// 2. 设置模块名称为类型名称
    /// 3. 设置模块类型为传入的泛型类型
    /// 4. 设置加载状态为未加载
    /// 5. 将模块信息添加到内部列表
    /// 6. 返回当前实例以支持链式调用
    /// </remarks>
    public IModuleCatalog AddModule<T>() where T : IModule
    {
        _Modules.Add(new ModuleInfo
        {
            ModuleName = typeof(T).Name,
            ModuleType = typeof(T),
            IsLoaded = false
        });
        return this;
    }

    /// <summary>
    /// 从指定路径添加模块到目录中
    /// </summary>
    /// <param name="path">模块DLL路径</param>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 创建新的 ModuleInfo 对象
    /// 2. 设置程序集路径为传入路径
    /// 3. 设置加载状态为未加载
    /// 4. 将模块信息添加到内部列表
    /// 5. 返回当前实例以支持链式调用
    /// </remarks>
    public IModuleCatalog AddModuleFromPath(string path)
    {
        // 此时只记录路径，暂不加载，延迟到 Manager 处理
        _Modules.Add(new ModuleInfo
        {
            AssemblyPath = path,
            IsLoaded = false
        });
        return this;
    }
    
    /// <summary>
    /// 从指定目录路径扫描并注册模块
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查目录是否存在
    /// 2. 获取目录中所有DLL文件
    /// 3. 为每个DLL文件调用 AddModuleFromPath 方法
    /// 4. 返回当前实例以支持链式调用
    /// </remarks>
    public IModuleCatalog AddScanRegisterFromPath(string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) return this;

        var dlls = Directory.GetFiles(directoryPath, "*.dll");
        foreach (var dll in dlls)
        {
            // 这里只是简单的添加路径，具体的反射检查交给 Manager 的 LoadAssembly
            // 这样可以避免在扫描阶段就加载无效的 DLL 导致程序崩溃
           AddModuleFromPath(dll);
        }

        return this;
    }
}