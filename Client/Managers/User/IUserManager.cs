namespace EDMS.DSM.Client.Managers.User;

public interface IUserManager : IManager
{
    Task<IListApiResult<List<Organization>>> GetOrganizationsAsync();
    Task<IApiResult<bool>> IsUserTokenValidAsync(string userToken);
    Task<TokenResult> RefreshUserTokenAsync(string aspnetuserId);
    Task<TokenResult> RegenerateUserTokenAsync<TIn>(TIn values);
}
