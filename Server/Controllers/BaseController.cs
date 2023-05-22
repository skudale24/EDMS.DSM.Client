using EDMS.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EDMS.DSM.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        public string AspnetUserId { get; set; } = string.Empty;
        //public string AppToken { get; set; } = string.Empty;
        public long ProgramId { get; set; } = 1;
        public int[] Roles { get; set; }
        public BaseController(IHttpContextAccessor httpContextAccessor,
                              IConfiguration configuration,
                              ILogger<AuthenticationController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _logger.LogInformation("Inside BaseController: ");

            if (httpContextAccessor == null)
            {
                //throw new ArgumentNullException(nameof(httpContextAccessor));
                _logger.LogInformation("httpContextAccessor is null");
                return;
            }

            _logger.LogInformation("httpContextAccessor is available");

            var httpContext = httpContextAccessor.HttpContext;
            var claimsIdentity = httpContext?.User?.Identity as ClaimsIdentity;

            if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
            {
                _logger.LogInformation($"claimsIdentity.IsAuthenticated {claimsIdentity.IsAuthenticated}");

                var claimsData = claimsIdentity.Claims;
                if (claimsData != null && claimsData.Any())
                {
                    _ = long.TryParse(claimsData.FirstOrDefault(c => c.Type == "ProgramID")?.Value, out long res);
                    ProgramId = res;
                }

                //AspnetUserId = claimsIdentity.Name;
            }

            //            var xClaimsHeader = httpContext?.Request.Headers[AppConstants.UserTokenHeaderKey];
            //            if (!string.IsNullOrEmpty(xClaimsHeader))
            //            {
            //                var publicKey = @"-----BEGIN RSA PUBLIC KEY-----
            //MIIBCgKCAQEAwu6q6nHGrLoa8/U7pcJLy9KFBcPSfSEojnQ8jhE0lhFGRuLW7FeW
            //rpStUp5w0E8jYSkyM2Q9n7gwmdn0v4tuzmZO1/C3XiBgCNlY+v10MIB3w8b+Uudj
            //eOytYNDsVcfJiYVpw/TcVgrTyxtlRnb1lP8GzQuBEvm8x8ITOeLeEfdQeftwGnFt
            //nvCz9CofKYOeEKoi1iIltKljE42nzBefbiErGlJvFGLP0yVgxLfhRUp4ViIAnNsX
            //mi4fP1nqvMaLpkZOgHd+WbcRF+0iSLoWnF8DyrWENHOuoFMoq+fK6mJynByXG9ke
            //yEcCPqzFuELEbUCai9h8+A20SJS/ZrBWrwIDAQAB
            //-----END RSA PUBLIC KEY-----
            //";

            //                RSAParameters rsaParameters;
            //                using (var rsa = RSA.Create())
            //                {
            //                    rsa.ImportFromPem(publicKey);
            //                    rsaParameters = rsa.ExportParameters(false);
            //                }

            //                var securityKey = new RsaSecurityKey(rsaParameters);
            //                IEnumerable<SecurityKey> signingKeys = new List<SecurityKey> { securityKey };
            //                var token = xClaimsHeader.ToString();
            //                var handler = new JwtSecurityTokenHandler();
            //                var tokenValidationParameters = new TokenValidationParameters
            //                {
            //                    ValidateIssuer = true,
            //                    ValidateAudience = true,
            //                    ValidateLifetime = false,
            //                    ValidateIssuerSigningKey = true,
            //                    IssuerSigningKeys = signingKeys,
            //                    ValidIssuer = _configuration["Jwt:Issuer"],
            //                    ValidAudience = _configuration["Jwt:Audience"],
            //                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(publicKey)),
            //                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) => new List<SecurityKey> { securityKey },
            //                    //IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
            //                    //{
            //                    //    // Implement your custom logic to retrieve the key based on the key identifier (kid)
            //                    //    // This can involve fetching the key from a key repository or using a predefined mapping

            //                    //    // Example logic: Check if the provided key identifier matches the expected key identifier
            //                    //    if (kid == "your_key_id")
            //                    //    {
            //                    //        // Return the corresponding key
            //                    //        return new List<SecurityKey> { securityKey };
            //                    //    }
            //                    //    return null; // Key not found for the specified key identifier
            //                    //}
            //                    // Provide a callback function to dynamically retrieve the key based on the key identifier (kid)
            //                    // The function takes the key identifier as input and should return the corresponding key
            //                    //IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
            //                    //{
            //                    //    // Implement your custom logic to retrieve the key based on the key identifier (kid)
            //                    //    // This can involve fetching the key from a key repository or using a predefined mapping

            //                    //    // Example logic: Check if the provided key identifier matches the expected key identifier
            //                    //    if (kid == "your_key_id")
            //                    //    {
            //                    //        // Return the corresponding key
            //                    //        return new List<SecurityKey> { securityToken.SecurityKey };
            //                    //    }
            //                    //    return null;
            //                    //}
            //                };

            //                try
            //                {
            //                    // Validate the JWT token here
            //                    var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            //                    // Token is valid, perform additional logic if needed
            //                }
            //                catch (SecurityTokenValidationException ex)
            //                {
            //                    // Token validation failed, handle the error condition
            //                }
        }
        //AppToken = httpContextAccessor.HttpContext?.Request.Headers["AppToken"];
    }
}
