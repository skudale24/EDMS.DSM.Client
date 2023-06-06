namespace EDMS.DSM.Client.Managers.User;

public class UserManager : IUserManager
{
    private readonly HttpRequest _httpRequest;

    public UserManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public Task<IListApiResult<List<Organization>>> GetOrganizationsAsync()
    {
        var urlWithParams = $"{UserEndPoints.Organizations}";
        return _httpRequest.GetRequestAsync<List<Organization>>(urlWithParams);
    }

    public async Task<TokenResult> RefreshUserTokenAsync(string aspnetuserId)
    {
        var urlWithParams = $"{UserEndPoints.RefreshUserToken}?aspnetUserId={aspnetuserId}";
        var data = await _httpRequest.AuthGetRequestAsync<string>(urlWithParams).ConfigureAwait(false);
        return data;
    }

    public async Task<TokenResult> RegenerateUserTokenAsync<TIn>(TIn values)
    {
        var urlWithParams = $"{UserEndPoints.RegenerateUserToken}";
        var data = await _httpRequest.AuthGetRequestDataAsync<TIn>(urlWithParams, values).ConfigureAwait(false);
        return data;
    }

    Task<IApiResult<bool>> IUserManager.IsUserTokenValidAsync(string userToken)
    {
        throw new NotImplementedException();
    }

    public IApiResult<bool> IsUserTokenValid(string userToken)
    {
        ApiResult<bool> result = new() { Message = "true", Result = true, Status = true };
        return result;

        //string urlWithParams = $"{UserEndPoints.IsUserTokenValid}";
        //return await _httpRequest.GetRequest<bool>(urlWithParams);
    }
}
