using System;
using System.Collections;
using System.Data;
using EDM.ContentHandler;
using VTI.Common;

namespace EDM.DocFile
{
    public class MissingPdfFile
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
                Lg.ModuleName = Module + ":EDM.DocFile.MissingPdfFile";
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
        protected virtual String UpdatePDFPhysicalFlagSql { get { return "p_U_PDFPhysicalFlagInfo"; } }
        protected virtual String GetAllSql { get { return "p_AP_GET_AllPDFFilesInfo"; } }
        #endregion           

        public MissingPdfFile() { Init(); ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public MissingPdfFile(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MissingPdfFile(String module) : this() { Module = module; }
        public MissingPdfFile(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MissingPdfFile(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }

        protected virtual void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.MissingPdfFile";
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
                Common.Log.Info(Module + ":EDM.DocFile.MissingPdfFile", "GetAll", SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);                               
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.DocFile.MissingPdfFile", "EDM.DocFile.MissingPdfFile", ex); return null; }
        }
        public void FindMissingPDFFiles(DataSet ds)
        {
            try
            {
                String docId, fileName, relLocation, filePath, projectID, advisorName, Storage, errorMessage = String.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    docId = SqlDb.CheckStringDBNull(dr["PKID"]);
                    fileName = SqlDb.CheckStringDBNull(dr["SystemName"]);
                    relLocation = SqlDb.CheckStringDBNull(dr["RelLocation"]);
                    projectID = SqlDb.CheckStringDBNull(dr["ProjectID"]);
                    advisorName = SqlDb.CheckStringDBNull(dr["AdvisorName"]);
                    Storage = SqlDb.CheckStringDBNull(dr["Storage"]);

                    filePath = relLocation + fileName;
                    FileFactory fileFactory = new FileHandlerCreator(Module, FileLocationType.DefaultUploadLocation, ConfigKey);
                    IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                    bool isExist = fileHndl.IsFileExists(filePath);
                    if (isExist)
                    {
                        long DocID = SqlDb.CheckLongDBNull(dr["PKID"]);
                        String logInfo = "## Update missing pdf file flag";
                        logInfo = logInfo + " of DocID" + DocID +"|ProjectID" + projectID + " |fileName:" + fileName + "|filePath:" + filePath + " ##";
                        UpdateMissingPDFFileFlag(DocID, true);
                    }
                }                
            }
            catch (Exception ex)
            {
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingPdfFile", "FindMissingPDFFiles", ex);               
            }
        }
        public Boolean UpdateMissingPDFFileFlag(long DocID, bool IsPhysicalFile)
        {
            String logParams = "DocID:" + DocID + "|IsPhysicalFile:" + IsPhysicalFile;

            try
            {
                Hashtable prms = new Hashtable();
                prms["DocFileID"] = DocID;
                if (IsPhysicalFile) { prms["IsPhysicalFile"] = 1; } else { prms["IsPhysicalFile"] = 0; }
                Db.SetSql(UpdatePDFPhysicalFlagSql, prms);
                Common.Log.Info(Module + ":EDM.DocFile.MissingPdfFile", "UpdateMissingFileFlag", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                int UpdateStatusId = SqlDb.CheckIntDBNull(dr["UpdateStatus"]);
                return UpdateStatusId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.MissingPdfFile", "UpdateMissingPDFFileFlag", ex, logParams);
                return false;
            }
        }
        #endregion --- Public Methods ---  
    }
}
