namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 定义模块管理器的接口，用于管理和执行应用程序中的模块
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义 Run 方法来启动模块管理系统
/// 2. 加载所有注册的模块
/// 3. 执行模块的注册和初始化过程
/// </remarks>
public interface IModuleManager
{
    /// <summary>
    /// 运行模块管理器，启动模块加载和初始化过程
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 遍历模块目录中的所有模块
    /// 2. 加载模块程序集
    /// 3. 注册模块服务
    /// 4. 初始化模块功能
    /// </remarks>
    void Run();
}