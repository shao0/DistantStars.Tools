namespace DistantStars.Tools.Core.Modularity;

/// <summary>
/// 2. 模块元数据：描述一个模块的信息
/// </summary>
internal class ModuleInfo : IModuleInfo
{
    public string ModuleName { get; set; }
    public Type ModuleType { get; set; } // 如果是代码注册，直接存 Type
    public string AssemblyPath { get; set; } // 如果是目录扫描，存 DLL 路径
    public bool IsLoaded { get; set; } = false;
}