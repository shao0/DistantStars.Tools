using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DistantStars.Tools.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DistantStars.Tools.Core.Extensions;

/// <summary>
/// ILoadOnce 接口的扩展方法，用于在控件加载时自动执行一次性加载逻辑
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义扩展方法 RegisterLoadOnce，用于注册支持一次性加载的视图和视图模型
/// 2. 创建工厂方法 LoadOnceFactory，负责创建视图实例并设置数据上下文
/// 3. 实现 ControlOnLoaded 事件处理器，在控件加载时调用 ILoadOnce 接口的 LoadOnce 方法
/// </remarks>
public static class LoadOnceExtensions
{
    /// <summary>
    /// 注册支持一次性加载的视图和视图模型
    /// </summary>
    /// <typeparam name="TV">视图类型，必须继承自 Control</typeparam>
    /// <typeparam name="TVM">视图模型类型，必须实现 ILoadOnce 接口</typeparam>
    /// <param name="service">服务集合</param>
    /// <remarks>
    /// 伪代码:
    /// 1. 将视图模型注册为作用域服务
    /// 2. 将视图注册为使用 LoadOnceFactory 工厂方法创建的实例
    /// </remarks>
    public static void RegisterLoadOnce<TV,TVM>(this IServiceCollection service) where TV : Control where TVM : class, ILoadOnce
    {
        service.AddScoped<TVM>();
        service.AddScoped(LoadOnceFactory<TV, TVM>);
    }

    /// <summary>
    /// 创建支持一次性加载的视图实例的工厂方法
    /// </summary>
    /// <typeparam name="TV">视图类型，必须继承自 Control</typeparam>
    /// <typeparam name="TVM">视图模型类型，必须实现 ILoadOnce 接口</typeparam>
    /// <param name="provider">服务提供者</param>
    /// <returns>配置好的视图实例</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 使用服务提供者创建视图实例
    /// 2. 设置视图的数据上下文为对应的视图模型
    /// 3. 为视图的 Loaded 事件添加事件处理器
    /// 4. 返回配置好的视图实例
    /// </remarks>
    private static TV LoadOnceFactory<TV, TVM>(IServiceProvider provider) where TV : Control where TVM : class, ILoadOnce
    {
        var v = ActivatorUtilities.CreateInstance<TV>(provider);
        v.DataContext = provider.GetService<TVM>();
        v.Loaded += ControlOnLoaded;
        return v;
    }

    /// <summary>
    /// 控件加载事件处理器，用于在控件加载时执行一次性加载逻辑
    /// </summary>
    /// <param name="sender">事件发送者（控件实例）</param>
    /// <param name="e">路由事件参数</param>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查事件发送者是否为控件实例
    /// 2. 移除事件处理器以避免重复执行
    /// 3. 检查控件的数据上下文是否实现了 ILoadOnce 接口
    /// 4. 如果实现了接口，则调用 LoadOnce 方法
    /// </remarks>
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