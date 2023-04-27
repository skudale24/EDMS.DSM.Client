namespace EDMS.DSM.Client.DTO;

public class QuoteSearchDetailDto
{
    public QuoteResponse SearchHistory { get; set; } = new();
    public List<CommentDetails> Comments { get; set; } = new();
}

public class CommentDetails
{
    public string CreatedBy { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
