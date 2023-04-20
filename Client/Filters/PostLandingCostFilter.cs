namespace EDMS.DSM.Client.Filters;

public class PostLandingCostFilter : PaginationFilter
{
    public string Destination { get; set; } = string.Empty;
}
