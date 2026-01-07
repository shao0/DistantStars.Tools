using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Tools.Models;

namespace DistantStars.Tools.Services;

public interface IFileService
{
    Task<Result<IEnumerable<string>>> CompareAndCopyAsync(string folder1Path, string folder2Path, string folder3Path);
    Task<Result<string>> SelectFolderAsync(string title);
    Task<Result> OpenFolderAsync(string pathFolder);
    Task<Result> WriteCacheFolderAsync(string name, CacheFolder cacheFolder);
    Task<Result<CacheFolder>> ReadCacheFolderAsync(string name);
}