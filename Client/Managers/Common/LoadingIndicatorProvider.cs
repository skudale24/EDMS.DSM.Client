namespace EDMS.DSM.Client.Managers.Common;

public class LoadingIndicatorProvider : ILoadingIndicatorProvider
{
    private readonly IJSRuntime _js;

    public LoadingIndicatorProvider(IJSRuntime js)
    {
        _js = js;
    }

    public async Task HoldAsync()
    {
        await _js.InvokeVoidAsync("showLoadingIndicator").ConfigureAwait(false);
    }

    public async Task ReleaseAsync()
    {
        await _js.InvokeVoidAsync("hideLoadingIndicator").ConfigureAwait(false);
    }
}
