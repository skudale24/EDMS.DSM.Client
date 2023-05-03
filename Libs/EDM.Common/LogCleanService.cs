using System;  
using VTI.Common; 

namespace EDM.Common
{
    public class LogCleanService
    {
        #region --- Members ---
        public String ConfigKey = String.Empty;
        public long ProgramId;
        public String Module;
        public long ByUserId;

        public SqlDb Db;
        public EDM.Common.Log Lg;
        #endregion

        #region --- Constructors ---
        public LogCleanService() { Init(); }
        public LogCleanService(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public LogCleanService(String module) : this() { Module = module; }
        public LogCleanService(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public LogCleanService(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new EDM.Common.Log(ConfigKey);
            Lg.ModuleName = configKey + ".Service.LogCleanService";
        }
        #endregion --- Private Methods ----

        #region --- Public Methods ---
        public EDM.Common.MethodReturn ProcessLogClean()
        {
            EDM.Common.MethodReturn mr = new EDM.Common.MethodReturn();
            string Message = string.Empty;
            try
            {
                Lg.Info("Module : ProcessLogClean", "--- Starting ---");
                EDM.Common.Log obj = new EDM.Common.Log(ConfigKey);
                if (!obj.Delete())
                {
                    Lg.Info("Module : ProcessLogClean", "Error:" + obj.Message);
                    Message = obj.Message;
                    mr.Status = false;
                }
                else
                {
                    Message = "Module: LogClean process are done sucessfully";
                    mr.Status = true;
                }               
                mr.Message = Message;
                Lg.Info("Module : ProcessLogClean", "--- End ---");                
                return mr;
            }
            catch (Exception ex)
            {
                mr.Status = false; mr.Message = ex.Message;
                Lg.Error("Module: ProcessLogClean: Exception", ex);
                return mr;
            }
        }
        #endregion
    }
}
