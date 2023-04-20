namespace EDMS.DSM.Client.Authentication;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly ILocalStorageService _localStorage;

    public AuthenticationManager(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string> GetCurrentUserAsync()
    {
        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        return userToken;
    }

    public Task<string> LogoutAsync()
    {
        throw new NotImplementedException();
    }
}
