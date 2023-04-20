namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class Transportation : ComponentBase, IDisposable
{
    private IEnumerable<SearchTransportationCostDto> _searchTransportationCosts =
        new List<SearchTransportationCostDto>();

    private MudTable<SearchTransportationCostDto> _tableRef = default!;
    public bool isExpanded = true;

    [Inject] private ITransportationCostManager _transportationCostManager { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    private TransportationCostFilter _transportationCostFilter { get; set; } = new();

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
    }

    /// <summary>
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private async Task<TableData<SearchTransportationCostDto>> ServerReload(TableState state)
    {
        _transportationCostFilter.PageSize = state.PageSize;
        _transportationCostFilter.PageNumber = state.Page + 1;
        var result = await _transportationCostManager.Search(_transportationCostFilter).ConfigureAwait(false);

        _searchTransportationCosts = result.Result;

        return new TableData<SearchTransportationCostDto> { TotalItems = result.TotalRecords, Items = result.Result };
    }

    public int? GetRowNumber(object element)
    {
        return ((_transportationCostFilter.PageNumber - 1) * _transportationCostFilter.PageSize) +
               _searchTransportationCosts?.TakeWhile(x => x != element).Count() + 1;
    }

    /// <summary>
    ///     Set the value of 'ShowDetails' to exapand and collaps detail view in table.
    /// </summary>
    /// <param name="id"></param>
    private void ShowBtnPress(int id)
    {
        var searchTransportationCost = _searchTransportationCosts.FirstOrDefault(f => f.Id == id);
        searchTransportationCost.ShowDetails = !searchTransportationCost.ShowDetails;
    }

    /// <summary>
    ///     Reset or clear the filter values.
    /// </summary>
    private void ResetFilters()
    {
        _transportationCostFilter = new TransportationCostFilter();
        _ = _tableRef.ReloadServerData();
    }

    private void PageChanged(int i)
    {
        _tableRef.NavigateTo(i - 1);
    }

    /// <summary>
    /// </summary>
    private void OnSearch()
    {
        _ = _tableRef.ReloadServerData();
        isExpanded = false;
    }

    private async Task UploadCSV()
    {
        DialogParameters parameters = new()
        {
            ["Title"] = "Upload Transportations", ["UploadFor"] = UploadFileTypes.transportation.ToString()
        };

        var dialog = _dialogService.Show<Upload>("Title", parameters);

        var result = await dialog.Result.ConfigureAwait(false);

        if (!result.Canceled)
        {
            await _tableRef.ReloadServerData().ConfigureAwait(false);
        }
    }
}
