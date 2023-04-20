namespace EDMS.DSM.Client.Authentication;

public interface IAuthenticationManager
{
    Task<string> GetCurrentUserAsync();
    Task<string> LogoutAsync();
}
