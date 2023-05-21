using EDMS.DSM.Server.Models;
using EDMS.Shared.Constants;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
                                        ILogger<AuthenticationController> logger) : base(httpContextAccessor, configuration)
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
                    // Claims not found in the request header, handle the error
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

                _logger.LogInformation($"{userId}:{programId}");

                // Construct the URL with the user ID appended as a query string parameter
                string url = $"{_configuration["CCGridUrl"]}";

                _logger.LogInformation($"{url}");

                // Generate the JWT token (implement your own logic)
                string jwtToken = GenerateJwtToken(userId, programId);

                _logger.LogInformation($"{jwtToken}");

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
                string[] keyValue = claimLine.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    claims[key] = value;
                }
            }

            return claims;
        }

        /// <summary>
        /// Generates the Access (JWT) token using the Private Key
        /// TODO: Move the Private Key to configuration section
        /// </summary>
        /// <returns></returns>
        private string GenerateJwtToken(string userId, string programId)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);
            var claims = new List<Claim>
            {
                new Claim("UserID", userId),
                new Claim("ProgramID", programId),
            };
            var token = new JwtSecurityToken(
                issuer: $"{_configuration["Jwt:Issuer"]}",
                audience: $"{_configuration["Jwt:Audience"]}",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: claims
            );
            token.Header.Add("kid", "your_key_id");
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
