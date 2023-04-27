namespace EDMS.DSM.Client.DTO;

public class SearchPincodeDto
{
    public int Id { get; set; }
    public string Pincode { get; set; } = string.Empty;
    public string Taluka { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
