using System;
using System.IO;
using DistantStars.Client.Core.Modularity;

namespace DistantStars.Extensions;

public static class ModuleExtensions
{
    public static IModuleCatalog AddModules(this IModuleCatalog moduleCatalog)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");
        moduleCatalog.AddScanRegisterFromPath(path);
        return moduleCatalog;
    }
}