namespace Parser.Services;

public record Result
{
    public bool Success { get; }
    public Error? Error { get; }
    
    protected Result(bool Success, Error? error)
    {
        Success = Success;
        Error = error;
    }

    public static Result Ok() => new(true, null);
    public static Result Fail(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));
    
    public static implicit operator Result(Error error) => Fail(error);
}

public record Result<T> : Result
{
    public T? Value { get; }
    
    private Result(T value) : base(true, null) => Value = value;
    private Result(Error error) : base(false, error) { }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}