﻿namespace EDMS.DSM.Client.Filters;

public class ExworksRateFilter : PaginationFilter
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
}
