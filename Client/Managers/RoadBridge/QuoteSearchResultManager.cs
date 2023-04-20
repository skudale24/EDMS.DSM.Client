namespace EDMS.DSM.Client.Managers.RoadBridge;

public class QuoteSearchResultManager : IQuoteSearchResultManager
{
    private readonly HttpRequest _httpRequest;

    public QuoteSearchResultManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IListApiResult<List<QuoteSearchDto>>> Search(QuoteSearchResultFilter quoteSearchResultFilter)
    {
        var queryString = HttpExtensions.GenerateQueryString(quoteSearchResultFilter);
        var urlWithParams = $"{QuoteSearch.Search}/?{queryString}";

        return await _httpRequest.GetRequestAsync<List<QuoteSearchDto>>(urlWithParams).ConfigureAwait(false);
    }

    public async Task<IApiResult<QuoteSearchDetailDto>> Search(int id)
    {
        var urlWithParams = $"{QuoteSearch.Search}/{id}";
        return await _httpRequest.GetRequestAsync<QuoteSearchDetailDto>(urlWithParams).ConfigureAwait(false);
    }

    public async Task<Stream> DownloadPdf(int id)
    {
        var urlWithParams = $"{QuoteSearch.DownloadPdfFile}/{id}";
        var stream = await _httpRequest.GetStreamAsync(urlWithParams).ConfigureAwait(false);

        return stream;
    }


    public async Task<Stream> ExportCSVReport(QuoteSearchResultFilter quoteSearchResultFilter)
    {
        var queryString = HttpExtensions.GenerateQueryString(quoteSearchResultFilter);
        var urlWithParams = $"{QuoteSearch.ExportReport}/?{queryString}";
        var stream = await _httpRequest.GetStreamAsync(urlWithParams).ConfigureAwait(false);
        return stream;
    }

    public async Task<IApiResult> MarkAsExecuted(int id)
    {
        var urlWithParams = $"{QuoteSearch.MarkAsExecuted}/{id}";
        var response = await _httpRequest.PostRequestAsync<string, ApiResult>(urlWithParams, string.Empty)
            .ConfigureAwait(false);

        return response;
    }

    public async Task<IApiResult> MarkAsClosed(int id)
    {
        var urlWithParams = $"{QuoteSearch.MarkAsClosed}/{id}";
        var response = await _httpRequest.PostRequestAsync<string, ApiResult>(urlWithParams, string.Empty)
            .ConfigureAwait(false);

        return response;
    }

    public async Task<IApiResult> AddComments(int id, string comment)
    {
        var urlWithParams = $"{QuoteSearch.AddComments}/{id}";
        var response = await _httpRequest.PostRequestAsync<string, ApiResult>(urlWithParams, comment)
            .ConfigureAwait(false);

        return response;
    }

    public async Task<IApiResult> UpdateEfreight(int id, string efreightNo)
    {
        var urlWithParams = $"{QuoteSearch.UpdateEfreightNo}/{id}";
        var response = await _httpRequest.PutRequestAsync<string, ApiResult>(urlWithParams, efreightNo)
            .ConfigureAwait(false);

        return response;
    }
}
