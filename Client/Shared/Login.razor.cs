using System.IdentityModel.Tokens.Jwt;

namespace EDMS.DSM.Client.Shared;

public partial class Login : ComponentBase
{
    [Inject] private NavigationManager _navManager { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    [Inject] private ILogger<Login> _logger { get; set; } = default!;

    private int _programId;
    private int _generatedById;
    private DateTime expires;

    protected override async Task OnInitializedAsync()
    {
        var fullUri = _navManager.ToAbsoluteUri(_navManager.Uri);

        if (fullUri != null)
            if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut))
                //&& _navManager.TryGetQueryString(StorageConstants.RefreshToken, out string refreshTokenOut))
                if (!string.IsNullOrWhiteSpace(userTokenOut))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.ReadJwtToken(userTokenOut);

                    if (securityToken is JwtSecurityToken jwtSecurityToken)
                    {
                        expires = jwtSecurityToken.ValidTo;
                    }

                    var claims = GetClaimsFromToken(userTokenOut);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "UserID")?.Value, out _generatedById);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "ProgramID")?.Value, out _programId);
                    
                    // Use retrieved `userId` to update authentication state.
                    var userInfoDto = new UserInfoDto
                    {
                        AspnetUserId = _generatedById.ToString(),
                        ProgramId = _programId.ToString(),
                        ExpiryTime = expires.Ticks
                    };

                    _logger.LogInformation($"Current DateTime {DateTime.UtcNow}, Expiry Time: {expires}");

                    // Use retrieved `userToken` to update authentication state.
                    await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, userTokenOut, userInfoDto);

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
