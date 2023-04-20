namespace EDMS.DSM.Client.Components;

partial class VersionAlertDialog
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string LatestVersion { get; set; } = default!;


    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }
}
