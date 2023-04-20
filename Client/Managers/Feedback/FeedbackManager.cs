namespace EDMS.DSM.Client.Managers.Feedback;

public class FeedbackManager : IFeedbackManager
{
    private readonly HttpRequest _httpRequest;

    public FeedbackManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }


    public async Task<IApiResult> FeedbackSubmit<TIn>(TIn feedbackSubmit)
    {
        var urlWithParams = string.Empty;

        urlWithParams = $"{FeedbackPoints.FeedbackEmail}";

        var res = await _httpRequest.PostRequestAsync<TIn, ApiResult>(urlWithParams, feedbackSubmit)
            .ConfigureAwait(false);

        return res;
    }

    public async Task<IApiResult<UserInfoDto>> GetUserData()
    {
        var urlWithParams = $"{NavMenuEndPoints.UserData}";

        return await _httpRequest.GetRequestAsync<UserInfoDto>(urlWithParams).ConfigureAwait(false);
    }
}
