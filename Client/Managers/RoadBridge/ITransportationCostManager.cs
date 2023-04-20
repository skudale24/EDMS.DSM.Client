namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface ITransportationCostManager : IManager
{
    Task<IListApiResult<List<SearchTransportationCostDto>>> Search(TransportationCostFilter transportationCostFilter);
}
