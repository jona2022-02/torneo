using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public static class SinglePage
{
    private static readonly HashSet<string> NoColumns = new(StringComparer.OrdinalIgnoreCase);

    public static Result<PagedResult<T>> Wrap<T>(Result<T> result, PageQuery page)
    {
        var request = new PagedRequest(page.PageNumber, page.PageSize, page.SortBy, page.SortDirection);
        var sort = SortGuard.Validate(request.SortBy, request.SortDirection, NoColumns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<T>>(sort);
        }

        if (result.IsFailure)
        {
            return ResultMap.Failure<PagedResult<T>>(result);
        }

        IReadOnlyList<T> data = request.Offset == 0 ? [result.Value] : [];
        return Result<PagedResult<T>>.Success(new PagedResult<T>(data, request.PageNumber, request.PageSize, 1));
    }
}
