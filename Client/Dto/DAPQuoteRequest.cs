namespace EDMS.DSM.Client.Dto;

public class DAPQuoteRequest
{
    public string Type { get; set; } = default!;
    public string FinalDestinationPincode { get; set; } = default!;
    public UnitWeight Weight { get; set; } = default!;
    public int? VolumeInCbm { get; set; } = default!;
}

public class UnitWeight
{
    public int Weight { get; set; }
    public string Unit { get; set; } = default!;
}
