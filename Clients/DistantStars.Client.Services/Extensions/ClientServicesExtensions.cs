using DistantStars.Client.Contracts.Services;
using DistantStars.Client.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Client.Services.Extensions;

public static class ClientServicesExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}