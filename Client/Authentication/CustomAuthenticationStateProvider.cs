namespace EDMS.DSM.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private readonly ILocalStorageService _localStorage;

    private ClaimsPrincipal _user =
        new(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Name, "user"), new(ClaimTypes.Role, "user") },
            "CustomAuth"));

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Get user token from the local storage.
            var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

            // If the token is null, consider the authentication as anonymous (or non-authorized).
            return userToken == null
                ? await Task.FromResult(new AuthenticationState(_anonymous)).ConfigureAwait(false)
                : await Task.FromResult(new AuthenticationState(_user)).ConfigureAwait(false);
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous)).ConfigureAwait(false);
        }
    }

    public async Task UpdateAuthenticationStateAsync(string userToken)
    {
        if (!string.IsNullOrEmpty(userToken))
        {
            await _localStorage.SetItemAsStringAsync(StorageConstants.UserToken, userToken).ConfigureAwait(false);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }
        else
        {
            await _localStorage.RemoveItemAsync(StorageConstants.UserToken).ConfigureAwait(false);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public async Task UpdateAuthenticationStateAsync(string userToken, string refreshToken)
    {
        if (!string.IsNullOrEmpty(userToken) && !string.IsNullOrEmpty(refreshToken))
        {
            await _localStorage.SetItemAsStringAsync(StorageConstants.UserToken, userToken).ConfigureAwait(false);
            await _localStorage.SetItemAsStringAsync(StorageConstants.RefreshToken, refreshToken).ConfigureAwait(false);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }
        else
        {
            await _localStorage.RemoveItemAsync(StorageConstants.UserToken).ConfigureAwait(false);
            await _localStorage.RemoveItemAsync(StorageConstants.RefreshToken).ConfigureAwait(false);
            await _localStorage.RemoveItemAsync(StorageConstants.AspNetUserId).ConfigureAwait(false);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public void UpdateAuthenticationState(TokenResult userData)
    {
        var claim = (from c in _user.Claims
            where c.Type == ClaimTypes.Expiration
            select c).FirstOrDefault();

        var identity = _user.Identity as ClaimsIdentity;
        identity?.RemoveClaim(claim);
        identity?.AddClaim(new Claim(ClaimTypes.Expiration, userData.expiryTime.ToString()));
    }

    public void UpdateAuthenticationState(IEnumerable<string>? userPermissions, UserInfoDto userInfoDto)
    {
        if (null == userPermissions)
        {
            return;
        }

        var claimsList = new List<Claim>
        {
            new(ClaimTypes.Name, "user"),
            new(ClaimTypes.Role, "user"),
            new(ClaimTypes.UserData, userInfoDto.AspnetUserId),
            new(ClaimTypes.Expiration, userInfoDto.ExpiryTime.ToString())
        };
        claimsList.AddRange(userPermissions.Select(per => new Claim(ClaimTypes.Role, per)));

        _user = new ClaimsPrincipal(new ClaimsIdentity(claimsList, "CustomAuth"));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
    }
}
