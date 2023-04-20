namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class TermsAndConditions : ComponentBase, IDisposable
{
    protected string contentAsDeltaJson = default!;

    protected string contentAsHtml = default!;
    protected string contentAsText = default!;
    private string? dapValue = string.Empty;
    private string? exWorksValue = string.Empty;
    protected string savedContent = default!;

    [Inject] private ITermsManager _termsManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    private IEnumerable<TermsDto> _terms { get; set; } = new List<TermsDto>();

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override async Task OnInitializedAsync()
    {
        _interceptor.RegisterEvent();

        _terms = (await _termsManager.GetTermsAndConditions().ConfigureAwait(false)).Result;

        dapValue = _terms.FirstOrDefault(F => F.Key == "DAP")?.Terms;
        exWorksValue = _terms.FirstOrDefault(F => F.Key == "ExWorks")?.Terms;
    }

    private void OnChangeDAP(string htmlString)
    {
        dapValue = htmlString;
    }

    private void OnChangeExWork(string htmlString)
    {
        exWorksValue = htmlString;
    }

    private async void OnSave()
    {
        List<TermsDto> terms = new()
        {
            new TermsDto { Key = TermsAndConditionsKey.DAP.ToString(), Terms = dapValue },
            new TermsDto { Key = TermsAndConditionsKey.ExWorks.ToString(), Terms = exWorksValue }
        };

        var res = await _termsManager.SaveTermsAndConditions(terms).ConfigureAwait(false);

        _ = res.Status
            ? _snackbar.Add("Terms And Conditions has been saved successfully!", Severity.Success)
            : _snackbar.Add("Error occured while saving Terms And Conditions.", Severity.Error);
    }
}
