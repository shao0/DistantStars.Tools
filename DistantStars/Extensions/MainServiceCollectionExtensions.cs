using DistantStars.Client.Core.Extensions;
using DistantStars.Client.Services.Extensions;
using DistantStars.ViewModels;
using DistantStars.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Extensions;

public static class MainServiceCollectionExtensions
{
    public static IServiceCollection AddCommandServices(this IServiceCollection services)
    {
        services.RegisterLoadOnce<MainWindow,MainWindowViewModel>();
        services.AddClientServices();
        return services;
    }
}