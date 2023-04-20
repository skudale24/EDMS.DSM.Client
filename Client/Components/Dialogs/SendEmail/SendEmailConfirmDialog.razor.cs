namespace EDMS.DSM.Client.Components.Dialogs.SendEmail;

public partial class SendEmailConfirmDialog : ComponentBase
{
    private readonly SendEmailModel _sendEmailModel = new();

    public SendEmailConfirmDialog(MudDialogInstance mudDialog)
    {
        MudDialog = mudDialog;
    }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }


    [Parameter] public string Type { get; set; } = default!;

    private void Cancel()
    {
        MudDialog.Cancel();
    }


    private async void Submit()
    {
        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);

        StateHasChanged();
        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
    }
}
