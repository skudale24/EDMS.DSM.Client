using EDMS.DSM.Client.Validators;
using System.Text.Json;

namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class ExWorksGetQuote : ComponentBase, IDisposable
{
    private bool _enabledManualOption;
    private ExWorksQuoteRequestFluentValidator _exWorksQuoteRequestValidator = default!;

    private MudForm? _form;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [Inject] private IQuoteManager _quoteManager { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;


    [Inject] private ILookupManager _lookupManager { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    private QuoteRequest _exWorksQuoteRequest { get; set; } = new();
    private QuoteRequest _exWorksInputData { get; set; } = new();
    private QuoteData _quoteExworksData { get; set; } = new();
    private QuoteResponse _exWorksQuoteResult { get; set; } = default!;
    private SubmitQuoteRequestDto _exWorksQuoteSubmit { get; } = new();
    private bool _isValid { get; set; }
    private bool _showResult { get; set; }
    private bool _showFinalResult { get; set; }

    private bool _isPanelOpen { get; set; }
    private string LoggedUserEmail { get; set; } = string.Empty;
    public bool Terms_Exworks { get; set; } = true;

    public bool EnabledManualOption
    {
        get => _enabledManualOption;
        set
        {
            _enabledManualOption = value;
            HandleManualOption(value);
        }
    }

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }


    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
        //_exWorksQuoteRequest = _quoteState.Value.QuoteRequest;
        _exWorksQuoteRequest = new QuoteRequest();
        _exWorksQuoteRequest.ValidationEnabled = !_enabledManualOption;
        _exWorksQuoteRequestValidator = new ExWorksQuoteRequestFluentValidator();
        StateHasChanged();
    }

    private async Task GetQuote()
    {
        await _form.Validate().ConfigureAwait(false);

        if (!_form.IsValid)
        {
            return;
        }

        var validpincode =
            await _lookupManager.ValidatePincodeAsync(_exWorksQuoteRequest.ToPincode).ConfigureAwait(false);
        if (!validpincode)
        {
            _ = _snackbar.Add("Pin code not mapped in masters, kindly connect RoadBridge team!", Severity.Error);
            await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
            return;
        }

        var userdata = (await _quoteManager.GetUserData().ConfigureAwait(false)).Result;
        if (userdata != null)
        {
            LoggedUserEmail = userdata.EmailAddress;
        }

        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);
        _exWorksQuoteRequest.WeightType =
            _exWorksQuoteRequest.IsWeightInLBs ? WeightType.Lbs.ToString() : WeightType.Kgs.ToString();
        _exWorksQuoteRequest.Type = QuoteType.EXWORK.ToString().ToLower();

        var res = await _quoteManager.GetQuote<QuoteRequest, QuoteResponse>(_exWorksQuoteRequest).ConfigureAwait(false);
        _exWorksQuoteResult = res.Result;

        if (_exWorksQuoteResult is null)
        {
            return;
        }

        if (_exWorksQuoteResult.Input != null && !string.IsNullOrWhiteSpace(_exWorksQuoteResult.Input))
        {
            _exWorksInputData = JsonSerializer.Deserialize<QuoteRequest>(_exWorksQuoteResult.Input);
        }

        if (_exWorksQuoteResult.Data != null && !string.IsNullOrWhiteSpace(_exWorksQuoteResult.Data))
        {
            _quoteExworksData = JsonSerializer.Deserialize<QuoteData>(_exWorksQuoteResult.Data);

            if (_quoteExworksData.SplittedDataDetails != null &&
                _quoteExworksData.SplittedDataDetails.Vias.Count() == 0)
            {
                _ = _snackbar.Add("Pin code not mapped with any data, kindly connect RoadBridge team!", Severity.Error);
                await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
                return;
            }
        }

        _showResult = res.Status;
        _isPanelOpen = true;
        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
        _navigationManager.NavigateTo("/ExWorks/QuoteResult");
    }

    private async Task Submit(int i)
    {
        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);
        _exWorksQuoteSubmit.QuoteId = _exWorksQuoteResult.Id;
        if (_quoteExworksData.SplittedDataDetails != null && _quoteExworksData.SplittedDataDetails.Vias.Any())
        {
            _exWorksQuoteSubmit.Routing = _quoteExworksData.SplittedDataDetails.Vias[0].Name;
        }

        if (_exWorksQuoteSubmit.BookNow || _exWorksQuoteSubmit.MailMe)
        {
            var result = await _quoteManager.QuoteBookMail(_exWorksQuoteSubmit).ConfigureAwait(false);

            if (result.Status)
            {
                _ = _snackbar.Add(result.Message, Severity.Success);
                StateHasChanged();
            }
        }

        if (_exWorksQuoteSubmit.Print)
        {
            var stream = await _quoteManager.PrintQuote(_exWorksQuoteSubmit).ConfigureAwait(false);
            MemoryStream ms = new();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            if (i == 1)
            {
                await _jsRuntime.InvokeVoidAsync("OpenFileAsPDF", ms.GetBuffer(), $"{_exWorksQuoteResult.Id}.pdf")
                    .ConfigureAwait(false);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("SaveFileAsPDF", ms.GetBuffer(), $"{_exWorksQuoteResult.Id}.pdf")
                    .ConfigureAwait(false);
            }
        }

        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
        //NewRequest();
    }

    private async Task OpenModal()
    {
        try
        {
            if (!_exWorksQuoteSubmit.MailMe)
            {
                DialogParameters parameters = new() { ["UserEmail"] = LoggedUserEmail };

                DialogOptions options = new()
                {
                    CloseButton = true,
                    MaxWidth = MaxWidth.Small,
                    Position = DialogPosition.TopCenter,
                    DisableBackdropClick = true,
                    CloseOnEscapeKey = false
                };
                var dialog = _dialogService.Show<MailMe>("Mail Now", parameters, options);
                var result1 = await dialog.Result.ConfigureAwait(false);

                if (!result1.Canceled)
                {
                    _exWorksQuoteSubmit.MailMeAddress = result1.Data.ToString();
                }
            }
        }
        catch (Exception)
        {
            _ = _snackbar.Add("Something went wrong!", Severity.Error);
        }
    }

    private void NewRequest()
    {
        _showResult = false;
        EnabledManualOption = false;
        _exWorksQuoteRequest = new QuoteRequest();
        _navigationManager.NavigateTo("/ExWorks/GetQuote");
    }

    private void ShowTermsAndConditions()
    {
        DialogParameters parameters = new() { ["Type"] = "ExWorks" };

        DialogOptions options = new()
        {
            CloseButton = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.TopCenter,
            DisableBackdropClick = true,
            CloseOnEscapeKey = false
        };
        _ = _dialogService.Show<TermsAndConditionsDialog>("Ex-Works Terms & Conditions", parameters, options);
    }

    private void HandleManualOption(bool enableManualOption)
    {
        _exWorksQuoteRequest.ValidationEnabled = !enableManualOption;
    }
}
