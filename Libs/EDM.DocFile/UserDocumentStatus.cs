using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class UserDocumentStatus
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
        public long UserDocumentStatusId;
        public long UserId;
        public long DocTypeId;
        public String Status;
        public long ByUserId;
        public long ProgramId;
        public long HistoryKey = 0;
        public String ObjectType;
        public String HistoryData;
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
        public UserDocumentStatus() { Init(); }
        public UserDocumentStatus(String module) : this() { Module = module; }
        public UserDocumentStatus(String module, long userId, long docTypeId) : this(module) { UserId = userId; DocTypeId = docTypeId; Module = module; }
        public UserDocumentStatus(String module, String configKey, long programId, long userId, long docTypeId) : this(module) 
        { UserId = userId; ProgramId = programId; ConfigKey = configKey; DocTypeId = docTypeId; Module = module; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, UserId, DocTypeId are required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "UserId:" + UserId + "|DocTypeId:" + DocTypeId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["UserID"] = UserId;
                prms["DocTypeID"] = DocTypeId;

                Db.SetSql("p_GET_UserDocumentStatus", prms);
                Common.Log.Info(Module + ":EDM.DocFile.UserDocumentStatus", "GetById", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();

                //String sql = MsSql.GetSqlStmt("p_GET_DocumentStatus", prms);
                //Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "GetById", sql);

                //DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Record not found."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                UserDocumentStatusId = MsSql.CheckLongDBNull(dr["UserDocumentStatusID"]);
                Status = MsSql.CheckStringDBNull(dr["Status"]);

                return true;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.UserDocumentStatus", "GetById", ex, logParams);
                return false; 
            }
        }

        /// <summary>
        /// Module, UserId, DocTypeId, Status, ByUserId are required.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "UserId:" + UserId + "|DocTypeId:" + DocTypeId + "|Status:" + Status + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                prms["UserID"] = UserId;
                prms["DocTypeID"] = DocTypeId;
                prms["Status"] = Status;
                prms["ByUserID"] = ByUserId;
                prms[EDM.Setting.Fields.ObjectType] = ObjectType;
                prms[EDM.Setting.Fields.HistoryData] = HistoryData;
                prms[EDM.Setting.Fields.HistoryKey] = HistoryKey;
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;

                Db.SetSql("p_AU_UserDocumentStatus", prms);
                Common.Log.Info(Module + ":EDM.DocFile.UserDocumentStatus", "Save", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();

                //String sql = MsSql.GetSqlStmt("p_AU_DocumentStatus", prms);
                //Common.Log.Info(Module + ":EDM.DocFile.DocumentStatus", "Save", sql);

                //DataSet ds = MsSql.ExecuteQuery(sql);                
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                UserDocumentStatusId = MsSql.CheckLongDBNull(dr["UserDocumentStatusID"]);
                return UserDocumentStatusId > 0 ? true : false;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.UserDocumentStatus", "Save", ex, logParams);
                return false; 
            }
        }
        #endregion
    }
}
