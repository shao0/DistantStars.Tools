using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Tools.Contracts;
using DistantStars.Tools.Models;
using DistantStars.Tools.Services;

namespace DistantStars.Tools.ViewModels;

public partial class MainWindowViewModel(IFileService fileService) : ViewModelBase, ILoadOnce
{
    private const string CompareAndCopyCacheFileName = "CompareAndCopyCache.json";
    [ObservableProperty] private string _folder1Path = string.Empty;

    [ObservableProperty] private string _folder2Path = string.Empty;

    [ObservableProperty] private string _folder3Path = string.Empty;

    [ObservableProperty] private string _statusMessage = "请设置文件夹路径并点击开始比较";

    [ObservableProperty] private bool _isProcessing;

    [ObservableProperty] private List<string> _copyMessageList;

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

    [RelayCommand]
    private async Task OpenFolder1()
    {
        await fileService.OpenFolderAsync(Folder1Path);
    }

    [RelayCommand]
    private async Task OpenFolder2()
    {
        await fileService.OpenFolderAsync(Folder2Path);
    }

    [RelayCommand]
    private async Task OpenFolder3()
    {
        await fileService.OpenFolderAsync(Folder3Path);
    }

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