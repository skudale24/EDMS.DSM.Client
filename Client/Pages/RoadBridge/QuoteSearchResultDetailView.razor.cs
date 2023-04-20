using System.Text.Json;

namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class QuoteSearchResultDetailView : ComponentBase, IDisposable
{
    private QuoteSearchDetailDto _quoteSearchDetailDto = new();

    [Parameter] public int Id { get; set; }

    [Parameter] public string Type { get; set; } = string.Empty;

    [Inject] private IQuoteSearchResultManager _quoteSearchResultManager { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    private QuoteData _quoteData { get; set; } = new();
    private QuoteRequest _quoteInputData { get; set; } = new();

    private bool _isPanelOpen { get; } = true;
    private bool _isMarkAsExecuted { get; set; }
    private bool _isMarkAsClosed { get; set; }
    private string _comment { get; set; } = string.Empty;

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _interceptor.RegisterEvent();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync().ConfigureAwait(false);
        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);
        var result = await _quoteSearchResultManager.Search(Id).ConfigureAwait(false);
        _quoteSearchDetailDto = result.Result;

        if (_quoteSearchDetailDto != null && _quoteSearchDetailDto.SearchHistory != null &&
            !string.IsNullOrWhiteSpace(_quoteSearchDetailDto.SearchHistory.Input))
        {
            if (_quoteSearchDetailDto.SearchHistory != null &&
                _quoteSearchDetailDto.SearchHistory.Status == QuoteStatus.Closed.ToString())
            {
                _isMarkAsClosed = true;
            }

            if (_quoteSearchDetailDto.SearchHistory != null &&
                _quoteSearchDetailDto.SearchHistory.Status == QuoteStatus.Executed.ToString())
            {
                _isMarkAsExecuted = true;
                _isMarkAsClosed = true;
            }


            _quoteInputData = JsonSerializer.Deserialize<QuoteRequest>(_quoteSearchDetailDto.SearchHistory.Input);
        }

        if (_quoteSearchDetailDto != null && _quoteSearchDetailDto.SearchHistory != null &&
            !string.IsNullOrWhiteSpace(_quoteSearchDetailDto.SearchHistory.Data))
        {
            _quoteData = JsonSerializer.Deserialize<QuoteData>(_quoteSearchDetailDto.SearchHistory.Data);
        }

        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
    }

    private async void SubmitComment()
    {
        if (!string.IsNullOrEmpty(_comment))
        {
            var result = await _quoteSearchResultManager.AddComments(Id, _comment).ConfigureAwait(false);

            if (result.Status)
            {
                _comment = string.Empty;
                _ = _snackbar.Add("Comment added successfully!", Severity.Success);
                StateHasChanged();

                await OnParametersSetAsync().ConfigureAwait(false);
                await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
            }
        }
    }

    private async void MarkAsExecuted()
    {
        var result = await _quoteSearchResultManager.MarkAsExecuted(Id).ConfigureAwait(false);
        if (result.Status)
        {
            _isMarkAsExecuted = true;
            _ = _snackbar.Add("Marked as executed successfully!", Severity.Success);
            StateHasChanged();
        }
    }

    private async void MarkAsClosed()
    {
        var result = await _quoteSearchResultManager.MarkAsClosed(Id).ConfigureAwait(false);
        if (result.Status)
        {
            _isMarkAsClosed = true;
            _ = _snackbar.Add("Marked as closed successfully!", Severity.Success);
            StateHasChanged();
        }
    }

    private async Task DownloadPdf()
    {
        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);

        var stream = await _quoteSearchResultManager.DownloadPdf(Id).ConfigureAwait(false);
        MemoryStream ms = new();
        await stream.CopyToAsync(ms).ConfigureAwait(false);

        await _jsRuntime.InvokeVoidAsync("SaveFileAsPDF", ms.GetBuffer(), $"{_quoteData.ReferenceNumber}.pdf")
            .ConfigureAwait(false);

        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
    }
}
