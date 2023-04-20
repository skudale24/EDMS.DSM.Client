namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface IExworksRateManager : IManager
{
    Task<IListApiResult<List<SearchExworksRateDto>>> Search(ExworksRateFilter exworksRateFilter);
}
