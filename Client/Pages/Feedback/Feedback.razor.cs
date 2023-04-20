using EDMS.DSM.Client.Managers.Feedback;

namespace EDMS.DSM.Client.Pages.Feedback;

public partial class Feedback : ComponentBase, IDisposable
{
    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private IFeedbackManager _feedbackManager { get; set; } = default!;

    [Inject] private ISnackbar _snackbar { get; set; } = default!;

    private SubmitFeedbackUploadDto _feedbackSubmit { get; set; } = new();


    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _interceptor.RegisterEvent();
    }

    private async Task SubmitFeedback()
    {
        var userdata = (await _feedbackManager.GetUserData().ConfigureAwait(false)).Result;
        if (userdata != null)
        {
            _feedbackSubmit.UserEmail = userdata.EmailAddress;
            _feedbackSubmit.Name = userdata.UserName;
            _feedbackSubmit.Mobile = userdata.MobileNumber;
        }


        await _loadingIndicatorProvider.HoldAsync().ConfigureAwait(false);

        var result = await _feedbackManager.FeedbackSubmit(_feedbackSubmit).ConfigureAwait(false);

        _ = result.Status
            ? _snackbar.Add(result.Message, Severity.Success)
            : _snackbar.Add(result.Message, Severity.Error);
        StateHasChanged();

        await _loadingIndicatorProvider.ReleaseAsync().ConfigureAwait(false);
    }

    private void ResetFilters()
    {
        _feedbackSubmit = new SubmitFeedbackUploadDto();
    }
}
