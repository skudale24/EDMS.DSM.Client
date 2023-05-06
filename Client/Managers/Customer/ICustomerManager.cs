using EDMS.DSM.Shared.Models;
using Telerik.DataSource;

namespace EDMS.DSM.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest);
        Task<IListApiResult<List<Communications>>> GetCommunicationsListAsync(int programId);
        Task<IApiResult> GenerateLetter<TIn, TOut>(TIn editAgentDto);
    }
}