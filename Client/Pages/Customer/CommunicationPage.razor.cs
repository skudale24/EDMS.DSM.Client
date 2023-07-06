using EDMS.DSM.Managers.Customer;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace EDMS.DSM.Client.Pages.Customer;

public partial class CommunicationPage : ComponentBase, IDisposable
{
    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IUploadManager _uploadManager { get; set; } = default!;

    [Inject] private ICustomerManager _customerManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    [Inject] private NavigationManager _navManager { get; set; } = default!;

    private IEnumerable<CommunicationDTO> Elements = new List<CommunicationDTO>();
    private MudDataGrid<CommunicationDTO> _grid;
    protected List<GridColumnDTO> GridColumns { get; set; } = default!;
    private bool isLoading = false;
    private string _searchString;
    private bool _sortNameByLength;
    private List<string> _events = new();

    private int _programId;
    private int _generatedById;
    string APRedirectUrl = $"{EndPoints.APBaseUrl}/Index.aspx";

    // custom sort by name length
    private Func<CommunicationDTO, object> _sortBy => x =>
    {
        if (_sortNameByLength)
            return x.TemplateName.Length;
        else
            return x.TemplateName;
    };

    // quick filter - filter gobally across multiple columns with the same input
    private Func<CommunicationDTO, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.TemplateName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (x.GeneratedDate?.ToString("MM/dd/yyyy").Contains(_searchString) == true)
            return true;

        if (x.CountofApplications.ToString().Contains(_searchString) == true)
            return true;

