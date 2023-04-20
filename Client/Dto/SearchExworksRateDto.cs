namespace EDMS.DSM.Client.Dto;

public class SearchExworksRateDto
{
    public int Id { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int CustomClearance { get; set; }
    public int AcdAciEnsPerHbl { get; set; }
    public int IhcThcPerWm { get; set; }
    public int DocPerHbl { get; set; }
    public int OneTonsTwoCbm { get; set; }
    public int ThreeTonsSixCbm { get; set; }
    public int FiveTonsTenCbm { get; set; }
    public int SevenTonsFourteenCbm { get; set; }
    public DateTime UploadedOn { get; set; } = DateTime.MinValue;
    public string OriginCode { get; set; } = string.Empty;
    public string DestinationCode { get; set; } = string.Empty;
}
