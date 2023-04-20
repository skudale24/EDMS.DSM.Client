namespace EDMS.DSM.Client.Store.UseCase.DapDduQuote;

[FeatureState]
public class State
{
    public State()
    {
    }

    public State(QuoteRequest dapQuoteRequest)
    {
        QuoteRequest = dapQuoteRequest;
    }

    public QuoteRequest QuoteRequest { get; init; } = new();
}