        if (x.ActionText?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (x.CompanyName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (x.GeneratedBy?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.PWAUpdaterService.NextVersionIsWaiting += PWAUpdaterService_NextVersionIsWaiting;
            //_interceptor.RegisterEvent();
        }
    }

    private async void PWAUpdaterService_NextVersionIsWaiting(object? sender, EventArgs e)
    {
        await this.PWAUpdaterService.SkipWaitingAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();
        _interceptor.RegisterEvent();

        try
        {
            if (_navManager.TryGetQueryString(StorageConstants.UserId, out _generatedById)
                && (_navManager.TryGetQueryString(StorageConstants.ProgramId, out _programId)))
            {
                await GetCommunicationsList();
                GridColumns = GenerateGridColumns();
            }
            else
            {
                await SetTopFrameUrl(APRedirectUrl);
                //_navManager.NavigateTo($"{EndPoints.APBaseUrl}/Index.aspx");
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            await HandleException("We are currently unable to display the Customer Communications at this time.");
        }
    }

    /// <summary>
    /// Gets all Customer Communications
    /// </summary>
    /// <returns></returns>
    private async Task GetCommunicationsList()
    {
        try
        {
            isLoading = true;
            StateHasChanged();

            var result = await _customerManager.GetCommunicationsListAsync();

            _ = result.Status
                ? _snackbar.Add(result.Message, Severity.Success)
                : _snackbar.Add(result.Message, Severity.Error);

            Elements = result.Result.ToCommunicationGrid();
            isLoading = false;
            StateHasChanged();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            await HandleException("We are currently unable to display the Customer Communications at this time.");
        }
    }

    private List<GridColumnDTO> GenerateGridColumns()
    {
        List<GridColumnDTO> columns = new List<GridColumnDTO>
        {
            new GridColumnDTO("TemplateName", "Letter Type", "TemplateName", true),
            new GridColumnDTO("CompanyName", "Utility", "CompanyName", true),
            new GridColumnDTO("CountofApplications", "Application Count", "CountofApplications", true),
            new GridColumnDTO("ActionText", "Action", "ActionText"),
            new GridColumnDTO("GeneratedDate", "Letter Generated Date", "GeneratedDate", true),
            new GridColumnDTO("GeneratedBy", "Letter Generated by", "GeneratedBy", true)
        };
        return columns;
    }

    /// <summary>
    /// Generate Letters functionality
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task ProcessLetterGeneration(CommunicationDTO item)
    {
        try
        {
            item.IsProcessing = true;
            item.IsButtonDisabled = true;
            StateHasChanged();

            GenerateLetterDTO model = new();
            model.LPCID = Convert.ToInt32(item.LPCID);
            model.TemplateFile = item.FilePath;
            model.TemplateID = item.TemplateId;
            model.TemplateType = item.TemplateType;
            model.ProgramId = _programId;
            model.GeneratedBy = _generatedById;
            var result = await _customerManager.GenerateLetter<GenerateLetterDTO, GenerateLetterDTO>(model);
            ApiResult<GenerateLetterDTO> response = result as ApiResult<GenerateLetterDTO>;

            if (response?.Status == true)
            {
                var items = await _customerManager.GetCommunicationsListAsync();
                var cRow = items.Result.Where(f => f.BatchId == response.Result.BatchId).FirstOrDefault();
                item.GeneratedBy = cRow?.GeneratedBy;
                item.GeneratedDate = cRow?.GeneratedDate?.Date ?? null;
                item.ActionText = "Download PDF";
                item.GeneratedFilePath = response.Result.GeneratedFilePath;
                item.BatchId = response.Result.BatchId;

                //TODO: get letter type from backend
                var letterType = item.FilePath?.Split("__").Skip(1).FirstOrDefault();
                //string letterType = item.TemplateType; Enum.GetName(typeof(ETemplateType), TemplateType);
                var downloadResult = await _uploadManager.DownloadSourceFileAsync(response.Result.GeneratedFilePath);

                await _loadingIndicatorProvider.HoldAsync();
                MemoryStream ms = new();
                await downloadResult.CopyToAsync(ms);
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC{letterType}";
                ms.Position = 0;
                await _jsRuntime.InvokeVoidAsync("OpenFileAsPDF", ms.GetBuffer(), fileName);
                await _loadingIndicatorProvider.ReleaseAsync();
                StateHasChanged();

                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();
            }
            else
            {
                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();
                await HandleException("We are currently unable to generate the Communication Letter requested by you.");
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            item.IsButtonDisabled = false;
            item.IsProcessing = false;
            StateHasChanged();
            await HandleException("We are currently unable to generate the Communication Letter requested by you.");
        }
    }

    /// <summary>
    /// Export Excel file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task DownloadExcel(CommunicationDTO item)
    {
        try
        {
            await _loadingIndicatorProvider.HoldAsync();
            var stream = await _uploadManager.DownloadExcelFileAsync<CommunicationDTO>(item);
            var fileName = $"CustomerList_{item.TemplateName}_{item.CompanyName}_{DateTime.Now.ToString("MMddyy")}.xlsx";
            fileName = fileName.Replace(": ", "_").Replace(" ", "_").Replace("-", "");
            using DotNetStreamReference streamRef = new(stream);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            await HandleException("We are facing some issues generating the excel file. Please try again later.");
        }
    }

    /// <summary>
    /// Download PDF directly
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task DownloadSourceFile(CommunicationDTO item)
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {
            var letterType = item.FilePath?.Split("__").Skip(1).FirstOrDefault();
            //string letterType = item.TemplateType; Enum.GetName(typeof(ETemplateType), TemplateType);
            var result = await _uploadManager.DownloadSourceFileAsync(item.GeneratedFilePath);
            using DotNetStreamReference streamRef = new(result);
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC{letterType}";
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            await HandleException("We are currently unable to get the file requested by you. Please try after some time.");
        }
    }

    protected async Task ExportGridClicked()
    {
        try
        {
            await _loadingIndicatorProvider.HoldAsync();
            var data = _grid.FilteredItems;
            var result = await _uploadManager.ExportGridAsync(data, GridColumns);
            using DotNetStreamReference streamRef = new(result);
            var fileName = "Tools: Customer Communications.xlsx";
            fileName = fileName.Replace(": ", "_").Replace(" ", "_").Replace("-", "");
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedException();
        }
        catch (Exception)
        {
            await HandleException("We are currently unable to export the grid as requested by you.");
        }
    }

    // events
    void RowClicked(DataGridRowClickEventArgs<CommunicationDTO> args)
    {
        _events.Insert(0, $"Event = RowClick, Index = {args.RowIndex}, Data = {System.Text.Json.JsonSerializer.Serialize(args.Item)}");
    }

    void SelectedItemsChanged(HashSet<CommunicationDTO> items)
    {
        _events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
    }

    void IDisposable.Dispose()
    {
        this.PWAUpdaterService.NextVersionIsWaiting -= PWAUpdaterService_NextVersionIsWaiting;
        _interceptor.DisposeEvent();
    }

    private async Task SetTopFrameUrl(string url)
    {
        await _jsRuntime.InvokeVoidAsync("setTopFrameUrl", url);
    }

    async Task HandleUnauthorizedException()
    {
        await _loadingIndicatorProvider.ReleaseAsync();
        await SetTopFrameUrl(APRedirectUrl);
    }

    async Task HandleException(string message)
    {
        await _loadingIndicatorProvider.ReleaseAsync();

        _navManager.NavigateTo($"/errorpage?{StorageConstants.UserId}={_generatedById}&{StorageConstants.ProgramId}={_programId}");

        //_ = _snackbar.Add(message, Severity.Warning);
    }

}
