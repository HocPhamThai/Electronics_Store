namespace Electronics_Store.Shared.Responder;

public class ServiceResponder<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}