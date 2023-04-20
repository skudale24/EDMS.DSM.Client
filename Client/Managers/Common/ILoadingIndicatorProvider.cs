namespace EDMS.DSM.Client.Managers.Common;

public interface ILoadingIndicatorProvider : IManager
{
    Task HoldAsync();
    Task ReleaseAsync();
}
