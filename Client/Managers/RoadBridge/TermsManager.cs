namespace EDMS.DSM.Client.Managers.RoadBridge;

public class TermsManager : ITermsManager
{
    private readonly HttpRequest _httpRequest;

    public TermsManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<TermsDto>>> GetTermsAndConditions()
    {
        var urlWithParams = $"{TermsEndPoints.Terms}";

        return await _httpRequest.GetRequestAsync<List<TermsDto>>(urlWithParams).ConfigureAwait(false);
    }

    public async Task<IApiResult> SaveTermsAndConditions(List<TermsDto> termsAndConditions)
    {
        var urlWithParams = $"{TermsEndPoints.Terms}";

        var res = await _httpRequest.PostRequestAsync<List<TermsDto>, ApiResult>(urlWithParams, termsAndConditions)
            .ConfigureAwait(false);

        return res;
    }
}
