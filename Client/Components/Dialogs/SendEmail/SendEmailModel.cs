using System.ComponentModel.DataAnnotations;

namespace EDMS.DSM.Client.Components.Dialogs.SendEmail;

public class SendEmailModel
{
    [Required(ErrorMessage = "The Name Filed is required.")]
    public string Name { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }
}

public class MarkupQuoteRequestDto
{
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string UserId { get; set; }
    public List<MarkupDetails> Markup { get; set; } = new();
}

public class MarkupDetails
{
    public int Id { get; set; }
    public int Markup { get; set; }
}

#region FCL

public class MarkupItem
{
    public int _20 { get; set; } = 0;

    public int _40 { get; set; } = 0;

    public int _40Hc { get; set; } = 0;
}

public class FCLMarkupData
{
    public int Id { get; set; }
    public MarkupItem ItemMarkup { get; set; }
}

public class MarkupFCLRequestDto
{
    public string Type { get; set; }
    public List<FCLMarkupData> NewMarkup { get; set; }
    public string Email { get; set; }
    public string CustomerName { get; set; }
    public string UserId { get; set; }
}

#endregion
