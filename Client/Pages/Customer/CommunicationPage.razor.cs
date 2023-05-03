using EDMS.DSM.Managers.Customer;

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

        var result = await _customerManager.GetCommunicationsListAsync();

        _ = result.Status
            ? _snackbar.Add(result.Message, Severity.Success)
            : _snackbar.Add(result.Message, Severity.Error);

        Elements = result.Result.ToCommunicationGrid();
        isLoading = false;
        StateHasChanged();
    }
    private async Task ProcessLetterGeneration(CommunicationDTO item)
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {
            Console.WriteLine($"Item: {item.LPCID}");
            item.IsProcessing = true;
            // Disable the button
            item.IsButtonDisabled = true;
            Console.WriteLine($"Item IsProcessing: {item.IsProcessing}");
            StateHasChanged();

            // Perform the long-running operation
            // Do some long-running operation here
            var result = await _customerManager.GenerateLetter(model);
            //await Task.Delay(2000);

            // Enable the button again
            item.IsButtonDisabled = false;
            item.IsProcessing = false;

            //await FetchCommunications();

            //Simulate PDF creation
            if (result.Status)
            {
                item.Action = "Download PDF";
            }

            StateHasChanged();

            await _loadingIndicatorProvider.ReleaseAsync();
        }
        catch (Exception)
        {
            _ = _snackbar.Add("Error occurred while generating letter.", Severity.Error);
            await _loadingIndicatorProvider.ReleaseAsync();
        }
    }

    //private async Task GenerateLetterAsync()
    //{
    //    try
    //    {
    //        // Do some long-running operation here
    //        await Task.Delay(2000);
    //        await _loadingIndicatorProvider.ReleaseAsync();
    //    }
    //    catch (Exception)
    //    {
    //        _ = _snackbar.Add("Error occurred while generating letter.", Severity.Error);
    //        await _loadingIndicatorProvider.ReleaseAsync();
    //    }
    //}

    private async Task DownloadSourceFile(string csvFileName)
    {
        await _loadingIndicatorProvider.HoldAsync();

        try
        {
            var result = await _uploadManager.DownloadSourceFileAsync(csvFileName);
            using DotNetStreamReference streamRef = new(result);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", csvFileName, streamRef);
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
        public string TemplateFile { get; set; } = "Home_Uplift__Ineligibility_Notice.pdf";
        public string NewLocalPath { get; set; } = "C:\\Users\\siddharth.k\\source\\EDM-DSM\\eScore\\EDMS.AP\\Tools\\CustomerCommunications\\ApplicationDoc\\";
        public int ProgramId { get; set; } = 2;
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int GeneratedBy { get; set; } = 10572;

        //[Required(ErrorMessage = "The Segment field is required.")]
        //public string Segment { get; set; }

        //[Required(ErrorMessage = "The Country field is required.")]
        //public string Country { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The City field is required.")]
        //public string City { get; set; }

        //[Required(ErrorMessage = "The Name field is required.")]
        //public string Name { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The PhoneNo field is required.")]
        //public string PhoneNo { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The Address field is required.")]
        //public string Address { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The FaxNo field is required.")]
        //public string FaxNo { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The Email field is required.")]
        //public string Email { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The Category field is required.")]

        //public string Category { get; set; }

        //[Required(ErrorMessage = "The Routing field is required.")]

        //public string Routing { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The HubAgent field is required.")]
        //public string HubAgent { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The Code field is required.")]
        //public string Code { get; set; } = string.Empty;
    }

}
