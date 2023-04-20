namespace EDMS.DSM.Client.Managers.Menu;

public class NavManager : INavManager
{
    private readonly HttpRequest _httpRequest;

    public NavManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<NavMenuDto>>> GetMenus()
    {
        var urlWithParams = $"{NavMenuEndPoints.Menus}";

        return await _httpRequest.GetRequestAsync<List<NavMenuDto>>(urlWithParams).ConfigureAwait(false);
    }

    public async Task<IApiResult<UserInfoDto>> GetUserData()
    {
        var urlWithParams = $"{NavMenuEndPoints.UserData}";

        return await _httpRequest.GetRequestAsync<UserInfoDto>(urlWithParams).ConfigureAwait(false);
    }
}
