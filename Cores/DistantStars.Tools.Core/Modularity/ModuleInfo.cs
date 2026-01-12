namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 模块信息的实现类，用于描述一个模块的基本属性
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 实现 IModuleInfo 接口的所有属性
/// 2. 提供模块名称、类型、路径和加载状态的存储
/// 3. 初始化 IsLoaded 属性为 false
/// </remarks>
internal class ModuleInfo : IModuleInfo
{
    /// <summary>
    /// 获取或设置模块名称
    /// </summary>
    /// <value>模块的唯一标识名称</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 存储模块的名称字符串
    /// 2. 通常使用模块类型名作为默认名称
    /// </remarks>
    public string ModuleName { get; set; }
    
    /// <summary>
    /// 获取或设置模块类型
    /// </summary>
    /// <value>模块的类型信息</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 当模块通过代码注册时，直接存储类型引用
    /// 2. 当模块通过DLL加载时，从程序集中获取类型后存储
    /// </remarks>
    public Type ModuleType { get; set; }
    
    /// <summary>
    /// 获取或设置模块程序集路径
    /// </summary>
    /// <value>模块DLL文件的路径</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 存储模块所在DLL的完整路径
    /// 2. 用于动态加载外部模块
    /// </remarks>
    public string AssemblyPath { get; set; }
    
    /// <summary>
    /// 获取或设置模块是否已加载
    /// </summary>
    /// <value>布尔值表示模块加载状态</value>
    /// <remarks>
    /// 伪代码:
    /// 1. 初始化为 false 表示未加载
    /// 2. 加载成功后设置为 true
    /// 3. 防止重复加载同一模块
    /// </remarks>
    public bool IsLoaded { get; set; } = false;
}