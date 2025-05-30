namespace Application.DTOs.Result;

public class OperationResult<T>
{
    public bool Succeeded { get; private set; }
    public string? Error { get; private set; }
    public T? Data { get; private set; }

    private OperationResult(bool succeeded, T? data = default, string? error = null)
    {
        Succeeded = succeeded;
        Data = data;
        Error = error;
    }

    public static OperationResult<T> Success(T data) => new(true, data);
    public static OperationResult<T> Failure(string error) => new(false, default, error);
}