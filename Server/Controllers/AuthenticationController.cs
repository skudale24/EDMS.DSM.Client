using EDMS.DSM.Server.DTO;
using EDMS.DSM.Server.Models;
using EDMS.DSM.Server.Security;
using EDMS.Shared.Constants;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EDMS.DSM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<AuthenticationController> logger) :
                                            base(httpContextAccessor,
                                                 configuration,
                                                 logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Returns Access Token (JWT) for Customer Communications
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateToken")]
        public IActionResult GenerateToken()
        {
            try
            {
                // Retrieve the prepared claims from the request header
                string preparedClaimsBase64 = Request.Headers["X-Claims"];

                if (string.IsNullOrEmpty(preparedClaimsBase64))
                {
                    // Claims not found in the request header
                    return BadRequest(new APIResponse { Message = "Claims not provided" });
                }

                _logger.LogInformation(preparedClaimsBase64);

                // Decode the Base64 string to byte array
                byte[] preparedClaims = Convert.FromBase64String(preparedClaimsBase64);

                // Convert the prepared claims bytes to string using UTF-8 encoding
                string claims = Encoding.UTF8.GetString(preparedClaims);

                _logger.LogInformation(claims);

                //// Prepare the claims for sending
                //byte[] preparedClaims = PrepareClaims(claims);

                // Extract the user ID from the claims (assuming it is a string)
                //string claimsString = AP.Common.PGPHelper.DecryptClaims(preparedClaims); // Assuming you have a method to decrypt the prepared claims
                Dictionary<string, string> parsedClaims = ParseClaims(claims);
                string userId = parsedClaims["UserID"];
                string programId = parsedClaims["ProgramID"];
                string sessionExpiration = parsedClaims["SessionExpiration"];
                _logger.LogInformation($"{sessionExpiration} {userId}:{programId}");
                string url = $"{_configuration["CCGridUrl"]}";
                _logger.LogInformation($"{url}");

                // Generate the JWT token
                string jwtToken = GenerateJwtToken(userId, programId, sessionExpiration);
                var refreshToken = GenerateRefreshToken(sessionExpiration);
                _logger.LogInformation($"{jwtToken}");
                _logger.LogInformation($"{refreshToken.Token}");

                // Return the URL and JWT token in the response header
                Response.Headers.Add("X-URL", url);
                Response.Headers.Add("X-JWT-Token", jwtToken);
                Response.Headers.Add("X-JWT-Token-Key", StorageConstants.UserToken);

                // Return a success response
                return Ok(new APIResponse { Message = "Token generated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} : {ex.StackTrace}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        [HttpPost("Regenerate")]
        public async Task<IApiResult> RegenerateToken([FromBody] UserInfoDto userInfoDto)
        {
            try
            {
                int UT = 24;

                _logger.LogInformation($"{userInfoDto.ExpiryTime} {userInfoDto.AspnetUserId}:{userInfoDto.ProgramId}");

                string sessionExpiration = DateTime.Now.AddMinutes(userInfoDto.TimeOutMinutes).ToString("yyyy-MM-dd HH:mm:ss");

                // Generate the JWT token
                string userToken = GenerateJwtToken(userInfoDto.AspnetUserId.ToString(), userInfoDto.ProgramId, sessionExpiration);
                
                _logger.LogInformation($"{userToken}");

                Response.Headers.Add(StorageConstants.UserToken, userToken);

                return ApiResult<UserInfoDto>.Success(new UserInfoDto
                {
                    UserToken = userToken,
                    ExpiryTime = DateTime.UtcNow.AddHours(UT).Ticks
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} : {ex.StackTrace}");
                return ApiResult<UserInfoDto>.Fail(ex.Message);
            }
        }

        [HttpGet("refresh")]
        public async Task<IApiResult> RefreshToken([FromQuery] string aspnetUserId, [FromHeader] string reftoken)
        {
            string userId = "";
            string programId = "";
            string sessionExpiration = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd HH:mm:ss");
            var refreshToken = GenerateRefreshToken(sessionExpiration);
            string userToken = GenerateJwtToken(userId, programId, sessionExpiration);

            //Take expiry hours from db
            int UT = 24;
            int RT = 7;

            //    var r2 = _dbcontext.ConfigurationData.FirstOrDefault(c => c.groupkey == AuthConstants.DExpiry &&
            //                   c.configkey == AuthConstants.RT)?.configvalue;

            //    if (!string.IsNullOrEmpty(r2))
            //    {
            //        RT = int.Parse(r2);
            //    }

            //Validate and Create refresh Token
            //RefreshToken refreshToken = await _loginService.RefreshTokenAsync(aspnetUserId, reftoken, RT);

            //if (refreshToken == null || string.IsNullOrEmpty(refreshToken.Token))
            //{
            //    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return (ApiResult.Fail("Unauthorized User."));

            //}

            //    long orgId = await _userRepo.UserOrgId(appToken);

            //    //Add logic for checking if existing usertoken is valid in redis cache if valid return same else create new


            //    var r1 = (_dbcontext.ConfigurationData.FirstOrDefault(c => c.groupkey == AuthConstants.HExpiry &&
            //    c.configkey == AuthConstants.UT)?.configvalue);
            //    if (!string.IsNullOrEmpty(r1))
            //    {
            //        UT = int.Parse(r1);
            //    }

            //    //Generate new user token  
            //var _loginExpiryDateTime = Security.SecurityToken.DateTimeToUnixTimestamp(DateTime.UtcNow.AddHours(UT));
            //var _securityToken = new Security.SecurityToken(_loginExpiryDateTime, aspnetUserId, 1, "");
            //string userToken = _securityToken.Encryption();

            UserInfoDto userInfo = new()
            {
                //UserName = string.Concat(userdata.firstname, " ", userdata.lastname),
                //EmailAddress = userdata.emailaddress,
                AspnetUserId = aspnetUserId,
                //OrgId = orgId,
                UserToken = userToken,
                RefreshToken = refreshToken,
                //UserId = aspnetUserId,
                ExpiryTime = DateTime.UtcNow.AddHours(UT).Ticks
            };

            return (ApiResult<UserInfoDto>.Success(userInfo));
        }

        /// <summary>
        /// Prepares the claims for sending
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private byte[] PrepareClaims(string claims)
        {
            // Convert the claims string to bytes
            byte[] claimsBytes = System.Text.Encoding.UTF8.GetBytes(claims);

            // Return the claims bytes as-is
            return claimsBytes;
        }

        /// <summary>
        /// Parses the claims string into a dictionary
        /// </summary>
        /// <param name="claimsString"></param>
        /// <returns></returns>
        private Dictionary<string, string> ParseClaims(string claimsString)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();

            string[] claimLines = claimsString.Split('\n');
            foreach (string claimLine in claimLines)
            {
                int colonIndex = claimLine.IndexOf(':');
                if (colonIndex >= 0)
                {
                    string key = claimLine.Substring(0, colonIndex).Trim();
                    string value = claimLine.Substring(colonIndex + 1).Trim();
                    claims[key] = value;
                }
            }

            return claims;
        }

        /// <summary>
        /// Generates the Access (JWT) token using the Private Key
        /// </summary>
        /// <returns></returns>
        private string GenerateJwtToken(string userId, string programId, string sessionExpiration)
        {

            if (DateTime.TryParseExact(sessionExpiration, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expires))
            {
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);

                TimeSpan timeoutDuration = expires - DateTime.Now;
                int timeoutInMinutes = (int)timeoutDuration.TotalMinutes + 1;

                var claims = new List<Claim>
                {
                    new Claim("UserID", userId),
                    new Claim("ProgramID", programId),
                    new Claim("exp", expires.ToString(), ClaimValueTypes.Integer),
                    new Claim("timeout", timeoutInMinutes.ToString(), ClaimValueTypes.Integer),
                };
                var token = new JwtSecurityToken(
                    issuer: $"{_configuration["Jwt:Issuer"]}",
                    audience: $"{_configuration["Jwt:Audience"]}",
                    expires: expires,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    claims: claims
                );
                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
            else
            {
                throw new Exception("Invalid token expiry.");
            }
        }

        private RefreshToken GenerateRefreshToken(string sessionExpiration)
        {
            DateTime expires;
            if (DateTime.TryParseExact(sessionExpiration, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out expires))
            {
                int expiryInDays = expires.Subtract(DateTime.Now).Days;
                RefreshToken refreshToken = Security.SecurityToken.CreateRefreshToken(expiryInDays);
                return refreshToken;
            }
            else
            {
                throw new Exception("Invalid token expiry.");
            }
        }
    }
}
