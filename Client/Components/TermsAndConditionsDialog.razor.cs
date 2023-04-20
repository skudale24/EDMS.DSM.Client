namespace EDMS.DSM.Client.Components;

public partial class TermsAndConditionsDialog
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    public string TermsMarkUpString { get; set; } = default!;

    [Parameter] public string Type { get; set; } = default!;

    [Inject] private ITermsManager _termsManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var terms = await _termsManager.GetTermsAndConditions().ConfigureAwait(false);
        TermsMarkUpString = terms.Result.FirstOrDefault(F => F.Key == Type)?.Terms;
    }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
