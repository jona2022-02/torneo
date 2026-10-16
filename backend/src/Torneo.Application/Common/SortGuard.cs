using Torneo.Domain.Common;

namespace Torneo.Application.Common;

public static class SortGuard
{
    public static Result Validate(string? sortBy, string sortDirection, IReadOnlySet<string> allowedColumns)
    {
        if (sortDirection is not ("asc" or "desc"))
        {
            return Result.Failure(ErrorCodes.Validation, "sortDirection debe ser asc o desc.");
        }

        if (sortBy is not null && !allowedColumns.Contains(sortBy))
        {
            return Result.Failure(ErrorCodes.Validation, "sortBy no es válido.");
        }

        return Result.Success();
    }
}
