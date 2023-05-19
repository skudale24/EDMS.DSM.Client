using Microsoft.AspNetCore.Http;

namespace EDMS.DSM.Client.Shared;

public partial class Login : ComponentBase
{
    [Inject] private NavigationManager _navManager { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var fullUri = _navManager.ToAbsoluteUri(_navManager.Uri);

        if (fullUri != null)
        // Get `userToken` and 'refreshToken' from the Uri.
        {
            _ = _snackbar.Add($"FullUri. {fullUri}", Severity.Info);

            if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut))
            //&& _navManager.TryGetQueryString(StorageConstants.RefreshToken, out string refreshTokenOut))
            {
                if (!string.IsNullOrWhiteSpace(userTokenOut))
                {
                    _ = _snackbar.Add($"UserToken. {userTokenOut}", Severity.Info);

                    // Use retrieved `userToken` to update authentication state.
                    //await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, userTokenOut)
                    //    .ConfigureAwait(false);

                    _navManager.NavigateTo($"/?programId={2}&userId=10572&_z={userTokenOut}", true, true);

                    return;
                }
            }
        }

        var returnUrl = _navManager.GetUriWithQueryParameter("1", "1");
        var hosturl = $"{EndPoints.LoginPage}/?at={AppConstants.AppTokenValue}&returnUrl=";
        var urlBytes = Encoding.UTF8.GetBytes(returnUrl);
        var encodedReturnUrl = Convert.ToBase64String(urlBytes);

        //TODO: Set parent page url from config
        _navManager.NavigateTo($"http://localhost:53398/Index.aspx");
    }

    protected override async Task OnParametersSetAsync()
    {
        //var fullUri = _navManager.ToAbsoluteUri(_navManager.Uri);

        //if (fullUri != null)
        //// Get `userToken` and 'refreshToken' from the Uri.
        //{
        //    if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut))
        //    //&& _navManager.TryGetQueryString(StorageConstants.RefreshToken, out string refreshTokenOut))
        //    {
        //        if (!string.IsNullOrWhiteSpace(userTokenOut))
        //        {
        //            await LocalStorage.SetItemAsStringAsync(StorageConstants.UserToken, userTokenOut).ConfigureAwait(false);

        //            //// Use retrieved `userToken` to update authentication state.
        //            //await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, userTokenOut)
        //            //    .ConfigureAwait(false);

        //            _navManager.NavigateTo("/?programId={2}&userId=10572", true, true);
        //            return;
        //        }
        //    }
        //}

        //var returnUrl = _navManager.GetUriWithQueryParameter("1", "1");
        //var hosturl = $"{EndPoints.LoginPage}/?at={AppConstants.AppTokenValue}&returnUrl=";
        //var urlBytes = Encoding.UTF8.GetBytes(returnUrl);
        //var encodedReturnUrl = Convert.ToBase64String(urlBytes);
        //_navManager.NavigateTo($"http://localhost:53398/Index.aspx");

        //await base.OnParametersSetAsync();
    }
}
