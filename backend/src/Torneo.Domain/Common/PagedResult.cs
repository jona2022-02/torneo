namespace Torneo.Domain.Common;

public sealed class PagedResult<T> : PageInfo
{
    public PagedResult(IReadOnlyList<T> data, int pageNumber, int pageSize, int totalRecords)
        : base(pageNumber, pageSize, totalRecords)
    {
        Data = data;
    }

    public IReadOnlyList<T> Data { get; }
}
