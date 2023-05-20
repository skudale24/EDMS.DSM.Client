namespace EDMS.DSM.Client.Managers.RoadBridge;

public class QuoteManager : IQuoteManager
{
    private readonly HttpRequest _httpRequest;
    private readonly ILocalStorageService _localStorage;

    public QuoteManager(HttpRequest httpRequest, ILocalStorageService localStorage)
    {
        _httpRequest = httpRequest;
        _localStorage = localStorage;
    }


    public async Task<Stream> PrintQuote<TIn>(TIn quoteSubmit)
    {
        var urlWithParams = string.Empty;
        urlWithParams = $"{QuoteEndPoints.PrintQuote}";
        var stream = await _httpRequest.GetStreamDataAsync(urlWithParams, quoteSubmit).ConfigureAwait(false);
        return stream;
    }

    public async Task<IApiResult> QuoteBookMail<TIn>(TIn quoteSubmit)
    {
        var urlWithParams = string.Empty;
        urlWithParams = $"{QuoteEndPoints.BookMailQuote}";
        var res = await _httpRequest.PostRequest1Async(urlWithParams, quoteSubmit).ConfigureAwait(false);
        return res;
    }

    public async Task<IApiResult<TOut>> GetQuote<TIn, TOut>(TIn quoteRequest)
    {
        var urlWithParams = string.Empty;

        urlWithParams = $"{QuoteEndPoints.GetQuote}";

        var res = await _httpRequest.PostRequestAsync<TIn, TOut>(urlWithParams, quoteRequest).ConfigureAwait(false);

        return res;
    }

    public async Task<IApiResult<UserInfoDto>> GetUserData()
    {
        var urlWithParams = $"{NavMenuEndPoints.UserData}";

        return await _httpRequest.GetRequestAsync<UserInfoDto>(urlWithParams).ConfigureAwait(false);
    }

    public async Task UpdateExWorkChargesByQuoteID(string quoteRequest, int quoteId)
    {
        var urlWithParams = string.Empty;
        urlWithParams = $"{EndPoints.ApiBaseUrl}/api/v1/quote/updateExWorkCharges/{quoteId}";
        using var client = new HttpClient();

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

        var request = new HttpRequestMessage(HttpMethod.Post, urlWithParams);
        request.Content = new StringContent(quoteRequest, Encoding.UTF8, "application/json");
        request.Headers.Add(AppConstants.UserTokenHeaderKey, userToken);

        var response = await client.SendAsync(request).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }

    public async Task UpdateDAPDDUChargesByQuoteID(string quoteRequest, int quoteId)
    {
        var urlWithParams = string.Empty;
        urlWithParams = $"{EndPoints.ApiBaseUrl}/api/v1/quote/updateDAPDDUCharges/{quoteId}";
        using var client = new HttpClient();

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

        var request = new HttpRequestMessage(HttpMethod.Post, urlWithParams);
        request.Content = new StringContent(quoteRequest, Encoding.UTF8, "application/json");
        request.Headers.Add(AppConstants.UserTokenHeaderKey, userToken);

        var response = await client.SendAsync(request).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
