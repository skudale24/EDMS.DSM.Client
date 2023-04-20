namespace EDMS.DSM.Client.Managers.Quote;

public interface IQuoteManager : IManager
{
    Task<IApiResult<TOut>> GetQuote<TIn, TOut>(TIn quoteRequest);
    Task<IApiResult> QuoteSubmit<TIn>(TIn quoteSubmit);
    Task<IApiResult<bool>> IsValid(string pincode);
}
