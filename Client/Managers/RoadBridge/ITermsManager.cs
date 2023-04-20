namespace EDMS.DSM.Client.Managers.RoadBridge;

public interface ITermsManager : IManager
{
    Task<IListApiResult<List<TermsDto>>> GetTermsAndConditions();
    Task<IApiResult> SaveTermsAndConditions(List<TermsDto> termsAndConditions);
}
