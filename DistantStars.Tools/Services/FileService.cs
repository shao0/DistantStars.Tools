using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DistantStars.Tools.Models;

namespace DistantStars.Tools.Services;

public class FileService : IFileService
{
    private static readonly string ChachDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalCache");

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
            result.Message = $"文件夹1不存在: {folder2Path}";
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


    private List<string> GetFileNames(string folderPath, bool includeSubdirectories = false) => Directory.GetFiles(folderPath, "*", includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();

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