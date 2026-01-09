namespace DistantStars.Tools.Core.Modularity;

public interface IModuleInfo
{
    string ModuleName { get; set; }
    Type ModuleType { get; set; } // 如果是代码注册，直接存 Type
    string AssemblyPath { get; set; } // 如果是目录扫描，存 DLL 路径
    bool IsLoaded { get; set; } 
}