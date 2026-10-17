namespace Torneo.Domain.Common;

public class PageInfo
{
    public PageInfo(int pageNumber, int pageSize, int totalRecords)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalRecords { get; }

    public int TotalPages { get; }
}
