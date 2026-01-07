using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DistantStars.Tools.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Tools.Extensions;

public static class LoadOnceExtensions
{
    public static void RegisterLoadOnce<TV,TVM>(this IServiceCollection service) where TV : Control where TVM : class, ILoadOnce
    {
        service.AddScoped<TVM>();
        service.AddScoped(LoadOnceFactory<TV, TVM>);
    }

    private static TV LoadOnceFactory<TV, TVM>(IServiceProvider provider) where TV : Control where TVM : class, ILoadOnce
    {
        var v = ActivatorUtilities.CreateInstance<TV>(provider);
        v.DataContext = provider.GetService<TVM>();
        v.Loaded += ControlOnLoaded;
        return v;
    }

    private static void ControlOnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not Control control) return;
        control.Loaded -= ControlOnLoaded;
        if (control.DataContext is ILoadOnce loadOnce)
        {
            loadOnce.LoadOnce();
        }
    }
}