namespace EDMS.DSM.Client.Filters;

public class PaginationFilter
{
    private readonly int _defaultPageSize = 20;
    private readonly int _maxPageSize = 50;

    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = _defaultPageSize;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > _maxPageSize ? _defaultPageSize : pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
