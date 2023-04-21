using EDMS.DSM.Shared.Models;
using Telerik.DataSource;

namespace EDMS.DSM.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest);
        Task<IListApiResult<List<Communications>>> GetCommunicationsListAsync();
    }
}