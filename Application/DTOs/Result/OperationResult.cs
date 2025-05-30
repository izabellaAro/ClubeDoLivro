namespace Application.DTOs.Result;

public class OperationResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public static OperationResult Success() => new() { Succeeded = true };
    public static OperationResult Failure(string error) => new() { Succeeded = false, Error = error };
}