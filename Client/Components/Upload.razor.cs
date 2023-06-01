using EDMS.DSM.Client.Managers.User;
using Microsoft.AspNetCore.Components.Forms;

namespace EDMS.DSM.Client.Components;

public partial class Upload
{
    private readonly Func<Organization, string> converter = O => O.OrgDisplayName;

    private readonly List<string> OtherFileTypes = Enum.GetValues(typeof(SettingKeys)).Cast<SettingKeys>()
        .Where(W => W <= SettingKeys.UploadFilePath).Select(S => S.ToString()).ToList();

    private readonly Func<SettingKeys?, string> uploadFileTypesConverter = O => O.ToString();

    private readonly List<string> UploadFileTypesList = Enum.GetValues(typeof(PricingUploadFileType))
        .Cast<PricingUploadFileType>()
        .Select(S => S.ToString()).ToList();

    private IBrowserFile file = default!;
    private List<Organization> Organizations = new();
    private Organization SelectedOrganization = default!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IUserManager _userManager { get; set; } = default!;

    [Inject] private IUploadManager _uploadManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public bool ShowEmailEditor { get; set; }

    [Parameter] public string UploadFor { get; set; } = default!;

    [Parameter] public bool IsCommon { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Organizations = (await _userManager.GetOrganizationsAsync().ConfigureAwait(false)).Result;
    }

    private async Task Submit()
    {
        try
        {
            if (SelectedOrganization is null)
            {
                _ = _snackbar.Add("Please select an organization.", Severity.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(UploadFor))
            {
                _ = _snackbar.Add("Please select a file type.", Severity.Error);
                return;
            }

            if (file is null)
            {
                _ = _snackbar.Add("Please select a file.", Severity.Error);
                return;
            }

            await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);

            long maxFileSize = 1024 * 1024 * 1024;

            MemoryStream ms = new();
            await file.OpenReadStream(maxFileSize).CopyToAsync(ms).ConfigureAwait(false);
            _ = ms.Seek(0, SeekOrigin.Begin);
            _ = ms.GetBuffer();
            var uploadUri = OtherFileTypes.Contains(UploadFor)
                ? $"{UploadEndPoints.Upload}/{UploadFor}/{SelectedOrganization.OrgId}"
                : $"{UploadEndPoints.UploadPricing}/{UploadFor}/{SelectedOrganization.OrgId}";

            var res = await _uploadManager.UploadFileAsync(uploadUri, ms, file.Name).ConfigureAwait(false);
            MudDialog.Close(DialogResult.Ok(res.Status));

            await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _ = _snackbar.Add($"File not uploaded (Err:):{ex.Message}", Severity.Error);
            await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void UploadFiles(InputFileChangeEventArgs e)
    {
        file = e.File;
    }

    private async Task DownloadSample()
    {
        if (string.IsNullOrWhiteSpace(UploadFor))
        {
            _ = _snackbar.Add("Please select a file type.", Severity.Error);
            return;
        }

        //var csvString = CSVSampleConstants.GetCSVDownloadString(Enum.Parse<UploadFileTypes>(UploadFor));
        //await _jsRuntime.InvokeVoidAsync("SaveFileAsCSV", csvString, $"{UploadFor}_sample.csv").ConfigureAwait(false);
    }
}
