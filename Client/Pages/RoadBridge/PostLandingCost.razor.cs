namespace EDMS.DSM.Client.Pages.RoadBridge;

public partial class PostLandingCost : ComponentBase, IDisposable
{
    private IEnumerable<SearchPostLandingCostDto> _postLandingSearches = new List<SearchPostLandingCostDto>();

    private MudTable<SearchPostLandingCostDto> _tableRef = default!;

    public bool isExpanded = true;

    [Inject] private IPostLandingCostManager _postLandingCostManager { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    private PostLandingCostFilter _postLandingCostFilter { get; set; } = new();

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
    }

    private async Task<TableData<SearchPostLandingCostDto>> ServerReload(TableState state)
    {
        _postLandingCostFilter.PageSize = state.PageSize;
        _postLandingCostFilter.PageNumber = state.Page + 1;
        var result = await _postLandingCostManager.Search(_postLandingCostFilter).ConfigureAwait(false);
        _postLandingSearches = result.Result;
        return new TableData<SearchPostLandingCostDto> { TotalItems = result.TotalRecords, Items = result.Result };
    }

    public int? GetRowNumber(object element)
    {
        return ((_postLandingCostFilter.PageNumber - 1) * _postLandingCostFilter.PageSize) +
               _postLandingSearches?.TakeWhile(x => x != element).Count() + 1;
    }

    private void OnSearch()
    {
        _ = _tableRef.ReloadServerData();
        isExpanded = false;
    }

    private void ResetFilters()
    {
        _postLandingCostFilter = new PostLandingCostFilter();
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
            ["Title"] = "Upload Post Landing Cost", ["UploadFor"] = UploadFileTypes.postlanding.ToString()
        };

        var dialog = _dialogService.Show<Upload>("Title", parameters);

        var result = await dialog.Result.ConfigureAwait(false);

        if (!result.Canceled)
        {
            await _tableRef.ReloadServerData().ConfigureAwait(false);
        }
    }
}
