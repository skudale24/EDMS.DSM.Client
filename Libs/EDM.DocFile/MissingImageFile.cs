using System;
using System.Collections;
using System.Data;
using EDM.ContentHandler;
using VTI.Common;

namespace EDM.DocFile
{
    public class MissingImageFile
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
                Lg.ModuleName = Module + ":EDM.DocFile.MissingImageFile";
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
        protected virtual String UpdateImagesFlagSql { get { return "p_U_ImagePhysicalFlagInfo"; } }
        protected virtual String GetAllSql { get { return "p_AP_GET_AllImageFileInfo"; } }
        #endregion

        public MissingImageFile() { Init(); ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public MissingImageFile(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MissingImageFile(String module) : this() { Module = module; }
        public MissingImageFile(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MissingImageFile(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }

        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.MissingImageFile";
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
                Common.Log.Info(Module + ":EDM.DocFile.MissingImageFile", "GetAll", SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);            
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingImageFile", "EDM.DocFile.MissingImageFile", ex);
                return null;
            }
        }
        public void FindMissingImageFiles(DataSet ds)
        {
            try
            {
                String projectImageId, fileName, relLocation, filePath, projectId, advisorName, Storage, errorMessage = String.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    projectImageId = SqlDb.CheckStringDBNull(dr["PKID"]);
                    fileName = SqlDb.CheckStringDBNull(dr["SystemName"]);
                    relLocation = SqlDb.CheckStringDBNull(dr["RelLocation"]);
                    projectId = SqlDb.CheckStringDBNull(dr["ProjectID"]);
                    advisorName = SqlDb.CheckStringDBNull(dr["AdvisorName"]);
                    Storage = SqlDb.CheckStringDBNull(dr["Storage"]);

                    filePath = relLocation + fileName;
                    FileFactory fileFactory = new FileHandlerCreator(Module, FileLocationType.DefaultUploadLocation, ConfigKey);
                    IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                    long ProjectImageID = SqlDb.CheckLongDBNull(dr["PKID"]);
                    bool isExist = fileHndl.IsFileExists(filePath);
                    if (isExist)
                    {
                        String logInfo = "## Update missing image file flag";
                        logInfo = logInfo + "of ProjectImageID" + ProjectImageID + "|SystemName:" + fileName + "|filePath:" + filePath + " ##";
                        Common.Log.Info(Module, Module + ":EDM.DocFile.MissingImageFile", "FindMissingImageFiles", logInfo);
                        UpdateMissingFileFlag(ProjectImageID, true);                         
                    }                   
                }              
            }
            catch (Exception ex)
            {
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingImageFile", "FindMissingImageFiles", ex);                
            }
        }
        public Boolean UpdateMissingFileFlag(long ProjectImageID, bool IsPhysicalFile)
        {
            String logParams = "ProjectImageID:" + ProjectImageID + "|IsPhysicalFile:" + IsPhysicalFile;

            try
            {
                Hashtable prms = new Hashtable();
                prms["ProjectImageID"] = ProjectImageID;
                if (IsPhysicalFile) { prms["IsPhysicalFile"] = 1; } else { prms["IsPhysicalFile"] = 0; }

                Db.SetSql(UpdateImagesFlagSql, prms);
                Common.Log.Info(Module + ":EDM.DocFile.MissingImageFile", "UpdateMissingFileFlag", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                int UpdateStatusId = MsSql.CheckIntDBNull(dr["UpdateStatus"]);
                return UpdateStatusId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingImageFile", "UpdateMissingFileFlag", ex, logParams);
                return false;
            }
        }              
       
        #endregion --- Public Methods ---   
    }
}
