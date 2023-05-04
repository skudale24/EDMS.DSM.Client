using EDMS.DSM.Shared.Models;

namespace EDMS.DSM.Client.Extensions;

public static class GridExtensions
{
    public static List<CommunicationDTO> ToCommunicationGrid(this List<Communications> items)
    {
        return items.Select(x => new CommunicationDTO
        {
            Action = x.ActionText,
            CompanyName = x.CompanyName,
            CountofApplications = x.CountofApplications,
            FilePath = x.FilePath,
            GeneratedBy = x.GeneratedBy,
            GeneratedDate = x.GeneratedDate,
            LPCID = x.LPCID,
            TemplateId = x.TemplateId,
            TemplateName = x.TemplateName,
            TemplateType = x.TemplateType,
            GeneratedFilePath = x.GeneratedFilePath
        }).ToList();
    }
}

