namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class ExworkRate : ComponentBase, IDisposable
{
    private IEnumerable<SearchExworksRateDto> _exworksSearches = new List<SearchExworksRateDto>();

    private MudTable<SearchExworksRateDto> _tableRef = default!;
    public bool isExpanded = true;

    [Inject] private IExworksRateManager _exworksRateManager { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    private ExworksRateFilter _exworksRateFilter { get; set; } = new();

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
    }

    private async Task<TableData<SearchExworksRateDto>> ServerReload(TableState state)
    {
        _exworksRateFilter.PageSize = state.PageSize;
        _exworksRateFilter.PageNumber = state.Page + 1;
        var result = await _exworksRateManager.Search(_exworksRateFilter).ConfigureAwait(false);
        _exworksSearches = result.Result;
        return new TableData<SearchExworksRateDto> { TotalItems = result.TotalRecords, Items = result.Result };
    }

    public int? GetRowNumber(object element)
    {
        return ((_exworksRateFilter.PageNumber - 1) * _exworksRateFilter.PageSize) +
               _exworksSearches?.TakeWhile(x => x != element).Count() + 1;
    }

    private void OnSearch()
    {
        _ = _tableRef.ReloadServerData();
        isExpanded = false;
    }

    private void ResetFilters()
    {
        _exworksRateFilter = new ExworksRateFilter();
        _ = _tableRef.ReloadServerData();
    }

    private void PageChanged(int i)
    {
        _tableRef.NavigateTo(i - 1);
    }

    private async Task UploadCSV()
    {
        DialogParameters parameters = new()
        {
            ["Title"] = "Upload Ex-Works", ["UploadFor"] = UploadFileTypes.exworks.ToString()
        };

        var dialog = _dialogService.Show<Upload>("Title", parameters);

        var result = await dialog.Result.ConfigureAwait(false);

        if (!result.Canceled)
        {
            await _tableRef.ReloadServerData().ConfigureAwait(false);
        }
    }
}
