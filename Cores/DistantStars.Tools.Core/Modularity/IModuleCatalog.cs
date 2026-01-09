namespace DistantStars.Tools.Core.Modularity;

public interface IModuleCatalog
{
    public IEnumerable<IModuleInfo> Modules { get; }
    IModuleCatalog AddModule<T>() where T : IModule;
    IModuleCatalog AddModuleFromPath(string path);
    IModuleCatalog AddScanRegisterFromPath(string directoryPath);
}