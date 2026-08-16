namespace Parser.Services;

public record ParseResult<T>(
    bool Success,
    T? Data,
    string? Error
)
{
    public static ParseResult<T> Ok(T data) => new(true, data, null);
    public static ParseResult<T> Fail(string error) => new(false, default, error);
    public static ParseResult<T> Fail(string error, T? data) => new(false, data, error);
}