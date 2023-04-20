namespace EDMS.DSM.Client.Pages.Support;

public partial class Claims
{
    private string? authMessage;
    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string surnameMessage;

    private async Task GetClaimsPrincipalData()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        var user = authState.User;
        if (user.Identity.IsAuthenticated)
        {
            authMessage = $"{user.Identity.Name} is authenticated.";
            claims = user.Claims;
            surnameMessage = $"Surname: {user.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value}";
        }
        else
        {
            authMessage = "The user is NOT authenticated.";
        }
    }
}
