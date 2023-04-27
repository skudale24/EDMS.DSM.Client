namespace EDMS.DSM.Client.DTO;

public class ExWorksQuoteRequest
{
    public string SourcePincode { get; set; } = default!;
    public UnitWeight Weight { get; set; } = default!;
    public int? VolumeInCbm { get; set; }
}
