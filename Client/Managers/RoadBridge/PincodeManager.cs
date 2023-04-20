namespace EDMS.DSM.Client.Managers.RoadBridge;

public class PincodeManager : IPincodeManager
{
    private readonly HttpRequest _httpRequest;

    public PincodeManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<SearchPincodeDto>>> Search(PincodeFilter pincodeFilter)
    {
        var queryString = HttpExtensions.GenerateQueryString(pincodeFilter);
        var urlWithParams = $"{PincodeEndPoints.Search}/?{queryString}";

        return await _httpRequest.GetRequestAsync<List<SearchPincodeDto>>(urlWithParams).ConfigureAwait(false);
    }
}
