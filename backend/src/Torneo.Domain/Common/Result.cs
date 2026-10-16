namespace Torneo.Domain.Common;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
        {
            throw new InvalidOperationException("Un resultado exitoso no lleva error.");
        }

        if (!isSuccess && error is null)
        {
            throw new InvalidOperationException("Un resultado fallido necesita un error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(string code, string message) => new(false, new Error(code, message));
}
