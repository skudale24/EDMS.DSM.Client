namespace EDMS.DSM.Client.Managers.Quote;

public class QuoteManager : IQuoteManager
{
    private readonly HttpRequest _httpRequest;

    public QuoteManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IApiResult> QuoteSubmit<TIn>(TIn quoteSubmit)
    {
        var urlWithParams = string.Empty;

        urlWithParams = $"{QuoteEndPoints.SubmitQuote}";

        var res = await _httpRequest.PostRequestAsync<TIn, ApiResult>(urlWithParams, quoteSubmit).ConfigureAwait(false);

        return res;
    }

    public async Task<IApiResult<TOut>> GetQuote<TIn, TOut>(TIn quoteRequest)
    {
        var urlWithParams = string.Empty;

        urlWithParams = $"{QuoteEndPoints.GetQuote}";

        var res = await _httpRequest.PostRequestAsync<TIn, TOut>(urlWithParams, quoteRequest).ConfigureAwait(false);

        return res;
    }

    public async Task<IApiResult<bool>> IsValid(string pincode)
    {
        var urlWithParams = string.Empty;
        urlWithParams = $"{QuoteEndPoints.IsValid}/{pincode}";
        return await _httpRequest.GetRequestAsync<bool>(urlWithParams).ConfigureAwait(false);
    }
}
