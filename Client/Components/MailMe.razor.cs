namespace EDMS.DSM.Client.Components;

public partial class MailMe : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string UserEmail { get; set; } = string.Empty;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    private void Submit()
    {
        if (string.IsNullOrEmpty(UserEmail))
        {
            _ = _snackbar.Add("LoggedUserEmail is required!", Severity.Error);
            return;
        }

        MudDialog.Close(DialogResult.Ok(UserEmail));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
