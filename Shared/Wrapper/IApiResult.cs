namespace EDMS.Shared.Wrapper;

public interface IApiResult
{
    string Message { get; set; }
    bool Status { get; set; }
}

public interface IApiResult<out T> : IApiResult
{
    T Result { get; }
}

public interface IListApiResult<T> : IApiResult<T>
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int TotalPages { get; set; }
    int TotalRecords { get; set; }
}
