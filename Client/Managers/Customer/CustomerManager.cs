﻿using EDMS.DSM.Shared.Models;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace EDMS.DSM.Managers.Customer
{
    public class CustomerManager : ICustomerManager
    {
        private readonly HttpRequest _httpRequest;

        public CustomerManager(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        private List<Communications> _communications { get; set; }

        public async Task<DataSourceResult> GetCommunicationsListAsync(DataSourceRequest gridRequest)
        {

            var urlWithParams = $"{CustomerEndpoints.GetCommunications}";

            IApiResult<List<Communications>> result = await _httpRequest.GetRequestAsync<List<Communications>>(urlWithParams);

            //if (_communications.IsNullOrEmpty())
            //{
            //    //_communications = _db.Database.SqlQuery<Communications>($"EXECUTE dbo.[p_Get_HUP_AggregateList4CustomerCommunications_1]").ToList();
            //}

            _communications = result.Result;

            // use the Telerik DataSource Extensions to perform the query on the data
            // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
            DataSourceResult dataToReturn = await _communications.ToDataSourceResultAsync(gridRequest);

            return dataToReturn;
        }

        public async Task<IApiResult<List<Communications>>> GetCommunicationsListAsync()
        {
            var urlWithParams = $"{CustomerEndpoints.GetCommunications}";

            IApiResult<List<Communications>> result = await _httpRequest.GetRequestAsync<List<Communications>>(urlWithParams);

            _communications = result.Result;

            return result;
        }
    }
}