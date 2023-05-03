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
    public class PageLoadSummary
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public String RequestedURL;
        public String PageLoadTime;
        public String IPAddress;  
        public long ByUserId;

        private SqlDb Db;
        private EDM.Common.Log Lg;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Common.PageLoadSummary";
            }
        }
 
        #endregion --- Properties ---

        #region --- Constructors ---
        public PageLoadSummary()
        {
            ProgramId = EDM.Setting.Session.ProgramId;
            ByUserId = EDM.Setting.Session.UserId;
            ConfigKey = String.Empty;
        }
        public PageLoadSummary(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public PageLoadSummary(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        #endregion --- Constructors ---

        #region --- Public Methods ---
     
        public Boolean Add()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.Module] = Module;
                prms[EDM.Setting.Fields.RequestedURL] = RequestedURL;
                prms[EDM.Setting.Fields.PageLoadTime] = PageLoadTime;
                prms[EDM.Setting.Fields.IPAddress] = IPAddress;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_PageLoadSummary", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Common.PageLoadSummary", "Add", SqlforLog);
                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error saving PageLoadSummary";
                    Common.Log.Info(Module + ":EDM.Common.PageLoadSummary", "Add", Message);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Common.PageLoadSummary", "Add", ex, "PageLoadSummary [" + ByUserId + "] UserId ");
                return false;
            }
        }
        #endregion --- Public Methods ---
    }
}
