namespace Geo.Api.Client.Domain.Result;

public sealed class Result<TValue, TError>
{
    public Result(TValue? value, TError? error)
    {
        Value = value;
        Error = error;
    }

    public TValue? Value { get; }
    
    public TError? Error { get; }

    public static implicit operator Result<TValue, TError>(TValue value)
    {
        return new Result<TValue, TError>(value, default);
    }
    
    public static implicit operator Result<TValue, TError>(TError value)
    {
        return new Result<TValue, TError>(default, value);
    }
}