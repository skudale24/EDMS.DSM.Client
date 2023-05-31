using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace EDMS.DSM.Client.Shared;

public partial class Login : ComponentBase
{
    [Inject] private NavigationManager _navManager { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    private int _programId;
    private int _generatedById;

    protected override async Task OnInitializedAsync()
    {
        var fullUri = _navManager.ToAbsoluteUri(_navManager.Uri);

        if (fullUri != null)
            // Get `userToken` and 'refreshToken' from the Uri.
            if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut))
                //&& _navManager.TryGetQueryString(StorageConstants.RefreshToken, out string refreshTokenOut))
                if (!string.IsNullOrWhiteSpace(userTokenOut) && !string.IsNullOrWhiteSpace(userTokenOut))
                {
                    var claims = GetClaimsFromToken(userTokenOut);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "UserID")?.Value, out _generatedById);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "ProgramID")?.Value, out _programId);

                    // Use retrieved `userToken` to update authentication state.
                    await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, userTokenOut);
                    _navManager.NavigateTo($"/?userId={_generatedById}&programId={_programId}", true, true);
                    return;
                }

        _navManager.NavigateTo($"{EndPoints.APBaseUrl}/Index.aspx");

        //var returnUrl = _navManager.GetUriWithQueryParameter("1", "1");
        //var hosturl = $"{EndPoints.LoginPage}/?returnUrl=";
        //var urlBytes = Encoding.UTF8.GetBytes(returnUrl);
        //var encodedReturnUrl = Convert.ToBase64String(urlBytes);
    }

    private List<Claim> GetClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Claims.ToList();
    }
}
