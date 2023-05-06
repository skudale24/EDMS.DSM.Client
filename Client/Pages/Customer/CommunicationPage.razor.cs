using EDMS.DSM.Managers.Customer;
using Telerik.SvgIcons;

namespace EDMS.DSM.Client.Pages.Customer;

public partial class CommunicationPage : ComponentBase, IDisposable
{
    //    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IUploadManager _uploadManager { get; set; } = default!;

    [Inject] private ICustomerManager _customerManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

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

        if (x.Action?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
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
        await FetchCommunications();
    }

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
                item.Action = "Download PDF";

                // Enable the button again
                item.IsButtonDisabled = false;
                item.IsProcessing = false;
                StateHasChanged();

                await _loadingIndicatorProvider.HoldAsync();

                var letterType = item.FilePath?.Split("__").Skip(1).FirstOrDefault();
                //string letterType = item.TemplateType; Enum.GetName(typeof(ETemplateType), TemplateType);
                var downloadResult = await _uploadManager.DownloadSourceFileAsync(response.Result.GeneratedFilePath);

                MemoryStream ms = new();
                await downloadResult.CopyToAsync(ms);
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC{letterType}";
                await _jsRuntime.InvokeVoidAsync("OpenFileAsPDF", ms.GetBuffer(), fileName);

                await _loadingIndicatorProvider.ReleaseAsync();

            }

        }
        catch (Exception ex)
        {
            // Enable the button again
            item.IsButtonDisabled = false;
            item.IsProcessing = false;
            StateHasChanged();

            _ = _snackbar.Add("Error occurred while generating letter.", Severity.Error);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

    private async Task DownloadExcel()
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {
            // Do some long-running operation here
            //await Task.Delay(2000);

            var result = await _uploadManager.DownloadExcelFileAsync("");
            using DotNetStreamReference streamRef = new(result);
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_CC{""}";
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            await _loadingIndicatorProvider.ReleaseAsync();

        }
        catch (Exception)
        {
            _ = _snackbar.Add("We are facing some issues generating the excel file. Please try again later.", Severity.Warning);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

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
        public int ProgramId { get; set; } = 2;
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int GeneratedBy { get; set; } = 10572;
        public string? TemplateName { get; set; }
        public string? GeneratedFilePath { get; set; }
        public int BatchId { get; set; }
    }
}
