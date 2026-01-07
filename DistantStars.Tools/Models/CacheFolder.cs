namespace DistantStars.Tools.Models;

/// <summary>
/// 缓存文件夹信息的数据模型
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义一个包含三个文件夹路径的简单数据模型
/// 2. 提供属性来存储比较操作中使用的三个文件夹路径
/// </remarks>
public class CacheFolder
{
    /// <summary>
    /// 文件夹1路径
    /// </summary>
    public string Folder1Path { get; set; }
    
    /// <summary>
    /// 文件夹2路径
    /// </summary>
    public string Folder2Path { get; set; }
    
    /// <summary>
    /// 文件夹3路径（目标文件夹）
    /// </summary>
    public string Folder3Path { get; set; }
}