namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface IQuoteSearchResultManager : IManager
{
    Task<IListApiResult<List<QuoteSearchDto>>> Search(QuoteSearchResultFilter quoteSearchResultFilter);
    Task<Stream> ExportCSVReport(QuoteSearchResultFilter quoteSearchResultFilter);

    Task<IApiResult<QuoteSearchDetailDto>> Search(int id);

    Task<IApiResult> MarkAsExecuted(int id);

    Task<IApiResult> MarkAsClosed(int id);

    Task<IApiResult> AddComments(int id, string comment);

    Task<Stream> DownloadPdf(int id);

    Task<IApiResult> UpdateEfreight(int id, string efreightNo);
}
