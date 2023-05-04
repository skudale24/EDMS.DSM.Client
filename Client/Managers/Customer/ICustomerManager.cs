using EDMS.DSM.Shared.Models;
using Telerik.DataSource;
using Telerik.SvgIcons;

namespace EDMS.DSM.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest);
        Task<IListApiResult<List<Communications>>> GetCommunicationsListAsync();
        Task<IApiResult> GenerateLetter<TIn, TOut>(TIn editAgentDto);
    }
}