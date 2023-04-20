namespace EDMS.DSM.Client.Filters;

public class QuoteSearchResultFilter : PaginationFilter
{
    public string? Type { get; set; }
    public string PinCode { get; set; } = string.Empty;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string EfreightNo { get; set; } = string.Empty;
    public string QuoteNumber { get; set; } = string.Empty;
}
