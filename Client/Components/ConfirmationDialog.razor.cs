namespace EDMS.DSM.Client.Components;

public partial class ConfirmationDialog
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string ConfirmationMessage { get; set; } = string.Empty;

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
