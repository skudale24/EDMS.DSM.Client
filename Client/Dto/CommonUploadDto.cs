namespace EDMS.DSM.Client.DTO;

public class CommonUploadDto
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string FileName { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public string ActualFileName { get; set; } = default!;
    public DateTime UploadedDate { get; set; }
    public string UploadedBy { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Message { get; set; } = default!;
    public int TotalRecords { get; set; }
    public int SuccessRecords { get; set; }
    public int FailedRecords { get; set; }
    public DateTime UploadProcessStartedAt { get; set; }
    public DateTime UploadProcessEndedAt { get; set; }

    //Added for UI Only
    public bool ShowDetails { get; set; }
}
