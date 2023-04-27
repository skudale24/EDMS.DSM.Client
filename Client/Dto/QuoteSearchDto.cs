namespace EDMS.DSM.Client.DTO;

public class QuoteSearchDto
{
    public int Id { get; set; }
    public int SearchReportId { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<string> SearchInfo { get; set; } = new();
    public string SearchedBy { get; set; } = string.Empty;
    public DateTime SearchedOn { get; set; } = DateTime.UtcNow;
    public string QuoteNo { get; set; } = string.Empty;
    public string EFreightBkgNo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class SearchedBy
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
