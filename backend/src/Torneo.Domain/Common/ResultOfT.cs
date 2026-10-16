namespace Torneo.Domain.Common;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value)
        : base(true, null)
    {
        _value = value;
    }

    private Result(Error error)
        : base(false, error)
    {
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("El resultado fallido no tiene valor.");

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(string code, string message) => new(new Error(code, message));
}
