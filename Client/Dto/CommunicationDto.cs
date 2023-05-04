namespace EDMS.DSM.Client.DTO;

public class CommunicationDTO
{
    public long LPCID { get; set; }
    public int TemplateType { get; set; }
    public string? TemplateName { get; set; }
    public string? CompanyName { get; set; }
    public string? Action { get; set; }
    public string? GeneratedDate { get; set; }
    public string? GeneratedBy { get; set; }
    public string? GeneratedFilePath { get; set; }
    public int TemplateId { get; set; }
    public int CountofApplications { get; set; }
    public string? FilePath { get; set; }
    public bool IsButtonDisabled { get; set; } = false;
    public bool IsProcessing { get; set; } = false;
}
