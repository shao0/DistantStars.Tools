namespace DistantStars.Tools.Models;

public class Result
{
    private static readonly Result Empty= new ();
    public bool Status { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public static Result Default() => Empty;
}
public class Result<T> : Result
{
    
    private static readonly Result<T> Empty= new ();
    
    public new T Data { get; set; }
    
    public new static Result<T> Default() => Empty;
}