using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class DocumentStatus
    {
        public enum Id
        {
            Valid = 1,
            Invalid = 2,       
            Uploaded = 3,
            Expired = 4
        }

        #region --- Properties ---
        public String Message;
        public String Module;
        public long DocumentStatusId;
        public long CompanyId;
        public long DocTypeId;
        public String Status;
        public long ByUserId;
        public long ProgramId;
        private SqlDb Db;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                //Lg = new Log(_configKey);
                //Lg.ModuleName = Module + ":EDM.Common.UtilityAccess";
            }
        }
        #endregion
        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            //Lg = new Common.Log(ConfigKey);
            //Lg.ModuleName = Module + ":EDM.Badge.Badge";
        }
        #region --- Constructors ---
        public DocumentStatus() { Init(); }
        public DocumentStatus(String module) : this() { Module = module; }
        public DocumentStatus(String module, long companyId, long docTypeId) : this(module) { CompanyId = companyId; DocTypeId = docTypeId; Module = module; }
        public DocumentStatus(String module, String configKey, long programId, long companyId, long docTypeId) : this(module) 
        { CompanyId = companyId; ProgramId = programId; ConfigKey = configKey; DocTypeId = docTypeId; Module = module; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, CompanyId, DocTypeId are required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "CompanyId:" + CompanyId + "|DocTypeId:" + DocTypeId + "|ProgramId:" + ProgramId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CompanyID"] = CompanyId;
                prms["DocTypeID"] = DocTypeId;
                prms["ProgramID"] = ProgramId;

                //DSM-5063, DSM-5138 GJ
                Db.SetSql("p_GET_DocumentStatus", prms);
                Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "GetById", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();

                //String sql = MsSql.GetSqlStmt("p_GET_DocumentStatus", prms);
                //Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "GetById", sql);

                //DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Record not found."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                DocumentStatusId = MsSql.CheckLongDBNull(dr["DocumentStatusID"]);
                Status = MsSql.CheckStringDBNull(dr["Status"]);

                return true;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.DocumentStatus", "GetById", ex, logParams);
                return false; 
            }
        }

        /// <summary>
        /// Module, CompanyId, DocTypeId, Status, ByUserId are required.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "CompanyId:" + CompanyId + "|DocTypeId:" + DocTypeId + "|Status:" + Status + "|ByUserId:" + ByUserId + "|ProgramId:" + ProgramId;

            try
            {
                Hashtable prms = new Hashtable();
                prms["CompanyID"] = CompanyId;
                prms["DocTypeID"] = DocTypeId;
                prms["Status"] = Status;
                prms["ByUserID"] = ByUserId;
                prms["ProgramID"] = ProgramId;

                //DSM-5063,DSM-5138 GJ
                Db.SetSql("p_AU_DocumentStatus", prms);
                Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "Save", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();

                //String sql = MsSql.GetSqlStmt("p_AU_DocumentStatus", prms);
                //Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "Save", sql);

                //DataSet ds = MsSql.ExecuteQuery(sql);                
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                DocumentStatusId = MsSql.CheckLongDBNull(dr["DocumentStatusID"]);
                return DocumentStatusId > 0 ? true : false;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.DocumentStatus", "Save", ex, logParams);
                return false; 
            }
        }
        #endregion
    }
}
