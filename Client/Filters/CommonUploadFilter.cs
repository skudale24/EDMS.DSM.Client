namespace EDMS.DSM.Client.Filters;

public class CommonUploadFilter : PaginationFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Status { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public string UploadedBy { get; set; } = default!;
}
