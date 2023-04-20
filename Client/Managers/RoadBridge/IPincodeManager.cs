namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface IPincodeManager : IManager
{
    Task<IListApiResult<List<SearchPincodeDto>>> Search(PincodeFilter pincodeFilter);
}
