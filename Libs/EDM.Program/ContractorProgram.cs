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
   public class ContractorProgram
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
                Lg.ModuleName = _configKey + ":" + Module + ":EDM.Program.ContractorProgram";
            }
        }

        public long ProgramId = 0;
        public String ProgramName = String.Empty;
        public long CompanyID = 0;
        public string CompanyType = String.Empty;
        public long ByUserId = 0, StatusID=1;
        public int ProgramEnrollCount = 0;
        public long ProgramCompanyID = 0;
        public long AcctMgrID = 0;
        #endregion

        #region --- Constructors ---
        public ContractorProgram() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; ConfigKey = String.Empty; }
        public ContractorProgram(long id) : this() { ProgramId = id; ConfigKey = String.Empty; }
        public ContractorProgram(String configKey) : this() { ConfigKey = configKey; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// LpcId may be specified.
        /// </summary>
        public DataSet GetCurrentProgramEnrolled()
        {
            String logParams = "CompanyID:" + CompanyID+ "|CompanyType:"+ CompanyType;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CompanyID"] = CompanyID;
                prms["CompanyType"] = CompanyType;

                Db.SetSql("p_GET_CurrentContractorProgramEnrolled", prms);
                Lg.Debug("GetCurrentContractorProgramEnrolled", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetCurrentContractorProgramEnrolled", ex, logParams); return null; }
        }
        public DataSet GetProgramEnrollmentHistory()
        {
            String logParams = "CompanyID:" + CompanyID + "|CompanyType:" + CompanyType;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CompanyID"] = CompanyID;
                prms["CompanyType"] = CompanyType;

                Db.SetSql("p_GET_ContractorProgramEnrollmentHistory", prms);
                Lg.Debug("GetProgramEnrollmentHistory", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetProgramEnrollmentHistory", ex, logParams); return null; }
        }

        /// <summary>
        /// ProgramId, CallScript, ByUserId are required.
        /// </summary>
        public Boolean EnrollUnEnroll()
        {
            String logParams = "CompanyID:" + CompanyID + "|CompanyType:" + CompanyType + "|ProgramID:" + ProgramId + "|ProgramName:" + ProgramName + "|StatusID:" + StatusID + "|ByUserID:" + ByUserId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }
                if (CompanyID <= 0) { Message = "CompanyID is required."; return false; }
                if (String.IsNullOrEmpty(CompanyType)) { Message = "CompanyType is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["ProgramName"] = ProgramName;
                prms["ByUserID"] = ByUserId;
                prms["CompanyID"] = CompanyID;
                prms["CompanyType"] = CompanyType;
                prms["StatusID"] = StatusID;

                Db.SetSql("p_AU_EnrollUnEnrollContractorProgram", prms);
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
                return SqlDb.CheckLongDBNull(dr["ContractorProgramID"]) <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Enroll", ex, logParams); return false; }
        }
        public Boolean AddUpdateContractorProgramAcctMgr()
        {
            String logParams = "ProgramCompanyID:" + ProgramCompanyID + "|CompanyID:" + CompanyID + "|ProgramID:" + ProgramId + "|AcctMgrID:" + AcctMgrID + "|ByUserID:" + ByUserId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramCompanyID"] = ProgramCompanyID;
                prms["CompanyID"] = CompanyID;
                prms["ProgramID"] = ProgramId;
                prms["AcctMgrID"] = AcctMgrID;
                prms["ByUserID"] = ByUserId;
                
                Db.SetSql("p_AU_ContractorProgramAcctMgr", prms);
                Lg.Debug("AddUpdateContractorProgramAcctMgr", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error saving record";
                    Lg.Info("AddUpdateContractorProgramAcctMgr", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                return SqlDb.CheckLongDBNull(dr["ProgramCompanyID"]) <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("AddUpdateContractorProgramAcctMgr", ex, logParams); return false; }
        }
        public int IsProgramEnrolled()
        {
            String logParams = "CompanyID:" + CompanyID + "|CompanyType:" + CompanyType + "|ProgramID:" + ProgramId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return -1; }
                if (CompanyID <= 0) { Message = "CompanyID is required."; return -1; }
                if (String.IsNullOrEmpty(CompanyType)) { Message = "CompanyType is required."; return -1; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["CompanyID"] = CompanyID;
                prms["CompanyType"] = CompanyType;

                Db.SetSql("p_CHK_ContractorProgramEnrolled", prms);
                Lg.Debug("IsProgramEnrolled", Db.SqlStmt);
                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Record not found.";
                    Lg.Info("IsProgramEnrolled", Db.SqlStmt + "|" + Message);
                    return -1;  // Technical glitch
                }

                DataRow dr = ds.Tables[0].Rows[0];

                ProgramEnrollCount = SqlDb.CheckIntDBNull(dr["ProgramEnrollCount"]);

                if (SqlDb.CheckLongDBNull(dr["IDCount"]) > 0) { return 1; } // Exists     

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
