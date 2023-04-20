namespace EDMS.DSM.Client.Store.UseCase.Navigation;

public static class Reducers
{
    [ReducerMethod]
    public static State ReduceStateAction(State state,
        Action action)
    {
        return action == null && action.NavMenus == null ? state : new State(action.NavMenus);
    }
}
