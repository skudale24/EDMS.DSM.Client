using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using VTI.Common;
using EDMS.DSM.Client.Pages.Support;
using EDMS.DSM.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EDMS.DSM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // POST: api/Authentication/GenerateToken
        [HttpGet("GenerateToken")]
        public IActionResult GenerateToken()
        {
            // Retrieve the prepared claims from the request header
            string preparedClaimsBase64 = Request.Headers["X-Claims"];

            if (string.IsNullOrEmpty(preparedClaimsBase64))
            {
                // Claims not found in the request header, handle the error
                return BadRequest(new APIResponse { Message = "Claims not provided" });
            }

            // Decode the Base64 string to byte array
            byte[] preparedClaims = Convert.FromBase64String(preparedClaimsBase64);

            // Convert the prepared claims bytes to string using UTF-8 encoding
            string claims = System.Text.Encoding.UTF8.GetString(preparedClaims);

            //byte[] byteClaims = Convert.FromBase64String(claims);

            //////// Prepare the claims for sending
            //byte[] preparedClaims = PrepareClaims(claims);

            //// Convert the prepared claims to Base64 string for transmission or storage
            //string preparedClaimsBase64 = Convert.ToBase64String(preparedClaims);

            // Your logic to extract the user ID from the claims (assuming it is a string)
            //string claimsString = AP.Common.PGPHelper.DecryptClaims(preparedClaims); // Assuming you have a method to decrypt the prepared claims
            Dictionary<string, string> parsedClaims = ParseClaims(claims);
            string userId = parsedClaims["UserID"];
            string programId = parsedClaims["ProgramID"];

            //TODO: Make base url configurable
            // Your logic to construct the URL with the user ID appended as a query string parameter
            string url = $"https://localhost:5001?userId={userId}&programId={programId}";

            // Generate the JWT token (implement your own logic)
            string jwtToken = GenerateJwtToken();

            // Return the URL and JWT token in the response header
            Response.Headers.Add("X-URL", url);
            Response.Headers.Add("X-JWT-Token", jwtToken);

            // Return a success response
            return Ok(new APIResponse { Message = "Token generated successfully" });
        }

        // Prepare the claims for sending
        private byte[] PrepareClaims(string claims)
        {
            // Convert the claims string to bytes
            byte[] claimsBytes = System.Text.Encoding.UTF8.GetBytes(claims);

            // Return the claims bytes as-is
            return claimsBytes;
        }

        // Method to parse the claims string into a dictionary
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


        //// Extract the user ID from the claims (implement your own logic based on your claims structure)
        //private string ExtractUserIdFromClaims(string preparedClaimsBase64)
        //{
        //    // Implement your logic to extract the user ID from the claims here
        //    // This can be based on the structure of your claims or any other criteria you have
        //    // Parse the Base64 claims string and extract the relevant information

        //    // For demonstration purposes, assuming the claims string is a JSON object
        //    // Deserialize the claims JSON string and extract the user ID field
        //    // Adjust this logic based on the actual structure of your claims
        //    var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(preparedClaimsBase64);
        //    string userId = claims["UserID"];

        //    return userId;
        //}

        // Generate the JWT token (implement your own logic)
        private string GenerateJwtToken()
        {
            // Implement your logic to generate the JWT token here
            // This can be based on your specific requirements and the libraries/frameworks you are using

            // For demonstration purposes, assuming a simple token generation
            // Replace this logic with your own JWT token generation logic
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

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001/api/authenticate/generatetoken",
                audience: "https://localhost:5001",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}

