namespace Parser.Services;

public record ParseResult(
    bool Success,
    List<object>? Data,
    string? Error
)
{
    public static ParseResult Ok(List<object> data) => new(true, data, null);
    public static ParseResult Fail(string error) => new(false, null, error);
    public static ParseResult Fail(string error, List<object>? data) => new(false, data, error);
}