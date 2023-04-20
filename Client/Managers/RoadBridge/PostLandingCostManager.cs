//using Microsoft.AspNetCore.Http;

namespace EDMS.DSM.Client.Managers.RoadBridge;

public class PostLandingCostManager : IPostLandingCostManager
{
    private readonly HttpRequest _httpRequest;

    public PostLandingCostManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<SearchPostLandingCostDto>>> Search(
        PostLandingCostFilter postLandingCostFilter)
    {
        var queryString = HttpExtensions.GenerateQueryString(postLandingCostFilter);
        var urlWithParams = $"{PostLandingCostEndPoints.Search}/?{queryString}";

        return await _httpRequest.GetRequestAsync<List<SearchPostLandingCostDto>>(urlWithParams).ConfigureAwait(false);
    }
}
