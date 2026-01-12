using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DistantStars.Client.Contracts.Models;
using DistantStars.Client.Contracts.Services;
using DistantStars.Core.Models;

namespace DistantStars.Client.Services.Implementations;

/// <summary>
/// 文件服务实现类，提供文件操作相关功能
/// </summary>
public class FileService : IFileService
{
    /// <summary>
    /// 缓存目录路径，用于存储本地缓存文件
    /// </summary>
    private static readonly string ChachDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalCache");

    /// <summary>
    /// 比较并复制文件
    /// </summary>
    /// <param name="folder1Path">源文件夹路径1（从中获取要比较的文件列表）</param>
    /// <param name="folder2Path">源文件夹路径2（从中获取要复制的文件）</param>
    /// <param name="folder3Path">目标文件夹路径（复制文件到此位置）</param>
    /// <returns>包含操作结果和详细信息的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 验证源文件夹路径是否存在
    /// 2. 如果目标文件夹不存在，则创建它
    /// 3. 获取文件夹1中的所有文件（包括子目录）
    /// 4. 获取文件夹2中的所有文件（包括子目录）
    /// 5. 将文件夹2中的文件名和路径存储到字典中，用于快速查找
    /// 6. 遍历文件夹1中的每个文件
    /// 7. 如果文件夹2中存在同名文件，则复制到目标文件夹
    /// 8. 记录操作结果和详细信息
    /// </remarks>
    public async Task<Result<IEnumerable<string>>> CompareAndCopyAsync(string folder1Path, string folder2Path, string folder3Path)
    {
        var result = Result<IEnumerable<string>>.Default();
        if (!Directory.Exists(folder1Path))
        {
            result.Message = $"文件夹1不存在: {folder1Path}";
            return result;
        }

        if (!Directory.Exists(folder2Path))
        {
            result.Message = $"文件夹2不存在: {folder2Path}";  // 修复：原代码写的是"文件夹1不存在"
            return result;
        }

        if (!Directory.Exists(folder3Path))
            Directory.CreateDirectory(folder3Path);

        // 获取文件夹1中的所有文件（包括子目录）
        var filesInFolder1 = GetFileNames(folder1Path, true); // 包含子目录
        var filesInFolder2 = GetFileNames(folder2Path, true); // 包含子目录

        // 提取文件名，用于比较
        var files2Dictionary = new Dictionary<string,string>();
        var max = 0;
        foreach (var path in filesInFolder2)
        {
            var fileName = Path.GetFileName(path);
            files2Dictionary[fileName]= path;
            if (fileName.Length > max)
            {
                max = fileName.Length;
            }
        }

        var sbList = new List<StringBuilder>();
        var message = new StringBuilder();
        message.Append("总共文件:");
        message.Append(filesInFolder1.Count);
        message.Append("个,");
        var successCount = 0;
        foreach (var filePath in filesInFolder1)
        {
            var fileName = Path.GetFileName(filePath);
            var sb = new StringBuilder();
            sbList.Add(sb);
            sb.Append("复制文件");
            sb.Append(fileName);
            sb.Append("  =>  ");
            if (files2Dictionary.TryGetValue(fileName,out var sourceFilePath ))
            {
                try
                {
                    var destinationPath = Path.Combine(folder3Path, fileName);
                    await CopyFileAsync(sourceFilePath, destinationPath);
                    sb.Append("成功");
                    successCount++;
                }
                catch (Exception e)
                {
                    sb.Append($"失败{e.Message}");
                }

                result.Status = true;
            }
        }

        message.Append("成功复制文件:");
        message.Append(successCount);
        message.Append(",失败复制文件:");
        message.Append(filesInFolder1.Count - successCount);
        result.Data = sbList.Select(i => i.ToString());
        result.Message = message.ToString();
        return result;
    }

