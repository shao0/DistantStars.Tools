using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DistantStars.Client.Contracts.Services;
using DistantStars.Client.Core.Contracts;
using DistantStars.Core.Models;

namespace DistantStars.ViewModels;

/// <summary>
/// 主窗口视图模型，处理文件比较和复制的业务逻辑
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 初始化视图模型，注入文件服务
/// 2. 定义界面绑定的属性（文件夹路径、状态消息等）
/// 3. 实现文件夹选择、比较复制、打开文件夹等功能
/// 4. 实现缓存加载功能以保存上次的设置
/// </remarks>
public partial class MainWindowViewModel: ObservableObject, ILoadOnce
{
    public Task LoadOnce()
    {
        return Task.CompletedTask;
    }
}