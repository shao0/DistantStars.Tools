using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Client.Contracts.Models;
using DistantStars.Core.Models;

namespace DistantStars.Client.Contracts.Services;

/// <summary>
/// 文件服务接口，定义文件操作相关功能
/// </summary>
public interface IFileService
{
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
    Task<Result<IEnumerable<string>>> CompareAndCopyAsync(string folder1Path, string folder2Path, string folder3Path);
    
    /// <summary>
    /// 异步选择文件夹
    /// </summary>
    /// <param name="title">文件夹选择对话框的标题</param>
    /// <returns>包含所选文件夹路径的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 显示文件夹选择对话框
    /// 2. 等待用户选择文件夹
    /// 3. 返回所选文件夹的路径
    /// </remarks>
    Task<Result<string>> SelectFolderAsync(string title);
    
    /// <summary>
    /// 打开指定路径的文件夹
    /// </summary>
    /// <param name="pathFolder">要打开的文件夹路径</param>
    /// <returns>操作结果的Result对象</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 验证文件夹路径是否存在
    /// 2. 如果存在，则使用系统默认文件管理器打开该文件夹
    /// 3. 返回操作结果
    /// </remarks>
    Task<Result> OpenFolderAsync(string pathFolder);
    
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
    Task<Result> WriteCacheFolderAsync(string name, CacheFolder cacheFolder);
    
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
    Task<Result<CacheFolder>> ReadCacheFolderAsync(string name);
}