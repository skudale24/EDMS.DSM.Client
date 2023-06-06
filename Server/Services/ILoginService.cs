using EDMS.DSM.Server.Security;

namespace EDMS.DSM.Server.Services
{
    public interface ILoginService
    {
        //Task AddInitCacheForUser(string aspnetUserId, string userToken, RefreshToken refreshToken);
        //Task<string> GetCacheUserToken(string aspnetUserId);
        //Task LogOutUserAsync(string aspnetUserId);
        Task<RefreshToken> RefreshTokenAsync(string aspnetUserId, string token,int rt);
    }
}
