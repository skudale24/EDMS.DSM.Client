namespace EDMS.DSM.Client.Managers.RoadBridge;

public class ExworksRateManager : IExworksRateManager
{
    private readonly HttpRequest _httpRequest;

    public ExworksRateManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<SearchExworksRateDto>>> Search(ExworksRateFilter exworksRateFilter)
    {
        var queryString = HttpExtensions.GenerateQueryString(exworksRateFilter);
        var urlWithParams = $"{ExWorkEndPoints.Search}/?{queryString}";

        return await _httpRequest.GetRequestAsync<List<SearchExworksRateDto>>(urlWithParams).ConfigureAwait(false);
    }
}
