using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DistantStars.Tools.Core.Modularity;

public static class ModuleExtensions
{
    public static IModuleCatalog BuildModuleCatalog(this IServiceCollection services)
    {
        services.TryAddSingleton(services);
        var moduleCatalog = ModuleCatalog.Instance;
        services.TryAddSingleton<IModuleCatalog>(moduleCatalog);
        services.TryAddSingleton<IModuleManager, ModuleManager>();
        return moduleCatalog;
    }

    public static IServiceCollection RunModularity(this IModuleCatalog catalog, IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IModuleManager>().Run();
        return services;
    }
}