using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class MissingProjectReport
    {
        #region --- Properties ---
        public long ProgramId;
        public long ByUserId;

        public SqlDb Db;
        public Common.Log Lg;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.DocFile.MissingProjectReport";
            }
        }

        public String Module;
        public String Message;
        public int PKID;
        public long ReportMissingID;
        public long ProjectID;
        public long AdvisorID;             
        public String AdvisorName;
        public long MeasureID;
        public String MeasureName;
        public String Reasons;        
              
        #endregion --- Properties ---

        #region --- Abstract Properties ---        
        protected virtual String GetAllSql { get { return "p_GET_AllMissingReportInfo"; } }
        protected virtual String AddSql { get { return "p_A_MissingReportInfo"; } }
        #endregion

        public MissingProjectReport() { Init(); ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public MissingProjectReport(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MissingProjectReport(String module) : this() { Module = module; }
        public MissingProjectReport(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MissingProjectReport(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }

        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.MissingProjectReport";
        }

        #region --- Public Methods ---
        public virtual DataSet GetAll()
        {
            try
            {
                if (Module.Length <= 0) { Message = "Module is required."; return null; }
                Hashtable prms = new Hashtable();
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt(GetAllSql, prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile.MissingProjectReport", "GetAll", SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingProjectReport", "EDM.DocFile.MissingProjectReport", ex);
                return null;
            }
        }

        public Boolean Add()
        {
            String logParams = "ProgramId:" + ProgramId + "|ByUserId:" + ByUserId + "|ProjectID:" + ProjectID
                             + "|AdvisorName:" + AdvisorName + "|MeasureName:" + MeasureName + "|Reasons:" + Reasons;
                            
            try
            {
                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                if (ProjectID > 0)  prms["ProjectID"] = ProjectID;
                if (AdvisorID > 0) prms["AdvisorID"] = AdvisorID;
                if (MeasureID > 0) prms["MeasureID"] = MeasureID;
                prms["MeasureName"] = MeasureName;
                prms["Reasons"] = Reasons;            
                if (ByUserId > 0) prms[EDM.Setting.Fields.ByUserID] = ByUserId;

                Db.SetSql(AddSql, prms);
                Lg.Info("MissingProjectReport: Add", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("MissingProjectReport: Add", Db.SqlStmt + "|" + Message);
                    return false;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                ReportMissingID = SqlDb.CheckLongDBNull(dr["ReportMissingID"]);
                if (ReportMissingID <= 0) { Message = MsSql.CheckStringDBNull(dr["Message"]); }
                return ReportMissingID > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("MissingProjectReport: Add", ex, logParams);
                return false;
            }
        }

        public Boolean Delete()
        {
            String logParams = "ProjectID:" + ProjectID;

            try
            {
                if (ProjectID <= 0) { Message = "ProjectID is required"; return false; }                
                Hashtable prms = new Hashtable();
                prms["ProjectID"] = ProjectID;                                                      

                Db.SetSql("p_D_MissingReportInfo", prms);
                Lg.Info("Delete", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error deleting record.";
                    Lg.Info("Delete", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return SqlDb.CheckLongDBNull(dr["ProjectID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Delete", ex, logParams);
                return false;
            }
        }

        #endregion --- Public Methods --- 
    }
}
