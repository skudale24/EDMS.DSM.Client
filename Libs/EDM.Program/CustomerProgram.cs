using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.Program
{
    public class CustomerProgram
    {
        #region --- Properties ---
        public String Module;
        public String Message;

        private String _configKey = String.Empty;
        private SqlDb Db;
        private EDM.Common.Log Lg;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = _configKey + ":" + Module + ":EDM.Program.CustomerProgram";
            }
        }

        public long ProgramId = 0;
        public String ProgramName = String.Empty;
        public long CustomerLoginID = 0;
        public long ServiceAddressID = 0;
        public long ByUserId = 0;
        #endregion

        #region --- Constructors ---
        public CustomerProgram() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; ConfigKey = String.Empty; }
        public CustomerProgram(long id) : this() { ProgramId = id; ConfigKey = String.Empty; }
        public CustomerProgram(String configKey) : this() { ConfigKey = configKey; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// LpcId may be specified.
        /// </summary>
        public DataSet GetCurrentProgramEnrolled()
        {
            String logParams = "CustomerLoginID:" + CustomerLoginID;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CustomerLoginID"] = CustomerLoginID;

                Db.SetSql("p_GET_CurrentProgramEnrolled", prms);
                Lg.Debug("GetCurrentProgramEnrolled", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetCurrentProgramEnrolled", ex, logParams); return null; }
        }
        public DataSet GetProgramEnrollmentHistory()
        {
            String logParams = "CustomerLoginID:" + CustomerLoginID;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CustomerLoginID"] = CustomerLoginID;

                Db.SetSql("p_GET_ProgramEnrollmentHistory", prms);
                Lg.Debug("GetProgramEnrollmentHistory", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetProgramEnrollmentHistory", ex, logParams); return null; }
        }

        /// <summary>
        /// ProgramId, CallScript, ByUserId are required.
        /// </summary>
        public Boolean Enroll()
        {
            String logParams = "CustomerLoginID:" + CustomerLoginID + "|ServiceAddressID:" + ServiceAddressID + "|ProgramID:" + ProgramId + "|ProgramName:" + ProgramName + "|ByUserID:" + ByUserId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }
                if (CustomerLoginID <= 0) { Message = "CustomerLoginID is required."; return false; }
                if (ServiceAddressID <= 0) { Message = "ServiceAddressID is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["ProgramName"] = ProgramName;
                prms["ByUserID"] = ByUserId;
                prms["CustomerLoginID"] = CustomerLoginID;
                prms["ServiceAddressID"] = ServiceAddressID;

                Db.SetSql("p_AU_EnrollCustomerProgram", prms);
                Lg.Debug("Enroll", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error saving record";
                    Lg.Info("Enroll", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                return SqlDb.CheckLongDBNull(dr["CustomerProgramID"]) <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Enroll", ex, logParams); return false; }
        }
        public int IsProgramEnrolled()
        {
            String logParams = "CustomerLoginID:" + CustomerLoginID+ "|ServiceAddressID:"+ ServiceAddressID+ "|ProgramID:"+ ProgramId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return -1; }
                if (CustomerLoginID <= 0) { Message = "CustomerLoginID is required."; return -1; }
                if (ServiceAddressID <= 0) { Message = "ServiceAddressID is required."; return -1; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["CustomerLoginID"] = CustomerLoginID;
                prms["ServiceAddressID"] = ServiceAddressID;

                Db.SetSql("p_CHK_ProgramEnrolled", prms);
                Lg.Debug("IsProgramEnrolled", Db.SqlStmt);
                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Record not found.";
                    Lg.Info("IsProgramEnrolled", Db.SqlStmt + "|" + Message);
                    return -1;  // Technical glitch
                }

                DataRow dr = ds.Tables[0].Rows[0];
                if (SqlDb.CheckLongDBNull(dr["IDCount"]) > 0) return 1;    // Exists

                return 0;   // Doesn't exist, all good
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("IsProgramEnrolled", ex, logParams);
                return -1;
            }
        }
        #endregion
    }
}
