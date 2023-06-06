

using EDMS.DSM.Server.Security;

namespace EDMS.DSM.Server.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;

        public LoginService(ILogger<LoginService> logger)
        {
            _logger = logger;
        }

        public async Task<RefreshToken> RefreshTokenAsync(string aspnetUserId, string token, int rt)
        {

            return new RefreshToken();

            //    var existingRefreshToken = refreshTokens.FirstOrDefault(r => r.Token == token);
            //    if (existingRefreshToken?.Expires < DateTime.UtcNow)
            //    {
            //       // return null;
            //        var refreshToken = SecurityToken.CreateRefreshToken(rt);
            //        #region update user last login

            //        User user = await _userRepo.GetUserAsync(aspnetUserId);
            //        if (user != null)
            //        {
            //            user.updatedon = DateTime.UtcNow;
            //            user.lastlogin = DateTime.UtcNow;
            //            await _userRepo.UpdateUserLastLoginAsync(user);
            //        }
            //        #endregion

            //        await _userCacheSevice.AddAndRemoveRefreshTokenAsync(aspnetUserId, refreshToken, existingRefreshToken.Token);
            //        return refreshToken;
            //    }
            //    else
            //    {
            //        return existingRefreshToken;
            //    }

        }
    }
}
