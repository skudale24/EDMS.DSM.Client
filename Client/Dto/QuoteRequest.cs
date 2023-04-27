using System.Text.Json.Serialization;

namespace EDMS.DSM.Client.DTO;

public class QuoteRequest
{
    public string? Type { get; set; }
    public string? ToPincode { get; set; }
    public double Weight { get; set; }
    public string? WeightType { get; set; }
    public double Cbm { get; set; }
    [JsonIgnore] public bool IsWeightInLBs { get; set; }
    [JsonIgnore] public bool ValidationEnabled { get; set; }
}
