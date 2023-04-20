namespace EDMS.DSM.Client.Filters;

public class TransportationCostFilter : PaginationFilter
{
    public string Pod { get; set; } = string.Empty;
    public string FromZipCode { get; set; } = string.Empty;
    public string ToZipCode { get; set; } = string.Empty;
}
