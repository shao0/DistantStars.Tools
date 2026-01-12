namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 定义模块目录的接口，用于管理应用程序中的所有模块
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义 Modules 属性，返回所有已注册的模块信息
/// 2. 定义 AddModule 方法，通过泛型参数添加模块
/// 3. 定义 AddModuleFromPath 方法，从指定路径添加模块
/// 4. 定义 AddScanRegisterFromPath 方法，扫描目录并注册模块
/// </remarks>
public interface IModuleCatalog
{
    /// <summary>
    /// 获取所有已注册的模块信息集合
    /// </summary>
    /// <returns>模块信息集合</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 返回内部存储的模块信息列表
    /// 2. 确保返回的是不可修改的集合视图
    /// </remarks>
    public IEnumerable<IModuleInfo> Modules { get; }
    
    /// <summary>
    /// 添加指定类型的模块到目录中
    /// </summary>
    /// <typeparam name="T">模块类型，必须实现 IModule 接口</typeparam>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 创建指定类型的模块信息对象
    /// 2. 将模块信息添加到内部存储中
    /// 3. 返回当前目录实例以支持链式调用
    /// </remarks>
    IModuleCatalog AddModule<T>() where T : IModule;
    
    /// <summary>
    /// 从指定路径添加模块到目录中
    /// </summary>
    /// <param name="path">模块路径</param>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 根据路径创建模块信息对象
    /// 2. 将模块信息添加到内部存储中
    /// 3. 返回当前目录实例以支持链式调用
    /// </remarks>
    IModuleCatalog AddModuleFromPath(string path);
    
    /// <summary>
    /// 从指定目录路径扫描并注册模块
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    /// <returns>模块目录实例，支持链式调用</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 遍历目录中的所有DLL文件
    /// 2. 为每个DLL文件创建模块信息对象
    /// 3. 将模块信息添加到内部存储中
    /// 4. 返回当前目录实例以支持链式调用
    /// </remarks>
    IModuleCatalog AddScanRegisterFromPath(string directoryPath);
}