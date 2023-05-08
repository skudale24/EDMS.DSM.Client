using EDMS.Data.Constants;
using EDMS.DSM.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;


namespace EDM.CommunicationTemplate
{
    
    public class CustomerCommunications
    {
        #region ---Variables---
        public string Module { get; set; }
        public string Message { get; set; } = String.Empty;
        public long ProgramId { get; set; }
        public long ByUserId { get; set; }
        public long ProjectID { get; set; }
        public long ObjectID { get; set; }

        private SqlDb Db;
        private Common.Log Lg;
        private String _configKey = String.Empty;

        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.CommunicationTemplate";
            }
        }
        #endregion

        #region ---Constructor---
        public CustomerCommunications()
        {
            ConfigKey = String.Empty;
        }
        #endregion

        #region ---Methods---
       

        public DataSet GetAllCustomerCommunication()
        {
            try
            {
                //Hashtable prms = new Hashtable();
                //prms["ProgramId"] = ProgramId;
                //Db.SetSql("p_Get_HUP_AggregateList4CustomerCommunications", prms);
                //Lg.Info("GetAllCustomerCommunication", Db.SqlStmt);
                //return Db.ExecuteNoTransQuery();

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProgramID", SqlDbType.Int);
                parameters[0].Value = ProgramId;
                DataSet data = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(
                    SQLConstants.ConnectionString, 
                    "p_Get_HUP_AggregateList4CustomerCommunications", 
                    parameters);
                return data;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllCustomerCommunication", ex);
                return null;
            }
        }

        public DataSet GetApplicationDataByBatchId(int? batchId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BatchId", SqlDbType.Int);
                parameters[0].Value = batchId;
                DataSet data = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(
                    SQLConstants.ConnectionString,
                    "p_GET_HUP_CustomerCommunications",
                    parameters);
                return data;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetApplicationDataByBatchId", ex);
                return null;
            }
        }

        #endregion
    }
}
