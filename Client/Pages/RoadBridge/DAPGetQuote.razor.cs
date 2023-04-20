using EDMS.DSM.Client.Validators;
using System.Text.Json;
using Action = EDMS.DSM.Client.Store.UseCase.DapDduQuote.Action;

namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class DAPGetQuote : ComponentBase, IDisposable
{
    private DapQuoteRequestFluentValidator _dapQuoteRequestValidator = default!;
    private bool _enabledManualOption;

    private MudForm? _form;
    private bool _showFinalResult;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [Inject] private IQuoteManager _quoteManager { get; set; } = default!;

    [Inject] private ILookupManager _lookupManager { get; set; } = default!;


    [Inject] private IDialogService _dialogService { get; set; } = default!;


    [Inject] public IDispatcher _dispatcher { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    private QuoteRequest _quoteRequest { get; set; } = new();
    private QuoteRequest _quoteInputData { get; set; } = new();
    private QuoteData _quoteData { get; set; } = new();
    private QuoteResponse _dapQuoteResult { get; set; } = default!;
    private SubmitQuoteRequestDto _dapQuoteSubmit { get; } = new();
    private bool _isValid { get; set; }
    private bool _showResult { get; set; }
    private bool _isPanelOpen { get; set; } = true;
    private string LoggedUserEmail { get; set; } = string.Empty;
    public bool Terms_Dap { get; set; } = true;

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
        //_quoteState.StateChanged -= StateChanged;
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
        //_quoteRequest = _quoteState.Value.QuoteRequest;
        _quoteRequest = new QuoteRequest();
        _quoteRequest.ValidationEnabled = !_enabledManualOption;
        _dapQuoteRequestValidator = new DapQuoteRequestFluentValidator();
        StateHasChanged();
    }

    private async Task GetQuote()
    {
        await _form.Validate().ConfigureAwait(false);

        if (!_form.IsValid)
        {
            return;
        }

        var validpincode = await _lookupManager.ValidatePincodeAsync(_quoteRequest.ToPincode).ConfigureAwait(false);
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
        _quoteRequest.WeightType = _quoteRequest.IsWeightInLBs ? WeightType.Lbs.ToString() : WeightType.Kgs.ToString();
        Action action = new() { quoteRequest = _quoteRequest };
        _dispatcher.Dispatch(action);

        var res = await _quoteManager.GetQuote<QuoteRequest, QuoteResponse>(_quoteRequest).ConfigureAwait(false);
        _dapQuoteResult = res.Result;

        if (_dapQuoteResult is null)
        {
            return;
        }

        if (_dapQuoteResult.Input != null && !string.IsNullOrWhiteSpace(_dapQuoteResult.Input))
        {
            _quoteInputData = JsonSerializer.Deserialize<QuoteRequest>(_dapQuoteResult.Input);
        }

        if (_dapQuoteResult.Data != null && !string.IsNullOrWhiteSpace(_dapQuoteResult.Data))
        {
            _quoteData = JsonSerializer.Deserialize<QuoteData>(_dapQuoteResult.Data);

            if (_quoteData.SplittedDataDetails != null && _quoteData.SplittedDataDetails.Vias.Count() == 0)
            {
                _ = _snackbar.Add("Pin code not mapped with any data, kindly connect RoadBridge team!", Severity.Error);
                await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
                return;
            }
        }

        _showResult = res.Status;
        _isPanelOpen = true;
        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
        _navigationManager.NavigateTo("/dap/QuoteResult");
    }

    private async Task Submit(int i)
    {
        _dapQuoteSubmit.QuoteId = _dapQuoteResult.Id;
        if (_quoteData.SplittedDataDetails != null && _quoteData.SplittedDataDetails.Vias.Any())
        {
            _dapQuoteSubmit.Routing = _quoteData.SplittedDataDetails.Vias[0].Name;
        }

        if (_dapQuoteSubmit.BookNow || _dapQuoteSubmit.MailMe)
        {
            await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);

            var result = await _quoteManager.QuoteBookMail(_dapQuoteSubmit).ConfigureAwait(false);

            _ = result.Status
                ? _snackbar.Add(result.Message, Severity.Success)
                : _snackbar.Add(result.Message, Severity.Error);
            StateHasChanged();
        }

        if (_dapQuoteSubmit.Print)
        {
            var stream = await _quoteManager.PrintQuote(_dapQuoteSubmit).ConfigureAwait(false);
            MemoryStream ms = new();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            if (i == 1)
            {
                await _jsRuntime.InvokeVoidAsync("OpenFileAsPDF", ms.GetBuffer(), $"{_dapQuoteResult.Id}.pdf")
                    .ConfigureAwait(false);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("SaveFileAsPDF", ms.GetBuffer(), $"{_dapQuoteResult.Id}.pdf")
                    .ConfigureAwait(false);
            }

            StateHasChanged();
        }

        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
    }

    private async Task OpenModal()
    {
        try
        {
            if (!_dapQuoteSubmit.MailMe)
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
                    _dapQuoteSubmit.MailMeAddress = result1.Data.ToString()!;
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
        _quoteRequest = new QuoteRequest();
        _navigationManager.NavigateTo("/dap/GetQuote");
    }

    private void ShowTermsAndConditions()
    {
        DialogParameters parameters = new() { ["Type"] = "DAP" };

        DialogOptions options = new()
        {
            CloseButton = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.TopCenter,
            DisableBackdropClick = true,
            CloseOnEscapeKey = false
        };
        _ = _dialogService.Show<TermsAndConditionsDialog>("DAP Terms & Conditions", parameters, options);
    }

    // public void StateChanged(object sender, EventArgs args)
    // {
    //     _ = InvokeAsync(StateHasChanged);
    // }

    private void HandleManualOption(bool enableManualOption)
    {
        _quoteRequest.ValidationEnabled = !enableManualOption;
    }
}
