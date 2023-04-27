namespace EDMS.DSM.Client.DTO;

public class SubmitQuoteRequestDto
{
    public int QuoteId { get; set; }
    public string Routing { get; set; } = string.Empty;
    public bool BookNow { get; set; }
    public bool MailMe { get; set; }
    public bool Print { get; set; }
    public string MailMeAddress { get; set; } = "vaishali.l@iworktech.com";
}
