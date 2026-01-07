using DistantStars.Tools.Services;
using DistantStars.Tools.ViewModels;
using DistantStars.Tools.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Tools.Extensions;

public static class MainServiceCollectionExtensions
{
    public static IServiceCollection AddCommandServices(this IServiceCollection services)
    {
        services.RegisterLoadOnce<MainWindow,MainWindowViewModel>();
        services.AddSingleton<IFileService,FileService>();
        return services;
    }
}