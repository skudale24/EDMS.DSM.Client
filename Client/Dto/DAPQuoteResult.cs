namespace EDMS.DSM.Client.DTO;

public class DAPQuoteResult
{
    public int QuoteId { get; set; }
    public string Weight { get; set; } = default!;
    public int VolumeInCbm { get; set; }
    public string Pincode { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string State { get; set; } = default!;
    public List<DAPQuoteVia> Vias { get; set; } = default!;
    public DateTime ExchangeRateAsOn { get; set; }
    public double ExchangeRate { get; set; }
}

public class DAPQuoteVia
{
    public string Name { get; set; } = default!;
    public int SubTotal { get; set; }
    public int Gst { get; set; }
    public string GstTax { get; set; } = default!;
    public int TotalInINR { get; set; }
    public double TotalInUSD { get; set; }
}
