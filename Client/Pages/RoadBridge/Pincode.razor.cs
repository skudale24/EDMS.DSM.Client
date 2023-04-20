namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class Pincode : ComponentBase, IDisposable
{
    private IEnumerable<SearchPincodeDto> _pinSearches = new List<SearchPincodeDto>();

    private MudTable<SearchPincodeDto> _tableRef = default!;

    public bool isExpanded = true;

    [Inject] private IPincodeManager _pincodeManager { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    private PincodeFilter _pincodeFilter { get; set; } = new();

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
    }

    private async Task<TableData<SearchPincodeDto>> ServerReload(TableState state)
    {
        _pincodeFilter.PageSize = state.PageSize;
        _pincodeFilter.PageNumber = state.Page + 1;
        var result = await _pincodeManager.Search(_pincodeFilter).ConfigureAwait(false);
        _pinSearches = result.Result;
        return new TableData<SearchPincodeDto> { TotalItems = result.TotalRecords, Items = result.Result };
    }

    public int? GetRowNumber(object element)
    {
        return ((_pincodeFilter.PageNumber - 1) * _pincodeFilter.PageSize) +
               _pinSearches?.TakeWhile(x => x != element).Count() + 1;
    }

    private void OnSearch()
    {
        _ = _tableRef.ReloadServerData();
        isExpanded = false;
    }

    private void ResetFilters()
    {
        _pincodeFilter = new PincodeFilter();
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
            ["Title"] = "Upload Pincodes", ["UploadFor"] = UploadFileTypes.pincodes.ToString()
        };

        var dialog = _dialogService.Show<Upload>("Title", parameters);

        var result = await dialog.Result.ConfigureAwait(false);

        if (!result.Canceled)
        {
            await _tableRef.ReloadServerData().ConfigureAwait(false);
        }
    }
}
