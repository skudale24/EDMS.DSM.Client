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
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger)
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
            var privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAwu6q6nHGrLoa8/U7pcJLy9KFBcPSfSEojnQ8jhE0lhFGRuLW
7FeWrpStUp5w0E8jYSkyM2Q9n7gwmdn0v4tuzmZO1/C3XiBgCNlY+v10MIB3w8b+
UudjeOytYNDsVcfJiYVpw/TcVgrTyxtlRnb1lP8GzQuBEvm8x8ITOeLeEfdQeftw
GnFtnvCz9CofKYOeEKoi1iIltKljE42nzBefbiErGlJvFGLP0yVgxLfhRUp4ViIA
nNsXmi4fP1nqvMaLpkZOgHd+WbcRF+0iSLoWnF8DyrWENHOuoFMoq+fK6mJynByX
G9keyEcCPqzFuELEbUCai9h8+A20SJS/ZrBWrwIDAQABAoIBAQCRVZx8m0ODOGYs
CWTlWw1j85tWBwACZxxzyVn0mgNY95wr8ahIIa2okBUBijuWM8qvACX7hvsjABLM
QmJmBTEiFckm2sP8G6s+Lb5Xs2xTVRT2FIE9kmLpn5xMmLf75K7wJ1YJ0aANTplF
zzEc7kh/Q/zwF2mumiAT1zC6cdkcDrX272w3BBI2eYoAikfbkiMb8F1MABhxue44
Al09HZQvTmjIgvMnzwDUvM0M2vaqVYHkY2PNM+5TDr69nxfMTuGbLQZvBl60bK+b
FrehCEqUcYvktzVbUec47t9UsMDl/YU19ILGyYdycwbOFQ/c0EbP192ckxVY394S
m8NqMOjxAoGBAP/Vub3zLBHKOI67Z7CR5YqBtfRTX4imVAXVNTp7UNRTl1kNw3rS
ZbrKvUtsp5ffRgbHNpC5kK/RILUbqIWrmjgTslv4eb7/4qEz+uPUqnt/7Y8N2yyg
eiBwqndsIW8IVo3n3wp4cx5KpJRFPPs5hCNnFCojkO9MgW/JCVi678fjAoGBAMMO
4OO8Pfxj1fmkKZySmSs9SYhyHPI8kAXqhmEO9PRtGxKnlB8sdwcI8WJXfLsqysIg
wKmx0AeTwNFhz3CN/UdKnmrdrhoUedRT8vvynC/29gHURut6qaTDT5T4T6PIUc9f
YxcICEKW1TD5J4rGqxP+VQZvobtQi91LBvaai3fFAoGBAMrzq7Pqcu1x9MgWFz3V
n7jvCX9XnJP1DvbJmr5YEVk/LvNwncCTpCw4pU5uVc1/TDgYVUseSo+PYVkLJfdU
mLUfuwCG71379LdZWxDeJphudfBkV5jhfcC6YCD44NUKUk+kwCW4Q+ql4EXXX3cU
u6SyIycCq/mKQayCWS3QrmNtAoGAZ/uxhZYfUUIDmr6Z9D7uam+UmKmEptoESTMa
CfnIOdlEGnC9dNTmaxioXa2X78tDJbQCITSKWs+4daZ3yF/ZSr0LsJqWqo6J19gc
65UNEEDOKnF4kSXl89CuxxKMmho7CpqmH2wHwz/XTPE11DROlSz7NKAkDtBEj2bk
wmfmu6UCgYAmccel1/eMDbjsCZjGfwv+o1NWV4rPBsRssMtXxBk16iInNhoYZVVX
khAPZGns84S9A45QDhFuxNUa8z9GMKe0aTFGCQVN6JKJJXr9lSbxueuwiZFt5rMg
iylx4utOIgNNzNx51tiFEqcukLBTclPEMXiodvAKL4Po5rN12QO6GQ==
-----END RSA PRIVATE KEY-----
"; // Replace with your RS private key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserID", userId),
                new Claim("ProgramID", programId),
            };

            var token = new JwtSecurityToken(
                issuer: $"{_configuration["CCGridUrl"]}/api",
                audience: $"{_configuration["CCGridUrl"]}",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials,
                claims: claims
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
