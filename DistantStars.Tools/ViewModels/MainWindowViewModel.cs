using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Tools.Core.Contracts;
using DistantStars.Tools.Models;
using DistantStars.Tools.Services;

namespace DistantStars.Tools.ViewModels;

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
public partial class MainWindowViewModel(IFileService fileService) : ViewModelBase, ILoadOnce
{
    private const string CompareAndCopyCacheFileName = "CompareAndCopyCache.json";
    
    /// <summary>
    /// 文件夹1路径
    /// </summary>
    [ObservableProperty] private string _folder1Path = string.Empty;

    /// <summary>
    /// 文件夹2路径
    /// </summary>
    [ObservableProperty] private string _folder2Path = string.Empty;

    /// <summary>
    /// 文件夹3路径（目标文件夹）
    /// </summary>
    [ObservableProperty] private string _folder3Path = string.Empty;

    /// <summary>
    /// 状态消息
    /// </summary>
    [ObservableProperty] private string _statusMessage = "请设置文件夹路径并点击开始比较";

    /// <summary>
    /// 是否正在处理中
    /// </summary>
    [ObservableProperty] private bool _isProcessing;

    /// <summary>
    /// 复制消息列表
    /// </summary>
    [ObservableProperty] private List<string> _copyMessageList;

    /// <summary>
    /// 比较并复制文件的异步命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 验证所有文件夹路径是否已设置
    /// 2. 设置处理状态为真，清空消息列表
    /// 3. 调用文件服务的比较和复制方法
    /// 4. 如果操作成功，保存当前路径到缓存
    /// 5. 显示操作结果和详细信息
    /// 6. 处理异常情况
    /// 7. 最终将处理状态设为假
    /// </remarks>
    [RelayCommand]
    private async Task CompareAndCopyAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Folder1Path) || string.IsNullOrWhiteSpace(Folder2Path) || string.IsNullOrWhiteSpace(Folder3Path))
            {
                StatusMessage = "请先设置所有文件夹路径";
                return;
            }

            IsProcessing = true;
            CopyMessageList = [];
            StatusMessage = "正在处理中...";

            var result = await fileService.CompareAndCopyAsync(Folder1Path, Folder2Path, Folder3Path);
            if (result.Status)
            {
                var resultCacheFolder = await fileService.WriteCacheFolderAsync(CompareAndCopyCacheFileName, new CacheFolder { Folder1Path = Folder1Path, Folder2Path = Folder2Path, Folder3Path = Folder3Path });
                if (resultCacheFolder.Status)
                {
                    await LoadOnce();
                }
                else
                {
                    StatusMessage = $"缓存地址错误:{resultCacheFolder.Message}";
                }

                StatusMessage = result.Message;
            }
            else
            {
                StatusMessage = $"操作错误:{result.Message}";
            }

            CopyMessageList = result.Data.ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = $"错误: {ex.Message}";
        }
        finally
        {
            IsProcessing = false;
        }
    }

    /// <summary>
    /// 选择文件夹1的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的文件夹选择功能
    /// 2. 如果选择成功，更新文件夹1路径
    /// 3. 如果失败，显示错误消息
    /// </remarks>
    [RelayCommand]
    private async Task SelectFolder1()
    {
        var result = await fileService.SelectFolderAsync("选择文件夹1");
        if (result.Status)
        {
            Folder1Path = result.Data;
        }
        else
        {
            StatusMessage = result.Message;
        }
    }

    /// <summary>
    /// 选择文件夹2的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的文件夹选择功能
    /// 2. 如果选择成功，更新文件夹2路径
    /// 3. 如果失败，显示错误消息
    /// </remarks>
    [RelayCommand]
    private async Task SelectFolder2()
    {
        var result = await fileService.SelectFolderAsync("选择文件夹2");
        if (result.Status)
        {
            Folder2Path = result.Data;
        }
        else
        {
            StatusMessage = result.Message;
        }
    }

    /// <summary>
    /// 选择文件夹3的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的文件夹选择功能
    /// 2. 如果选择成功，更新文件夹3路径
    /// 3. 如果失败，显示错误消息
    /// </remarks>
    [RelayCommand]
    private async Task SelectFolder3()
    {
        var result = await fileService.SelectFolderAsync("选择文件夹3");
        if (result.Status)
        {
            Folder3Path = result.Data;
        }
        else
        {
            StatusMessage = result.Message;
        }
    }

    /// <summary>
    /// 打开文件夹1的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的打开文件夹功能
    /// 2. 传入文件夹1的路径
    /// </remarks>
    [RelayCommand]
    private async Task OpenFolder1()
    {
        await fileService.OpenFolderAsync(Folder1Path);
    }

    /// <summary>
    /// 打开文件夹2的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的打开文件夹功能
    /// 2. 传入文件夹2的路径
    /// </remarks>
    [RelayCommand]
    private async Task OpenFolder2()
    {
        await fileService.OpenFolderAsync(Folder2Path);
    }

    /// <summary>
    /// 打开文件夹3的命令
    /// </summary>
    /// <remarks>
    /// 伪代码:
    /// 1. 调用文件服务的打开文件夹功能
    /// 2. 传入文件夹3的路径
    /// </remarks>
    [RelayCommand]
    private async Task OpenFolder3()
    {
        await fileService.OpenFolderAsync(Folder3Path);
    }

    /// <summary>
    /// 加载缓存的路径设置
    /// </summary>
    /// <returns>异步任务</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 设置处理状态为真
    /// 2. 从文件服务读取缓存的文件夹路径
    /// 3. 如果读取成功，更新所有路径属性
    /// 4. 如果读取失败，显示错误消息
    /// 5. 最终将处理状态设为假
    /// </remarks>
    public async Task LoadOnce()
    {
        try
        {
            IsProcessing = true;
            var resultCacheFolder = await fileService.ReadCacheFolderAsync(CompareAndCopyCacheFileName);
            if (resultCacheFolder.Status)
            {
                Folder1Path = resultCacheFolder.Data.Folder1Path;
                Folder2Path = resultCacheFolder.Data.Folder2Path;
                Folder3Path = resultCacheFolder.Data.Folder3Path;
                StatusMessage = $"加载缓存地址完成";
            }
            else
            {
                StatusMessage = $"加载缓存地址错误:{resultCacheFolder.Message}";
            }
        }
        catch (Exception e)
        {
            StatusMessage = $"加载缓存地址错误:{e.Message}";
        }
        finally
        {
            IsProcessing = false;
        }
    }
}