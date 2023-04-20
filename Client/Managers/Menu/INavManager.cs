namespace EDMS.DSM.Client.Managers.Menu;

public interface INavManager : IManager
{
    Task<IListApiResult<List<NavMenuDto>>> GetMenus();
    Task<IApiResult<UserInfoDto>> GetUserData();
}
