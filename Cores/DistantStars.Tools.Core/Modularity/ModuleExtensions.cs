using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DistantStars.Tools.Core.Modularity;

public static class ModuleExtensions
{
    public static IModuleCatalog BuildModuleCatalog(this IServiceCollection services)
    {
        services.TryAddSingleton(services);
        services.TryAddScoped<IModuleInfo, ModuleInfo>();
        services.TryAddSingleton<IModuleCatalog, ModuleCatalog>();
        services.TryAddSingleton<IModuleManager, ModuleManager>();
        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IModuleCatalog>();
    }

    public static IServiceCollection RunModularity(this IModuleCatalog catalog, IServiceCollection services)
    {
        services.RemoveAll<IModuleCatalog>();
        services.TryAddSingleton(catalog);
        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IModuleManager>().Run();
        return services;
    }
}