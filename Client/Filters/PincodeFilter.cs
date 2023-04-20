namespace EDMS.DSM.Client.Filters;

public class PincodeFilter : PaginationFilter
{
    public string Pincode { get; set; } = string.Empty;
    public string Taluka { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
