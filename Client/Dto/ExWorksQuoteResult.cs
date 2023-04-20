namespace EDMS.DSM.Client.Dto;

public class ExWorksQuoteResult
{
    public int QuoteId { get; set; }
    public string Weight { get; set; } = default!;
    public int VolumeInCbm { get; set; }
    public string Pincode { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string State { get; set; } = default!;
    public List<ExWorksVia> Vias { get; set; } = default!;
    public DateTime ExchangeRateAsOn { get; set; }
    public double ExchangeRate { get; set; }
}

public class ExWorksVia
{
    public string Name { get; set; } = default!;
    public string GstTax { get; set; } = default!;
    public int TotalInINR { get; set; }
    public int RotalInUSD { get; set; }
    public int TotalINRWithGst { get; set; }
}