    /// <summary>
    /// 异步选择文件夹
    /// </summary>
    /// <param name="title">文件夹选择对话框的标题</param>
    /// <returns>包含所选文件夹路径的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查当前应用程序是否具有桌面样式生命周期
    /// 2. 如果是，则显示文件夹选择对话框
    /// 3. 如果用户选择了文件夹，则返回其绝对路径
    /// 4. 否则返回空结果
    /// </remarks>
    public async Task<Result<string>> SelectFolderAsync(string title)
    {
        var result = Result<string>.Default();

        if (Application.Current is { ApplicationLifetime: IClassicDesktopStyleApplicationLifetime { MainWindow: { } mainWindow } })
        {
            var folder = await mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
            });
            if (folder is { Count: > 0 })
            {
                result.Data = folder[0].Path.AbsolutePath;
            }
        }

        return result;
    }

    /// <summary>
    /// 打开指定路径的文件夹
    /// </summary>
    /// <param name="pathFolder">要打开的文件夹路径</param>
    /// <returns>操作结果的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查应用程序是否具有桌面样式生命周期
    /// 2. 验证文件夹路径是否存在
    /// 3. 如果存在，则使用explorer.exe打开该文件夹
    /// 4. 返回操作结果
    /// </remarks>
    public async Task<Result> OpenFolderAsync(string pathFolder)
    {
        var result = Result.Default();
        if (Application.Current is { ApplicationLifetime: IClassicDesktopStyleApplicationLifetime { } })
        {
            if (Directory.Exists(pathFolder))
            {
                await Task.Run(() => Process.Start("explorer.exe", pathFolder));
                result.Status = true;
            }
            else
            {
                result.Message = $"文件夹不存在: {pathFolder}";
            }
        }

        return result;
    }

    /// <summary>
    /// 异步写入缓存文件夹数据
    /// </summary>
    /// <param name="name">缓存文件的名称</param>
    /// <param name="cacheFolder">要序列化的缓存文件夹对象</param>
    /// <returns>操作结果的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查缓存文件夹对象是否为null
    /// 2. 如果不为null，则将其序列化为JSON格式
    /// 3. 确保缓存目录存在，如果不存在则创建
    /// 4. 将JSON数据写入指定名称的文件
    /// 5. 返回操作结果
    /// </remarks>
    public async Task<Result> WriteCacheFolderAsync(string name, CacheFolder cacheFolder)
    {
        var result = Result.Default();
        try
        {
            if (cacheFolder is not null)
            {
                await using var ms = new MemoryStream();
                await JsonSerializer.SerializeAsync(ms, cacheFolder);
                ms.Position = 0;
                using var sr = new StreamReader(ms);
                var json = await sr.ReadToEndAsync();
                if (!Directory.Exists(ChachDirectory))
                {
                    Directory.CreateDirectory(ChachDirectory);
                }

                var filePath = Path.Combine(ChachDirectory, name);
                await File.WriteAllTextAsync(filePath, json);
                result.Status = true;
            }
            else
            {
                result.Data = new();
                result.Status = true;
            }
        }
        catch (Exception e)
        {
            result.Message = e.Message;
        }

        return result;
    }

    /// <summary>
    /// 异步读取缓存文件夹数据
    /// </summary>
    /// <param name="name">要读取的缓存文件名称</param>
    /// <returns>包含缓存文件夹对象的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 构建缓存文件的完整路径
    /// 2. 检查文件是否存在
    /// 3. 如果存在，则反序列化JSON数据为CacheFolder对象
    /// 4. 如果不存在，则返回错误信息
    /// 5. 返回操作结果
    /// </remarks>
    public async Task<Result<CacheFolder>> ReadCacheFolderAsync(string name)
    {
        var result = Result<CacheFolder>.Default();
        try
        {
            var filePath = Path.Combine(ChachDirectory, name);
            if (File.Exists(filePath))
            {
                await using var sr = File.OpenRead(filePath);
                var cacheFolder = await JsonSerializer.DeserializeAsync<CacheFolder>(sr);
                result.Data = cacheFolder;
                result.Status = true;
            }
            else
            {
                result.Message = $"文件不存在: {filePath}";
            }
        }
        catch (Exception e)
        {
            result.Message = e.Message;
        }

        return result;
    }


    /// <summary>
    /// 获取指定文件夹中的所有文件路径
    /// </summary>
    /// <param name="folderPath">要搜索的文件夹路径</param>
    /// <param name="includeSubdirectories">是否包含子目录中的文件</param>
    /// <returns>文件路径列表</returns>
    private List<string> GetFileNames(string folderPath, bool includeSubdirectories = false) => Directory.GetFiles(folderPath, "*", includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();

    /// <summary>
    /// 异步复制文件到目标位置
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="destinationPath">目标文件路径</param>
    /// <returns>异步任务</returns>
    private async Task CopyFileAsync(string sourcePath, string destinationPath)
    {
        // 确保目标目录存在
        var destinationDir = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDir))
            Directory.CreateDirectory(destinationDir);
        await Task.Yield();
        File.Copy(sourcePath, destinationPath, true);
    }
}