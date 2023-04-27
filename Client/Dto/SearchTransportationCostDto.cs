namespace EDMS.DSM.Client.DTO;

public class SearchTransportationCostDto
{
    public int Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Pod { get; set; } = string.Empty;
    public string OriginState { get; set; } = string.Empty;
    public string OriginRegion { get; set; } = string.Empty;
    public string FromZipCode { get; set; } = string.Empty;
    public string ToZipCode { get; set; } = string.Empty;
    public DateTime UploadedOn { get; set; } = DateTime.MinValue;
    public string DestinationState { get; set; } = string.Empty;
    public string DestinationRegion { get; set; } = string.Empty;
    public string ApplicableSlabZone { get; set; } = string.Empty;
    public decimal CostPerKg { get; set; }
    public int OneTonTwoCbm { get; set; }
    public int ThreeTonsSixCbm { get; set; }
    public int FiveTonsTemCbm { get; set; }
    public int SevenTonsFourteenCbm { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public string PodCode { get; set; } = string.Empty;
    public string OriginStateCode { get; set; } = string.Empty;
    public string DestinationStateCode { get; set; } = string.Empty;

    //Added for UI
    public bool ShowDetails { get; set; }
}
