using DistantStars.Client.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Tools;

public class ModuleTools:IModule
{
    public void RegisterTypes(IServiceCollection services)
    {
    }

    public void OnInitialized(IServiceProvider provider)
    {
    }
}