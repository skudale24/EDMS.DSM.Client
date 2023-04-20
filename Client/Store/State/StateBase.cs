namespace EDMS.DSM.Client.Store.State;

public abstract class StateBase
{
    public StateBase(bool isLoading, string? currentErrorMessage)
    {
        (IsLoading, CurrentErrorMessage) = (isLoading, currentErrorMessage);
    }

    public bool IsLoading { get; }

    public string? CurrentErrorMessage { get; }

    public bool HasCurrentErrors => !string.IsNullOrWhiteSpace(CurrentErrorMessage);
}
