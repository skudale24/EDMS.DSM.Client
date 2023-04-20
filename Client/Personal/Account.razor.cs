using System.Text.RegularExpressions;

namespace EDMS.DSM.Client.Personal;

public partial class Account
{
    private MudForm? form;
    private MudTextField<string>? pwField1;
    public string? AvatarImageLink { get; set; } = "images/avatar_jonny.jpg";
    public string? AvatarIcon { get; set; }

    public string AvatarButtonText { get; set; } = "Delete Picture";
    public Color AvatarButtonColor { get; set; } = Color.Error;
    public string FirstName { get; set; } = "Jonny";
    public string LastName { get; set; } = "Larsson";
    public string JobTitle { get; set; } = "IT Consultant";
    public string Email { get; set; } = "Youcanprobably@findout.com";
    public bool FriendSwitch { get; set; } = true;
    public bool NotificationEmail_1 { get; set; } = true;
    public bool NotificationEmail_2 { get; set; }
    public bool NotificationEmail_3 { get; set; }
    public bool NotificationEmail_4 { get; set; } = true;
    public bool NotificationChat_1 { get; set; }

    public bool NotificationChat_2 { get; set; } = true;
    public bool NotificationChat_3 { get; set; } = true;
    public bool NotificationChat_4 { get; set; }

    private void DeletePicture()
    {
        if (!string.IsNullOrEmpty(AvatarImageLink))
        {
            AvatarImageLink = null;
            AvatarIcon = Icons.Material.Outlined.SentimentVeryDissatisfied;
            AvatarButtonText = "Upload Picture";
            AvatarButtonColor = Color.Primary;
        }
    }

    private void SaveChanges(string message, Severity severity)
    {
        _ = Snackbar.Add(message, severity, config => { config.ShowCloseIcon = false; });
    }

    private IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }

        if (pw.Length < 8)
        {
            yield return "Password must be at least of length 8";
        }

        if (!Regex.IsMatch(pw, @"[A-Z]"))
        {
            yield return "Password must contain at least one capital letter";
        }

        if (!Regex.IsMatch(pw, @"[a-z]"))
        {
            yield return "Password must contain at least one lowercase letter";
        }

        if (!Regex.IsMatch(pw, @"[0-9]"))
        {
            yield return "Password must contain at least one digit";
        }
    }

    private string? PasswordMatch(string arg)
    {
        return pwField1.Value != arg ? "Passwords don't match" : null;
    }
}
