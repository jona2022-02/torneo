namespace Torneo.Domain.Common;

public sealed class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public IReadOnlyList<T> Data { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalRecords { get; }

    public int TotalPages { get; }
}
