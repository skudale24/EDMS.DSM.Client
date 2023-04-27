namespace EDMS.DSM.Client.DTO;

public class QuoteResponse
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string? Type { get; set; }
    public string? Pincode { get; set; }
    public string? Input { get; set; }
    public string? Source { get; set; }
    public string? Data { get; set; }
    public string? UserId { get; set; }
    public bool IsEmail { get; set; }
    public bool IsBook { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Email { get; set; }
    public string? Pdf { get; set; }
    public string? EfreightNo { get; set; }
    public string? QuoteType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Status { get; set; }
    public string? Message { get; set; }
}
