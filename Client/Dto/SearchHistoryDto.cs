namespace EDMS.DSM.Client.DTO;

public class SearchHistoryDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public PriceInputReportDto SearchInfo { get; set; } = new();
    public string SearchedBy { get; set; } = string.Empty;
    public DateTime SearchedOn { get; set; } = DateTime.UtcNow;
}

public class PriceInputReportDto
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Customers { get; set; } = string.Empty;
    public int UserId { get; set; } = 0;
}

public class SearchHistorySearchedBy
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
}
