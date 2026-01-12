using System.Threading.Tasks;

namespace DistantStars.Client.Core.Contracts;

/// <summary>
/// 定义一次性加载操作的接口
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义 LoadOnce 异步方法，用于执行一次性初始化逻辑
/// 2. 确保实现类在控件加载时仅执行一次初始化操作
/// </remarks>
public interface ILoadOnce
{
    /// <summary>
    /// 执行一次性加载逻辑
    /// </summary>
    /// <returns>异步操作任务</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 检查当前状态，确保仅执行一次
    /// 2. 执行必要的初始化操作
    /// 3. 更新内部标志以防止重复执行
    /// </remarks>
    Task LoadOnce();
}