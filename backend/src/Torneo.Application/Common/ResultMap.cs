using Torneo.Domain.Common;

namespace Torneo.Application.Common;

public static class ResultMap
{
    public static Result<T> Failure<T>(Result result)
    {
        var error = result.Error!;
        return Result<T>.Failure(error.Code, error.Message);
    }
}
