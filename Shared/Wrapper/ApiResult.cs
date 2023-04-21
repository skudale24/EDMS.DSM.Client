namespace EDMS.Shared.Wrapper;

public class ApiResult : IApiResult
{
    public bool Status { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Task<IApiResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static IApiResult Success()
    {
        return new ApiResult { Status = true };
    }

    public static Task<IApiResult> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static IApiResult Fail(string message)
    {
        return new ApiResult { Status = false, Message = message };
    }

    public static IApiResult SuccessMessage(string message)
    {
        return new ApiResult { Status = true, Message = message };
    }
}

public class ApiResult<T> : ApiResult, IApiResult<T>
{
    public T Result { get; set; }

    public static Task<ApiResult<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<ApiResult<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }

    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T> { Status = true, Result = data };
    }

    public static ApiResult<T> Success(T data, string message)
    {
        return new ApiResult<T> { Status = true, Result = data, Message = message };
    }

    public static new ApiResult<T> Fail(string message)
    {
        return new ApiResult<T> { Status = false, Message = message };
    }

    public static ApiResult<T> Fail(T data, string message)
    {
        return new ApiResult<T> { Status = false, Message = message, Result = data };
    }
}

public class ListApiResult<T> : ApiResult<T>, IListApiResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}
