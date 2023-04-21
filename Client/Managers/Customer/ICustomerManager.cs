using EDMS.DSM.Shared.Models;
using Telerik.DataSource;

namespace EDMS.DSM.Managers.Customer
{
    public interface ICustomerManager
    {
        Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest);
        Task<IApiResult<List<Communications>>> GetCommunicationsListAsync();
    }
}