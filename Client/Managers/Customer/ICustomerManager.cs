using EDMS.DSM.Shared.Models;

namespace EDMS.DSM.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        //Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest);
        Task<IListApiResult<List<Communication>>> GetCommunicationsListAsync(int programId);
        Task<IApiResult> GenerateLetter<TIn, TOut>(TIn communication);
    }
}