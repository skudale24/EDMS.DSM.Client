namespace EDMS.DSM.Client.Dto;

public class QuoteData
{
    public string ToPincode { get; set; } = string.Empty;
    public string WeightType { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Cbm { get; set; } = string.Empty;
    public bool Agree { get; set; } = true;
    public string[] Locations { get; set; } = Array.Empty<string>();
    public string IsTransportationCost { get; set; } = string.Empty;
    public decimal ConversionRate { get; set; }
    public decimal ActualConversionRate { get; set; }
    public decimal ConversionRateInr { get; set; }
    public decimal ActualConversionRateInr { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public SplittedDataDetails SplittedDataDetails { get; set; } = new();
    public string ReferenceNumber { get; set; } = string.Empty;
}

public class SplittedDataDetails
{
    public decimal WeightInTon { get; set; }
    public decimal WeightRatio { get; set; }
    public decimal CbmRatio { get; set; }
    public decimal ApplicableRatio { get; set; }
    public decimal ApplicableWtRatio { get; set; }
    public decimal MarkupPer { get; set; }
    public string ApplicableSlabZone { get; set; } = string.Empty;
    public decimal ApplicableSlabZoneCharges { get; set; }
    public List<Via> Vias { get; set; } = new();
}

public class Via
{
    public string Name { get; set; } = string.Empty;
    public ParticularDetails ParticularDetails { get; set; } = new();
    public Charges Charges { get; set; } = new();
}

public class ParticularDetails
{
    public decimal TcByWeight { get; set; }
    public decimal TcByCbm { get; set; }
    public decimal DefaultCharges { get; set; }
    public decimal TcMarkup { get; set; }
    public decimal TcWithoutMarkUp { get; set; }
    public decimal Tc => TcWithoutMarkUp + DefaultCharges;
    public decimal FinalTc { get; set; }
    public decimal CfsByWeight { get; set; }
    public decimal CfsByCbm { get; set; }
    public decimal CfsCharges { get; set; }
    public decimal PlcByWt { get; set; }
    public decimal PlcByCbm { get; set; }
    public decimal FinalPlc { get; set; }
}

public class Charges
{
    public ChargesDetails TransportationCost { get; set; } = new();
    public ChargesDetails CustomClearance { get; set; } = new();
    public ChargesDetails FobPerWm { get; set; } = new();
    public ChargesDetails FobPerBl { get; set; } = new();
    public ChargesDetails DocumentationPerBl { get; set; } = new();
    public ChargesDetails SolasAdmin { get; set; } = new();
    public ChargesDetails SolasWm { get; set; } = new();
    public ChargesDetails GstData { get; set; } = new();
    public ChargesDetails SubTotal { get; set; } = new();
    public ChargesDetails Total { get; set; } = new();
    public ChargesDetails PlcPerWm { get; set; } = new();
    public float Gst { get; set; }
}

public class ChargesDetails
{
    public string Rate { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string Rupee { get; set; } = string.Empty;
    public string Dollar { get; set; } = string.Empty;
}
