using EDMS.DSM.Shared.Models;
using System.IdentityModel.Tokens.Jwt;

namespace EDMS.DSM.Client.Extensions;

public static class GridExtensions
{
    public static List<CommunicationDTO> ToCommunicationGrid(this List<Communication> items)
    {
        return items.Select(x => new CommunicationDTO
        {
            ActionText = x.ActionText,
            CompanyName = x.CompanyName,
            CountofApplications = x.CountofApplications,
            FilePath = x.FilePath,
            GeneratedBy = x.GeneratedBy,
            GeneratedDate = x.GeneratedDate?.Date ?? null,
            LPCID = x.LPCID,
            TemplateId = x.TemplateId,
            TemplateName = x.TemplateName,
            TemplateType = x.TemplateType,
            GeneratedFilePath = x.GeneratedFilePath,
            BatchId = x.BatchId
        }).ToList();
    }

    public static UserInfoDto GetUserInfoFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value, out int generatedById);
        int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "ProgramID")?.Value, out int programId);
        int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "timeout")?.Value, out int timeout);
        //long.TryParse(claims.FirstOrDefault(c => c.Type == "ExpiryTime")?.Value, out long expiryTimeTicks);

        return new UserInfoDto
        {
            AspnetUserId = generatedById.ToString(),
            ProgramId = programId.ToString(),
            Expires = jwtToken.ValidTo,
            TimeOutMinutes = timeout
        };
    }
}

