using EDM.ContentHandler;
using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class Type
    {
        public const int eScoreReport = 1;
        public const int Lpc = 21;
        public const int Contractor = 106;
        public const int Rebate = 107;
        public const int ISOrder = 110;
        public const int Application = 12000;
        /* Dec 18, 2017 | Nibha Kothari | ES-4336: PGEBGDR: AP: Edit Customer: Documents link */
        public const int EnergyReductionPlan = 160;
        public const int ProjectDocuments = 14000;
        public const int CustomerSiteDocuments = 14447;
        public const int HUPPIPDocuments = 15000;
        public const int WorkOrderDocuments = 15001;
        public const int ReliefFundDocuments = 15002;
        //public const int ReliefFundDocNonProfitCertificate = 15003;
        public const int GCSolarOtherDocuments = 15682;
        public const int QuickQuoteDocuments = 30014;
        public const int UserDocs = 30027;
        public const int ContributionFormDocs = 30041;
    }

    public class DocFile
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public String ConfigKey = String.Empty;
        public long ProgramId;

        public SqlDb Db;
        public Common.Log Lg;

        public EDM.Group.TypeId GroupId;
        public long DocFileId;
        public long DocTypeId;
        public long OldObjectId;
        public long ObjectId;
        public int ObjectType;
        public String OriginalName;
        public String SystemName;
        public String Location;
        public String Storage;
        public String Comments;
        public int StatusId = 1;
        public long UserId;
        public long ByUserId;
        public long PDTID;
        public String TranDate;
        public int IsUploaded;
        public string DocExpiryDate;
        public string DocTitle;  
        public int AccessType = 0;
        public int LPCAccess = 0;
        public string DocumentType;
        public long CustomerLoginID;
        public String SiteID;
        public string DocTypeName;
        public long DocTypeGroupId;
        public virtual String AddSql { get { return "p_AU_DocFiles"; } }
        #endregion

        #region --- Constructors ---
        public DocFile() { ProgramId = Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; Init(); }
        public DocFile(String module) : this() { Module = module; }
        public DocFile(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public DocFile(String module, long docFileId) : this(module) { DocFileId = docFileId; }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.DocFile";
        }
        #endregion --- Private Methods ---

        #region --- Methods ---
        /// <summary>
        /// Module, GroupId are required.
        /// </summary>
        public DataSet GetTypes()
        {
            String logParams = "GroupId:" + GroupId;
            try
            {
                if (GroupId <= 0) { Message = "GroupId is required."; return null; }

                EDM.DocType.DocType dt = new DocType.DocType(Module, ConfigKey, ProgramId);
                return dt.GetByGroupId(GroupId);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetTypes", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Module, DocTypeId, ObjectId, ObjectType are required. StatusId is assumed as 1 unless otherwise specified.
        /// </summary>
        public DataSet GetAll()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectID"] = ObjectId;
                prms["StatusID"] = StatusId;
                if(!String.IsNullOrEmpty(SiteID)) prms["CustomerSAUniqueID"] = SiteID;

                Db.SetSql("p_GET_DocFiles", prms);
                Lg.Info("GetAll", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex, logParams);
                return null;
            }
        }
        public Boolean GetContractDocuments()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectID"] = ObjectId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_Get_ContractDocFiles", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetContractDocuments", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetContractDocuments", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                DocFileId = MsSql.CheckLongDBNull(dr["DocFileId"]);
                DocTypeId = MsSql.CheckLongDBNull(dr["DocTypeId"]);
                ObjectType = MsSql.CheckIntDBNull(dr["ObjectType"]);
                ObjectId = MsSql.CheckLongDBNull(dr["ObjectId"]);
                OriginalName = MsSql.CheckStringDBNull(dr["OriginalName"]);
                SystemName = MsSql.CheckStringDBNull(dr["SystemName"]);
                Location = MsSql.CheckStringDBNull(dr["Location"]);
                ByUserId = MsSql.CheckLongDBNull(dr["ByUserID"]);
                return true;

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex, logParams);
                return false;
            }
        }
        public DataSet GetAllByObjectID()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                if(DocTypeId>0) prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                if(ObjectId>0) prms["ObjectID"] = ObjectId;
                prms["StatusID"] = StatusId;
                if (!String.IsNullOrEmpty(SiteID)) prms["CustomerSAUniqueID"] = SiteID;

                Db.SetSql("p_GET_DocFilesbyObjectID", prms);
                Lg.Info("GetAllByObjectID", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllByObjectID", ex, logParams);
                return null;
            }
        }

        public DataSet GetAllforQuickQuotes()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                if (DocTypeId > 0) prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                if (DocTypeGroupId > 0) prms["DocTypeGroupID"] = DocTypeGroupId;
                if (ObjectId > 0) prms["ObjectID"] = ObjectId;
                prms["StatusID"] = StatusId;

                Db.SetSql("p_GET_DocFilesforQuickQuotesV2", prms);
                Lg.Info("GetAllforQuickQuotes", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllforQuickQuotes", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// ObjectType, ObjectId, DocTypeId, OriginalName, SystemName, Location, ByUserId are required.
        /// </summary>
        public virtual Boolean Add()
        {
            String logParams = "ObjectId:" + ObjectId + "|DocTypeId:" + DocTypeId + "|OriginalName:" + OriginalName + "|DocTypeGroupId:" + DocTypeGroupId
                + "|SystemName:" + SystemName + "|Location:" + Location + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                if (DocFileId > 0) prms["DocFileID"] = DocFileId;
                if (DocTypeId > 0) prms["DocTypeID"] = DocTypeId;
                if (ObjectType > 0) prms["ObjectType"] = ObjectType;
                if (DocTypeGroupId > 0) prms["DocTypeGroupID"] = DocTypeGroupId;
                prms["ObjectID"] = ObjectId;
                prms["OriginalName"] = OriginalName;
                prms["SystemName"] = SystemName;
                prms["Location"] = Location;
                prms["Storage"] = Storage;
                //if(!string.IsNullOrEmpty(Comments)) prms["Comments"] = Comments;
                prms["ByUserID"] = ByUserId;
                prms["DocTitle"] = DocTitle;
                prms["Comments"] = Comments;
                prms["UserId"] = UserId;
               if(AccessType > 0) prms["AccessType"] = AccessType;
                //GJ DSM-5407: GreenConnect : Admin - GPP Contracts - Document Access
                if (LPCAccess > 0) prms["LPCAccess"] = LPCAccess;  
                if (!string.IsNullOrEmpty(DocumentType)) prms["DocumentType"] = DocumentType;
                //GJ DSM-5407: GreenConnect : Admin - GPP Contracts - Document Access

                Db.SetSql(AddSql, prms);
                Lg.Info("Add", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("Add", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DocFileId = SqlDb.CheckIntDBNull(dr["DocFileID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return DocFileId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Add", ex, logParams);
                return false;
            }
        }

        public  Boolean AddHistory()
        {
            String logParams = "ObjectId:" + ObjectId +  "|OriginalName:" + ProgramId + "|ProgramId:" + "|ByUserId:" + ByUserId;

            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ObjectId <= 0) { Message = "ObjectId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["ObjectID"] = ObjectId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_A_History", prms);
                Lg.Info("AddHistory", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("AddHistory", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                ObjectId = SqlDb.CheckLongDBNull(dr["ObjectID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return ObjectId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("AddHistory", ex, logParams);
                return false;
            }
        }
        /// <summary>
        /// Module, DocFileId, ByUserId are required.
        /// </summary>
        public Boolean Delete(Boolean deletePhysicalFile = true)
        {
            String logParams = "DocFileID:" + DocFileId + "|ByUserID:" + ByUserId + "|DeletePhysicalFile:" + deletePhysicalFile;
            try
            {
                if (DocFileId <= 0) { Message = "DocFileId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocFileID"] = DocFileId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_D_DocFiles", prms);
                Lg.Info("Delete", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error deleting file.";
                    Lg.Info("Delete", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                try
                {
                    if (deletePhysicalFile)
                    {
                        EDM.Setting.DB stg = new Setting.DB(ConfigKey, ProgramId);

                        Location = SqlDb.CheckStringDBNull(dr[EDM.Setting.Fields.Location]);

                        FileFactory fileFactory = new FileHandlerCreator(Module);
                        IFileHandler fileHndl = fileFactory.GetFileDeleteInstance(dr[EDM.Setting.Fields.Storage].ToString());
                        bool isDeleted = fileHndl.DeleteFile(Location);
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    Lg.Info("Delete", Message);
                }

                return SqlDb.CheckLongDBNull(dr["DocFileID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Delete", ex, logParams);
                return false;
            }
        }

        public DataSet GetAllDOCFileHistory()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                //prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectID"] = ObjectId;
                if (!String.IsNullOrEmpty(SiteID)) prms["CustomerSAUniqueID"] = SiteID;

                Db.SetSql("p_GET_DOCFileHistory", prms);
                Lg.Info("GetAllDOCFileHistory", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllDOCFileHistory", ex, logParams);
                return null;
            }
        }
        public DataSet GetAllDOCFileHistoryByObjectID()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                //prms["ObjectID"] = ObjectId;
                if (!String.IsNullOrEmpty(SiteID)) prms["CustomerSAUniqueID"] = SiteID;

                Db.SetSql("p_GET_DOCFileHistoryByObjectID", prms);
                Lg.Info("GetAllDOCFileHistory", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllDOCFileHistory", ex, logParams);
                return null;
            }
        }
        public virtual Boolean UpdateComment()
        {
            String logParams = "DocFileID:" + DocFileId + "Comments:" + Comments + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                if (DocFileId <= 0) { Message = "DocFileID is required."; return false; }
                prms["DocFileID"] = DocFileId;
                if (!string.IsNullOrEmpty(Comments)) prms["Comments"] = Comments;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_U_DocFilesComment", prms);
                Lg.Info("UpdateComment", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("UpdateComment", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DocFileId = SqlDb.CheckIntDBNull(dr["DocFileID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return DocFileId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("UpdateComment", ex, logParams);
                return false;
            }
        }
        public Boolean GetById()
        {
            String logParams = "DocFileID:" + DocFileId + "|ByUserId:" + ByUserId;
            try
            {
                if (DocFileId <= 0) { Message = "DocFileId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocFileID"] = DocFileId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DocFile", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetById", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetById", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                DocFileId = MsSql.CheckLongDBNull(dr["DocFileId"]);
                DocTypeId = MsSql.CheckLongDBNull(dr["DocTypeId"]);
                ObjectType = MsSql.CheckIntDBNull(dr["ObjectType"]);
                ObjectId = MsSql.CheckLongDBNull(dr["ObjectId"]);
                OriginalName = MsSql.CheckStringDBNull(dr["OriginalName"]);
                SystemName = MsSql.CheckStringDBNull(dr["SystemName"]);
                Location = MsSql.CheckStringDBNull(dr["Location"]);
                Comments = MsSql.CheckStringDBNull(dr["Comments"]);
                StatusId = MsSql.CheckIntDBNull(dr["StatusID"]);
                ByUserId = MsSql.CheckLongDBNull(dr["ByUserID"]);
                TranDate = MsSql.CheckStringDBNull(dr["TranDate"]);
                Storage = MsSql.CheckStringDBNull(dr["Storage"]);
                IsUploaded = MsSql.CheckIntDBNull(dr["IsUploaded"]);
                DocTitle = MsSql.CheckStringDBNull(dr["DocTitle"]);
                AccessType = MsSql.CheckIntDBNull(dr["AccessType"]);
                LPCAccess = MsSql.CheckIntDBNull(dr["LPCAccess"]);
                DocumentType = MsSql.CheckStringDBNull(dr["DocumentType"]);
                DocTypeName = MsSql.CheckStringDBNull(dr["DocTypeName"]);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.DocFile", "GetById", ex, logParams);
                return false;
            }
        }
        public DataSet GetGCContractByObjectId()
        {
            String logParams = "ObjectId:" + ObjectId ;
            try
            {
                if (ObjectId <= 0) { Message = "ObjectId is required."; return null;}

                Hashtable prms = new Hashtable();
                prms["ObjectId"] = ObjectId;
                prms["ProgramID"] = ProgramId;
                prms["ObjectType"] = ObjectType;
                if (ByUserId > 0) prms["ByUserID"] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_GCContractByObjectID", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetGCContractByObjectId", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetGCContractByObjectId", SqlforLog + "|" + Message);
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.DocFile", "GetGCContractByObjectId", ex, logParams);
                return null;
            }
        }
        public DataSet GetSitesDoc4Customer(long CustomerLoginID)
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["CustomerLoginID"] = CustomerLoginID;
                prms["ProgramID"] = ProgramId;
                if(AccessType > 0) prms["AccessType"] = AccessType;
                prms["StatusID"] = StatusId;

                Db.SetSql("p_GET_SitesDoc4Customer", prms);
                Lg.Info("GetAll", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex, logParams);
                return null;
            }
        }

        public DataSet GetSitesGCDoc4Customer(long CustomerLoginID)
        {
            String logParams = "ProgramID:" + ProgramId + "|CustomerLoginID:" + CustomerLoginID;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CustomerLoginID"] = CustomerLoginID;
                prms["ProgramID"] = ProgramId;          

                Db.SetSql("p_GET_SiteGCDoc4Customer", prms);
                Lg.Info("GetAll", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetSitesGCDoc4Customer", ex, logParams);
                return null;
            }
        }

        public DataSet GetAllCustomerSiteDocs()
        {
            String logParams = "ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                Hashtable prms = new Hashtable();    
                prms["ObjectID"] = ObjectId;
                prms["StatusID"] = StatusId;
                prms["ProgramID"] = ProgramId;

                Db.SetSql("p_GET_CustSiteDocFiles", prms);
                Lg.Info("GetAllCustomerSiteDocs", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllCustomerSiteDocs", ex, logParams);
                return null;
            }
        }

        public DataSet GetAllCustDOCFileHistory()
        {
            String logParams = "ObjectId:" + ObjectId + "|ProgramId:" + ProgramId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["ObjectID"] = ObjectId;              
                prms["ProgramID"] = ProgramId;

                Db.SetSql("p_GET_CustSiteDOCFileHistory", prms);
                Lg.Info("GetAllCustDOCFileHistory", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllCustDOCFileHistory", ex, logParams);
                return null;
            }
        }
        public DataSet GetDPPContractDocs()
        {
            String logParams = "ObjectId:" + ObjectId;
            try
            {
                if (ObjectId <= 0) { Message = "ObjectId is required."; return null; ; }

                Hashtable prms = new Hashtable();
                prms["ObjectId"] = ObjectId;
                prms["ProgramID"] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DPPContractDocs", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetDPPContractDocs", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetDPPContractDocs", SqlforLog + "|" + Message);
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.DocFile", "GetDPPContractDocs", ex, logParams);
                return null;
            }
        }

        public DataSet GetDPPContractDocsByLogin()
        {
            String logParams = "CustomerLoginID:" + CustomerLoginID;
            try
            {
                if (CustomerLoginID <= 0) { Message = "CustomerLoginID is required."; return null; ; }

                Hashtable prms = new Hashtable();
                prms["CustomerLoginID"] = CustomerLoginID;
                prms["ProgramID"] = ProgramId;
                prms["ObjectId"] = ObjectId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DPPContractDocsByLogin", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetDPPContractDocsByLogin", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetDPPContractDocsByLogin", SqlforLog + "|" + Message);
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.DocFile", "GetDPPContractDocsByLogin", ex, logParams);
                return null;
            }
        }

        public DataSet GetSurveyDocs()
        {
            String logParams = "ObjectId:" + ObjectId + " |ObjectType: " + ObjectType + " |DocTypeID: " + DocTypeId;
            try
            {
                if (ObjectId <= 0) { Message = "ObjectId is required."; return null; }
                if (ObjectType <= 0) { Message = "ObjectType is required."; return null; }
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ObjectID"] = ObjectId;
                prms["ObjectType"] = ObjectType;
                prms["DocTypeID"] = DocTypeId;
                prms["ProgramID"] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SurveyDocs", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile", "GetSurveyDocs", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.DocFile", "GetSurveyDocs", SqlforLog + "|" + Message);
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.DocFile", "GetSurveyDocs", ex, logParams);
                return null;
            }
        }

        public DataSet GetAllEPBDocs()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectID"] = ObjectId;           


                Db.SetSql("p_GET_EPBDocs", prms);
                Lg.Info("GetAllEPBDocs", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllEPBDocs", ex, logParams);
                return null;
            }
        }

        public DataSet GetEPBDocFileHistory()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectType:" + ObjectType + "|ObjectId:" + ObjectId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectID"] = ObjectId;

                Db.SetSql("p_GET_EPBDOCFileHistory", prms);
                Lg.Info("GetEPBDocFileHistory", Db.SqlStmt);

                return Db.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetEPBDocFileHistory", ex, logParams);
                return null;
            }
        }
        #endregion
    }
}
