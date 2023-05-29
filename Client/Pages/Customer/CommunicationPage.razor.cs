using EDMS.DSM.Client.Managers.Menu;
using EDMS.DSM.Managers.Customer;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace EDMS.DSM.Client.Pages.Customer;

public partial class CommunicationPage : ComponentBase, IDisposable
{
    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IUploadManager _uploadManager { get; set; } = default!;

    [Inject] private ICustomerManager _customerManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    [Inject] IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    [Inject] private NavigationManager _navManager { get; set; } = default!;

    [Inject] public INavManager NavManager { get; set; } = default!;

    [Inject] private CookieStorageAccessor _cookieStorageAccessor { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    private IEnumerable<CommunicationDTO> Elements = new List<CommunicationDTO>();
    private MudDataGrid<CommunicationDTO> _grid;
    protected List<GridColumnDTO> GridColumns { get; set; } = default!;
    private bool isLoading = false;
    private string _searchString;
    private bool _sortNameByLength;
    private List<string> _events = new();

    private int _programId;
    private int _generatedById;
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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        //_interceptor.RegisterEvent();
    }

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        if (HttpContextAccessor.HttpContext != null)
    //        {
    //            var headers = HttpContextAccessor.HttpContext.Request.Headers;
    //            //var authorizationHeader = headers["Authorization"];
    //            var accessTokenHeader = headers["AccessToken"];
    //            // Use headers as needed
    //        }
    //    }
    //}

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (_navManager.TryGetQueryString(StorageConstants.UserToken, out string userTokenOut))
            {
                if (!string.IsNullOrWhiteSpace(userTokenOut))
                {
                    await _cookieStorageAccessor.WriteLogAsync<string>($"UserToken. {userTokenOut}");

                    var claims = GetClaimsFromToken(userTokenOut);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "UserID")?.Value, out _generatedById);
                    int.TryParse(claims.FirstOrDefault(c => c.Type == "ProgramID")?.Value, out _programId);

                    // Use retrieved `userToken` to update authentication state.
                    await _authStateProvider.UpdateAuthenticationStateAsync(userTokenOut, userTokenOut)
                        .ConfigureAwait(false);
                    await GetCommunicationsList();
                    GridColumns = GenerateGridColumns();
                    return;
                }
            }

            _navManager.NavigateTo($"{EndPoints.APBaseUrl}/Index.aspx");

            //else
            //{
            //    if (HttpContextAccessor.HttpContext != null)
            //    {
            //        var headers = HttpContextAccessor.HttpContext.Request.Headers;

            //        if (headers != null)
            //        {
            //            var accessTokenHeader = HttpContextAccessor.HttpContext.Request.Headers[StorageConstants.UserToken];
            //            //if (accessTokenHeader == null)
            //            //{
            //            //    return;
            //            //}
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            _ = _snackbar.Add($"We are currently unable to display the Customer Communications at this time. \r\n {ex.Message} : {ex.StackTrace}", Severity.Warning);
        }
    }

    /// <summary>
    /// Gets all Customer Communications
    /// </summary>
    /// <returns></returns>
    private async Task GetCommunicationsList()
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
            // Disable the button
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

                // Enable the button again
                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();

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
            }
            else
            {
                // Enable the button again
                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();

                _ = _snackbar.Add($"We are currently unable to generate the Communication Letter requested by you at this time. <br> \r\n {response?.Message}", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            // Enable the button again
            item.IsButtonDisabled = false;
            item.IsProcessing = false;
            StateHasChanged();

            _ = _snackbar.Add($"We are currently unable to generate the Communication Letter requested by you at this time. {ex.Message} : {ex.StackTrace}", Severity.Warning);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

    /// <summary>
    /// Export Excel file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task DownloadExcel(CommunicationDTO item)
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {

            var stream = await _uploadManager.DownloadExcelFileAsync<CommunicationDTO>(item);

            var fileName = $"CustomerList_{item.TemplateName}_{item.CompanyName}_{DateTime.Now.ToString("MMddyy")}.xlsx";
            fileName = fileName.Replace(": ", "_").Replace(" ", "_").Replace("-", "");
            using DotNetStreamReference streamRef = new(stream);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (Exception ex)
        {
            //_ = _snackbar.Add("We are facing some issues generating the excel file. Please try again later.", Severity.Warning);
            await _loadingIndicatorProvider.ReleaseAsync();
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
        catch (Exception ex)
        {
            _ = _snackbar.Add($"We are currently unable to get the file requested by you. \r\n {ex.Message}", Severity.Warning);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

    protected async Task ExportGridClicked()
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {

            IEnumerable<CommunicationDTO> data = _grid.FilteredItems;

            var result = await _uploadManager.ExportGridAsync(data, GridColumns);

            using DotNetStreamReference streamRef = new(result);
            var fileName = "Tools: Customer Communications.xlsx";
            fileName = fileName.Replace(": ", "_").Replace(" ", "_").Replace("-", "");
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (Exception ex)
        {
            await _loadingIndicatorProvider.ReleaseAsync();
            _ = _snackbar.Add($"We are currently unable to export the grid as requested by you. \r\n {ex.Message}", Severity.Warning);
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
        //_interceptor.DisposeEvent();
    }

    private List<Claim> GetClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Claims.ToList();
    }
}
