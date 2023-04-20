namespace EDMS.Shared.Wrapper;

public class PagedApiResult<T> : ApiResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public static PagedApiResult<T> Success(T data, int pageNumber, int pageSize, int totalPages, int totalRecords)
    {
        return new PagedApiResult<T>
        {
            Status = true,
            Result = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords
        };
    }
}
