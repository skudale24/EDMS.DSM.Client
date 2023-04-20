namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface IPostLandingCostManager : IManager
{
    Task<IListApiResult<List<SearchPostLandingCostDto>>> Search(PostLandingCostFilter postLandingCostFilter);
}
