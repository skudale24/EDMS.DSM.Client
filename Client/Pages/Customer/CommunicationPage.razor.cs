using EDM.Setting;
using EDMS.Data.Constants;
using EDMS.DSM.Managers.Customer;
using MudBlazor;
using System.Data;

namespace EDMS.DSM.Client.Pages.Customer;

public partial class CommunicationPage : ComponentBase, IDisposable
{
    //    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IUploadManager _uploadManager { get; set; } = default!;

    [Inject] private ICustomerManager _customerManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    //[Inject] private CookieStorageAccessor _cookieStorageAccessor { get; set; } = default!;

    private GenerateLetterDTO model { get; set; } = new();

    private IEnumerable<CommunicationDTO> Elements = new List<CommunicationDTO>();
    //private bool IsButtonDisabled = false;
    private bool isLoading = false;
    private string _searchString;
    private bool _sortNameByLength;
    private List<string> _events = new();

    //TODO: Replace hardcoded values with actual values
    private int _programId = 2;
    private int _generatedById = 10572;
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

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await FetchCommunications();

            GridParams.UserID = 10572;
            _generatedById = GridParams.UserID;

            GridParams.ProgramID = 2;
            _programId = GridParams.ProgramID;

            //await GetGridParams();
        }
        catch (Exception ex)
        {
            _ = _snackbar.Add($"We are unable to load the CC grid data at this time. {ex.Message} : {ex.StackTrace}", Severity.Warning);
        }
    }

    //private async Task GetGridParams()
    //{
    //    try
    //    {
    //        var obj = await _cookieStorageAccessor.GetValueAsync<string>("grid_params");
    //        if (obj == null || obj == "")
    //        {
    //            await _cookieStorageAccessor.WriteLogAsync<string>("Cookie _gid not found.");
    //        }
    //        else
    //        {
    //            //_ = _snackbar.Add($"Grid params : {obj}", Severity.Info);

    //            var str = Convert.ToString(obj);

    //            //_ = _snackbar.Add($"Grid params: {str}", Severity.Info);

    //            List<string> result = str?.Split('_').ToList();

    //            foreach (string s in result)
    //            {
    //                //_ = _snackbar.Add($"{s}", Severity.Info);

    //            }

    //            //var gParams = str
    //            //    .Split('_')
    //            //    .Where(x => int.TryParse(x.Trim(), out _))
    //            //    .Select(int.Parse)
    //            //    .ToList();

    //            GridParams.UserID = int.Parse(result[0]);
    //            _generatedById = GridParams.UserID;

    //            GridParams.ProgramID = int.Parse(result[1]);
    //            _programId = GridParams.ProgramID;

    //            await _cookieStorageAccessor.WriteLogAsync<string>(obj);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _ = _snackbar.Add($"We are unable to fetch the CC grid at this time. {ex.Message} : {ex.StackTrace}", Severity.Warning);
    //    }
    //}

    private async Task FetchCommunications()
    {
        isLoading = true;
        StateHasChanged();

        var result = await _customerManager.GetCommunicationsListAsync(_programId);

        _ = result.Status
            ? _snackbar.Add(result.Message, Severity.Success)
            : _snackbar.Add(result.Message, Severity.Error);

        Elements = result.Result.ToCommunicationGrid();
        isLoading = false;
        StateHasChanged();
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

            model.LPCID = Convert.ToInt32(item.LPCID);
            model.TemplateFile = item.FilePath;
            model.TemplateID = item.TemplateId;
            model.ProgramId = _programId;
            model.GeneratedBy = _generatedById;
            var result = await _customerManager.GenerateLetter<GenerateLetterDTO, GenerateLetterDTO>(model);
            ApiResult<GenerateLetterDTO> response = result as ApiResult<GenerateLetterDTO>;

            if (response?.Status == true)
            {
                var items = await _customerManager.GetCommunicationsListAsync(_programId);
                var cRow = items.Result.Where(f => f.BatchId == response.Result.BatchId).FirstOrDefault();
                item.GeneratedBy = cRow?.GeneratedBy;
                item.GeneratedDate = cRow?.GeneratedDate;
                item.ActionText = "Download PDF";
                item.GeneratedFilePath = response.Result.GeneratedFilePath;
                item.BatchId = response.Result.BatchId;

                // Enable the button again
                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();

                await _loadingIndicatorProvider.HoldAsync();

                //TODO: get letter type from backend
                var letterType = item.FilePath?.Split("__").Skip(1).FirstOrDefault();
                //string letterType = item.TemplateType; Enum.GetName(typeof(ETemplateType), TemplateType);
                var downloadResult = await _uploadManager.DownloadSourceFileAsync(response.Result.GeneratedFilePath);

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

                _ = _snackbar.Add($"We are unable to generate the Communication Letter at this time. <br> \r\n {response?.Message}", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            // Enable the button again
            item.IsButtonDisabled = false;
            item.IsProcessing = false;
            StateHasChanged();

            _ = _snackbar.Add($"We are unable to generate the Communication Letter at this time. {ex.Message} : {ex.StackTrace}", Severity.Warning);
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
            //var request = new DownloadExcelFileRequest
            //{
            //};
            //model.LPCID = Convert.ToInt32(item.LPCID);
            //model.TemplateFile = item.FilePath;
            //model.TemplateID = item.TemplateId;
            //model.ProgramId = _programId;
            //model.GeneratedBy = _generatedById;
            var result = await _uploadManager.DownloadExcelFileAsync<CommunicationDTO>(item);
            using DotNetStreamReference streamRef = new(result);
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC{""}.xlsx";
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
        catch (Exception)
        {
            _ = _snackbar.Add("Source file not found.", Severity.Error);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

    protected async Task ExportGridClicked()
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {
            var result = await _uploadManager.ExportGridAsync();
            using DotNetStreamReference streamRef = new(result);
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC.xlsx";
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (Exception ex)
        {
            await _loadingIndicatorProvider.ReleaseAsync();
            EDM.Common.Log.Error("Common", "EDMS.AP.Tools.CustomerCommunications.List", "ExportGridClicked", ex);
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

    public class GenerateLetterDTO
    {
        public string TemplateFile { get; set; } = "/CDN/HUP_Template/Home_Uplift__Ineligibility_Notice.pdf";
        public string NewLocalPath { get; set; } = "C:\\Users\\siddharth.k\\source\\EDM-DSM\\eScore\\EDMS.AP\\Tools\\CustomerCommunications\\ApplicationDoc\\";
        public int ProgramId { get; set; }
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int GeneratedBy { get; set; }
        public string? TemplateName { get; set; }
        public string? GeneratedFilePath { get; set; }
        public int BatchId { get; set; }
    }

    public class DownloadExcelFileDTO
    {
        public string TemplateFile { get; set; } = "/CDN/HUP_Template/Home_Uplift__Ineligibility_Notice.pdf";
        public string NewLocalPath { get; set; } = "C:\\Users\\siddharth.k\\source\\EDM-DSM\\eScore\\EDMS.AP\\Tools\\CustomerCommunications\\ApplicationDoc\\";
        public int ProgramId { get; set; }
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int GeneratedBy { get; set; }
        public string? TemplateName { get; set; }
        public string? GeneratedFilePath { get; set; }
        public int BatchId { get; set; }
    }
}
