namespace DistantStars.Tools.Models;

/// <summary>
/// 操作结果的基础类，用于封装操作的状态、消息和数据
/// </summary>
/// <remarks>
/// 伪代码:
/// 1. 定义操作状态（成功/失败）
/// 2. 定义消息（用于描述操作结果详情）
/// 3. 定义数据（用于返回操作结果数据）
/// 4. 提供静态方法创建默认实例
/// </remarks>
public class Result
{
    private static readonly Result Empty = new();
    
    /// <summary>
    /// 操作是否成功
    /// </summary>
    public bool Status { get; set; }
    
    /// <summary>
    /// 操作消息
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// 操作返回的数据
    /// </summary>
    public object Data { get; set; }
    
    /// <summary>
    /// 获取默认的Result实例
    /// </summary>
    /// <returns>默认的Result实例</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 返回一个预创建的空Result实例
    /// 2. 避免频繁创建新实例以提高性能
    /// </remarks>
    public static Result Default() => Empty;
}

/// <summary>
/// 泛型操作结果类，用于封装操作的状态、消息和指定类型的返回数据
/// </summary>
/// <typeparam name="T">返回数据的类型</typeparam>
/// <remarks>
/// 伪代码:
/// 1. 继承基础Result类
/// 2. 提供类型安全的数据属性
/// 3. 提供静态方法创建默认实例
/// </remarks>
public class Result<T> : Result
{
    private static readonly Result<T> Empty = new();
    
    /// <summary>
    /// 操作返回的指定类型数据
    /// </summary>
    public new T Data { get; set; }
    
    /// <summary>
    /// 获取默认的Result&lt;T&gt;实例
    /// </summary>
    /// <returns>默认的Result&lt;T&gt;实例</returns>
    /// <remarks>
    /// 伪代码:
    /// 1. 返回一个预创建的空Result&lt;T&gt;实例
    /// 2. 避免频繁创建新实例以提高性能
    /// </remarks>
    public new static Result<T> Default() => Empty;
}