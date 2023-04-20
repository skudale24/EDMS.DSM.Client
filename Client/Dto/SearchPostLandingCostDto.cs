namespace EDMS.DSM.Client.Dto;

public class SearchPostLandingCostDto
{
    public int Id { get; set; }
    public string Destination { get; set; } = string.Empty;
    public int CustomClearanceCharges { get; set; }
    public int CfsChargesPerCbm { get; set; }
    public int PlcPerWm { get; set; }
    public int Transport { get; set; }
    public int Gst { get; set; }
    public DateTime UploadedOn { get; set; } = DateTime.MinValue;
}
