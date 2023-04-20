namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface IQuoteManager : IManager
{
    Task<IApiResult<TOut>> GetQuote<TIn, TOut>(TIn quoteRequest);

    Task<Stream> PrintQuote<TIn>(TIn quoteSubmit);
    Task<IApiResult> QuoteBookMail<TIn>(TIn quoteSubmit);
    Task<IApiResult<UserInfoDto>> GetUserData();

    Task UpdateExWorkChargesByQuoteID(string quoteRequest, int quoteId);
    Task UpdateDAPDDUChargesByQuoteID(string quoteRequest, int quoteId);
}
