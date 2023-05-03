using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Common
{
    public class AuditLog
    {
        #region --- Members ---
        public String Module;
        public String Message;

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
                Lg.ModuleName = Module + ":EDM.Common.AuditLog";
            }
        }
        #endregion

        #region --- Constructors ---
        public AuditLog() { ConfigKey = String.Empty; }
        public AuditLog(String module) { Module = module; ConfigKey = String.Empty; }
        #endregion

        #region --- Methods ---
        public DataSet GetAll(String startDt = "", String endDt = "")
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (!String.IsNullOrEmpty(startDt)) prms["Start"] = startDt;
                if (!String.IsNullOrEmpty(endDt)) prms["End"] = endDt;

                Db.SetSql("p_GET_AuditLogs", prms);
                Lg.Info("GetAll", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex);
                return null;
            }
        }
        #endregion
    }
}
