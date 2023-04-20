namespace EDMS.DSM.Client.Store.UseCase.DapDduQuote;

public static class Reducers
{
    [ReducerMethod]
    public static State ReduceStateAction(State state,
        Action action)
    {
        return action != null && action.quoteRequest != null ? new State(action.quoteRequest) : state;
    }
}
