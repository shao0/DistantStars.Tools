namespace DistantStars.Tools.Core.Modularity;

// 3. 模块目录：负责存储模块清单
internal class ModuleCatalog : IModuleCatalog
{
    private ModuleCatalog(){}

    public static ModuleCatalog Instance { get; } = new();

    public IEnumerable<IModuleInfo> Modules => _Modules;
    List<ModuleInfo> _Modules { get; } = [];

    // 支持代码注册
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

    // 支持目录扫描注册
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