using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.Common
{
   public class UtilityAccess
    {
        #region --- Members ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId = 0;

        private EDM.Common.Log Lg;
        private SqlDb Db;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Common.UtilityAccess";
            }
        }
        #endregion

        #region --- Constructors ---
        public UtilityAccess() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; Init(); }
       // public UtilityAccess(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public UtilityAccess(String module) : this() { Module = module;  }
        public UtilityAccess(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        #endregion

        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.Badge.Badge";
        }

        #region --- Methods ---
        public Boolean ISCustomerAllowedEmail(string ToEmail, long LpcId)
        {
            bool result = true;
            try
            {
                if (!string.IsNullOrEmpty(ToEmail) && ToEmail.ToLower().Contains("customeremail") && !CHKCustomerAllowedSignUp(LpcId))
                {
                    result = false;
                }
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
            return result;
        }
        public Boolean CHKCustomerAllowedSignUp(long UtilityId)
        {
            long programId = ProgramId == (long)Programs.GPP ? (long)Programs.DPPWithGPP : ProgramId;

            String logParams = "ProgramID:" + programId + "|CompanyID:" + UtilityId;

            try
            {
                if (programId <= 0) { Message = "ProgramId is required."; return false; }
                if (UtilityId <= 0) { Message = "UtilityId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = programId;
                prms["CompanyID"] = UtilityId;

                Db.SetSql("p_CHK_CustomerAllowedSignUp", prms);
                Lg.Info("CustomerAllowedSignUp", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Customer not Allowed to SignUp.";
                    Lg.Info("CustomerAllowedSignUp", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (MsSql.CheckBoolDBNull(dr["ISAllowedSignUp"]))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("CustomerAllowedSignUp", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}
