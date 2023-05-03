using System;
using System.Collections;
using System.Data;
using EDM.ContentHandler;
using VTI.Common;

namespace EDM.DocFile
{
    public class MissingSignFile
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
                Lg.ModuleName = Module + ":EDM.DocFile.MissingSignFile";
            }
        }

        public String Module;
        public String Message;
        public int PKID;
        public String SystemName;
        public String RelLocation;
        public long ProgramIncentiveID;
        public String AdvisorName;
        public String Storage;
        #endregion --- Properties ---

        #region --- Abstract Properties --- 
        protected virtual String UpdateSignImagesFlagSql { get { return "p_U_SignImagesPhysicalFlagInfo"; } }
        protected virtual String GetAllSql { get { return "p_AP_GET_AllSignFileInfo"; } }
        #endregion
     
        public MissingSignFile() { Init(); ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public MissingSignFile(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MissingSignFile(String module) : this() { Module = module; }
        public MissingSignFile(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MissingSignFile(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }

        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.MissingSignFile";
        }

        #region --- Public Methods ---
        public virtual DataSet GetAll()
        {
            try
            {
                if (Module.Length <= 0) { Message = "Module is required."; return null; }
                Hashtable prms = new Hashtable();
                prms["FileDurationKey"] = "CSGFileCheckApp_Report_GetFileDuration";
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt(GetAllSql, prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile.MissingSignFile", "GetAll", SqlforLog); 
                return MsSql.ExecuteNoTransQuery(sql);              
                                
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.DocFile.MissingSignFile", "EDM.DocFile.MissingSignFile", ex); return null; }
        }      
        public void FindMissingSignInImageFiles(DataSet ds)
        {
            try
            {
                String customerSAId, fileName, relLocation, filePath, projectId, advisorName, Storage, errorMessage = String.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    customerSAId = SqlDb.CheckStringDBNull(dr["PKID"]);
                    fileName = SqlDb.CheckStringDBNull(dr["SystemName"]);
                    relLocation = SqlDb.CheckStringDBNull(dr["RelLocation"]);
                    projectId = SqlDb.CheckStringDBNull(dr["ProjectID"]);
                    advisorName = SqlDb.CheckStringDBNull(dr["AdvisorName"]);
                    Storage = SqlDb.CheckStringDBNull(dr["Storage"]);

                    filePath = relLocation + fileName;
                    FileFactory fileFactory = new FileHandlerCreator(Module, FileLocationType.DefaultUploadLocation, ConfigKey);
                    IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                    bool isExist = fileHndl.IsFileExists(filePath);
                    if (isExist)
                    {
                        long CustomerSAID = SqlDb.CheckLongDBNull(dr["PKID"]);
                        String logInfo = "## Update missing sign image file flag";
                        logInfo = logInfo + " of CustomerSAID" + CustomerSAID + "|ProjectID:" + projectId + "|SystemName:" + fileName + "|filePath:" + filePath + " ##";
                        UpdateSignInImageMissingFileFlag(CustomerSAID, true);
                    }
                }                
            }
            catch (Exception ex)
            {
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingSignFile", "FindMissingSignInImageFiles", ex);                    
            }
        }   
        public Boolean UpdateSignInImageMissingFileFlag(long CustomerSAIID, bool IsPhysicalFile)
        {
            String logParams = "CustomerSAIID:" + CustomerSAIID + "|IsPhysicalFile:" + IsPhysicalFile;

            try
            {
                Hashtable prms = new Hashtable();
                prms["CustomerSAIID"] = CustomerSAIID;
                if (IsPhysicalFile) { prms["IsPhysicalFile"] = 1; } else { prms["IsPhysicalFile"] = 0; }
                Db.SetSql(UpdateSignImagesFlagSql, prms);                              
                Common.Log.Info(Module + ":EDM.DocFile.MissingImageFile", "UpdateSignInImageMissingFileFlag", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                int UpdateStatusId = SqlDb.CheckIntDBNull(dr["UpdateStatus"]);
                return UpdateStatusId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingSignFile", "UpdateSignInImageMissingFileFlag", ex, logParams);
                return false;
            }
        }

        #endregion --- Public Methods ---   
    }
}
