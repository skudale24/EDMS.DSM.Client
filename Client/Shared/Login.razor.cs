namespace EDMS.DSM.Client.Shared;

public partial class Login : ComponentBase
{
    [Inject] private NavigationManager _navManager { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var fullUri = _navManager.ToAbsoluteUri(_navManager.Uri);

        if (fullUri != null)
            // Get `userToken` and 'refreshToken' from the Uri.
        {
            if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut)
                && _navManager.TryGetQueryString(StorageConstants.RefreshToken, out string refreshTokenOut))
            {
                if (!string.IsNullOrWhiteSpace(userTokenOut) && !string.IsNullOrWhiteSpace(refreshTokenOut))
                {
                    // Use retrieved `userToken` to update authentication state.
                    await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, refreshTokenOut)
                        .ConfigureAwait(false);
                    _navManager.NavigateTo("/", true, true);
                    return;
                }
            }
        }

        var returnUrl = _navManager.GetUriWithQueryParameter("1", "1");
        var hosturl = $"{EndPoints.LoginPage}/?at={AppConstants.AppTokenValue}&returnUrl=";
        var urlBytes = Encoding.UTF8.GetBytes(returnUrl);
        var encodedReturnUrl = Convert.ToBase64String(urlBytes);
        _navManager.NavigateTo($"{hosturl}{encodedReturnUrl}");
    }
}
