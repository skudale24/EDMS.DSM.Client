using System;
using System.Collections;
using System.Data;
using EDM.Common;
using VTI.Common;
using EDM.ContentHandler;

namespace EDM.DocFile
{
    public class eScoreReport
    {
        #region --- Constants ---
        public enum ObjectType
        {
            Project = 102,
            ProgramIncentive = 107
        }
        #endregion

        #region --- Properties ---
        public String Module = String.Empty;
        public String Message = String.Empty;
        public String ConfigKey = String.Empty;
        public long ProgramId;

        public SqlDb Db;
        public Common.Log Lg;

        public String DocTypeId;
        public String ObjectTypeId;
        public String ObjectId;
        public String ReportLoc;
        public String Storage;
        public Boolean Track;
        public int StatusId = -1;
        #endregion

        #region --- Constructors ---
        public eScoreReport() { ProgramId = EDM.Setting.Session.ProgramId; Init(); }
        public eScoreReport(String module) : this() { Module = module; }
        public eScoreReport(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public eScoreReport(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new Common.Log(ConfigKey);
            Lg.ModuleName = Module + ":EDM.DocFile.eScoreReport";
        }
        #endregion --- Private Methods ---

        #region --- Public Methods ---
        public String Exists(System.Web.UI.Page pg)
        {
            String logParams = "QueryString:" + pg.Request.RawUrl;

            try
            {
                if (pg.Request.QueryString.Count <= 0) { Message = "Empty query string."; return String.Empty; }

                DocTypeId = String.Empty + pg.Request["dtid"];
                ObjectTypeId = String.Empty + pg.Request["ot"];
                ObjectId = String.Empty + pg.Request["oid"];
                ReportLoc = String.Empty + pg.Request["rl"];
                Storage = String.Empty + pg.Request["stg"];
                Track = String.Empty + pg.Request["track"] == "1" ? true : false;

                if (String.IsNullOrEmpty(DocTypeId) || String.IsNullOrEmpty(ObjectTypeId)
                    || String.IsNullOrEmpty(ObjectId) || String.IsNullOrEmpty(ReportLoc))
                {
                    Message = "Missing query string parameters.";
                    Lg.Info("Exists", logParams + "|" + Message);
                    return String.Empty;
                }

                EDM.Setting.DB stg = new EDM.Setting.DB(ConfigKey, ProgramId);

                FileFactory fileFactory = new FileHandlerCreator(Module);
                IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                bool isExist = fileHndl.IsFileExists(ReportLoc);

                if (isExist)
                {
                    StatusId = 1;
                    Message = "Success";

                    if (Track) SaveStatus();

                    string fileURL = string.Empty;
                    switch (Module)
                    {
                        case EDM.Setting.Module.AdminPortal:
                            fileURL = "~/Controls/FileHandler.ashx";
                            break;
                        case EDM.Setting.Module.ContraPortal:
                            fileURL = "~/Common/Controls/FileHandler.ashx";
                            break;
                        default:
                            fileURL = "~/Common/Controls/FileHandler.ashx";
                            break;
                    }
                    return fileURL +"?storage=" + Storage + "&fileName=" + System.Web.HttpUtility.UrlEncode(ReportLoc) + "&filePath=" + ReportLoc;
                }
                else
                {
                    StatusId = 0;
                    Message = logParams + "|File still missing.";

                    if (Track) SaveStatus();

                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Exists", ex, logParams);
                return String.Empty;
            }
        }

        public DataSet Get4ReminderEmail()
        {
            try
            {
                Db.SetSql("p_GET_eScoreReports4Email", new Hashtable());
                Lg.Info("Get4ReminderEmail", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Lg.Error("Get4ReminderEmail", ex);
                return null;
            }
        }

        public Boolean SendReminderEmails(String template)
        {
            String module = Module + ":EDM.DocFile.eScoreReport";
            try
            {
                int missing = 0, emailsSent = 0, emailsNotSent = 0;

                DataSet ds = Get4ReminderEmail();
                if (MsSql.IsEmpty(ds)) { StatusId = 1; Message = "No records to process"; return true; }

                EDM.Setting.Email cfg = new Setting.Email(ConfigKey, ProgramId);

                String custEmail = String.Empty, body = String.Empty, subj = String.Empty, reportLoc = String.Empty, filePath = String.Empty;
                long objectId = 0;

                Message = String.Empty;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    long LPCID = MsSql.CheckLongDBNull(dr["CompanyID"]);
                    //String LogoIMG = EDM.Common.Helper.GetLPCLogoHtml(LPCID);
                    //if ((ProgramId & (long)EDM.Common.Programs.eScoreWithLI) == ProgramId || (ProgramId & (long)EDM.Common.Programs.eScoreWithLI) == (long)EDM.Common.Programs.eScoreWithLI)
                    //    LogoIMG = EDM.Common.Helper.GetLPCLogo(LPCID);

                    long programModel = MsSql.CheckLongDBNull(dr["ProgramModel"]);
                    if (programModel == EDM.Program.Model.Turnkey)
                        cfg.ObjectId = (int)Setting.Email.Type.CustReportReminder;
                    else if (programModel == EDM.Program.Model.LPCManaged)
                        cfg.ObjectId = (int)Setting.Email.Type.CustReportReminderLPC;
                    cfg.GetById();

                    ///* START |  March 24, 2021 | DSM-3896 */
                    //long ProgramID_Param = 1;
                    //if (ProgramId > 0) { ProgramID_Param = ProgramId; }
                    //EDM.Setting.DB objSetting = new EDM.Setting.DB(ConfigKey, ProgramID_Param);
                    //objSetting.ProgramId = ProgramID_Param;
                    //String domain = objSetting.GetByKey(EDM.Setting.Key.SiteUrl);
                    ///* START |  March 24, 2021 | DSM-3896 */

                    template = cfg.Body;
                    String subject = cfg.Subject;
                    EDM.Common.UtilityAccess _objUtilityAccess = new EDM.Common.UtilityAccess(Module);

                    if (!String.IsNullOrEmpty(template) && cfg.StatusId == EDM.Setting.Status.Active && _objUtilityAccess.ISCustomerAllowedEmail(cfg.ToEmail, LPCID))
                    {
                        custEmail = MsSql.CheckStringDBNull(dr["CustEmail"]);
                        objectId = MsSql.CheckLongDBNull(dr["ObjectID"]);
                        reportLoc = MsSql.CheckStringDBNull(dr["ReportLocation"]);

                        EDM.Setting.DB stg = new EDM.Setting.DB(ConfigKey, ProgramId);

                        String siteUrl = stg.GetByKey(EDM.Setting.Key.CPUrl);
                        String participationAgreement = siteUrl + "/CustomerTerms.aspx?ID=" + MsSql.CheckStringDBNull(dr["Terms"]);

                        FileFactory fileFactory = new FileHandlerCreator(module);
                        IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                        bool isExist = fileHndl.IsFileExists(reportLoc);

                        if (isExist)
                        {
                            body = template;
                            //String EmailImagesUrl = EDM.Common.Helper.GetEmailImagesUrl();
                            //body = body.Replace("%%EMAILIMAGESURL%%", EmailImagesUrl);
                            body = body.Replace("%%CUSTOMERNAME%%", MsSql.CheckStringDBNull(dr["CustName"]));
                            body = body.Replace("%%REPORTURL%%", GetUrl(MsSql.CheckIntDBNull(dr["ObjectType"]), MsSql.CheckStringDBNull(dr["ObjectID"])
                                , reportLoc, programId: ProgramId, configKey: ConfigKey, Storage: MsSql.CheckStringDBNull(dr["Storage"])));
                            //body = body.Replace("%%CONTRACTOR_URL%%", MsSql.CheckStringDBNull(dr["CPUrl"]));
                            //body = body.Replace("%%RESCH_URL%%", MsSql.CheckStringDBNull(dr["CPUrl"]));
                            //body = body.Replace("%%YEAR%%", DateTime.Today.Year.ToString());
                            body = body.Replace("%%SITEID%%", MsSql.CheckStringDBNull(dr["CustomerSAUniqueID"]));
                            body = body.Replace("%%LOGINID%%", MsSql.CheckStringDBNull(dr["UserLogin"]));
                            body = body.Replace("%%TERMS%%", participationAgreement);
                            body = body.Replace("%%LPC_PH%%", MsSql.CheckStringDBNull(dr["PIPPhone"]));

                            //body = body.Replace("%%LPCLOGO%%", LogoIMG);
                            body = body.Replace("%%LPCNAME%%", MsSql.CheckStringDBNull(dr["CompanyName"]));
                            //body = body.Replace("%%DOMAINC%%", domain);
                            body = cfg.ReplaceGeneralSubstitutionTags(body, lpcId: LPCID);

                            subj = subject.Replace("%%PROJECT_UNIQUE_ID%%", MsSql.CheckStringDBNull(dr["UniqueID"]));

                            Email.MailMessage mm = new Email.MailMessage(ConfigKey, ProgramId);
                            if (!mm.SendWLog(module, cfg, objectId, cfg.ToEmail.Replace("%%CustomerEmail%%", custEmail), body, subj))
                            {
                                emailsNotSent++;
                                Message += "(" + custEmail + ")(" + objectId + ")(" + filePath + ")(" + mm.Message + ")";
                            }
                            else
                            {
                                emailsSent++;
                                StatusId = 1;
                                DocTypeId = MsSql.CheckStringDBNull(dr["DocTypeId"]);
                                ObjectTypeId = MsSql.CheckStringDBNull(dr["ObjectType"]);
                                ObjectId = MsSql.CheckStringDBNull(dr["ObjectId"]);
                                SaveStatus();
                            }
                        }
                        else
                        {
                            missing++;
                            Message += "(" + custEmail + ")(" + objectId + ")(" + filePath + ")(Still missing)";
                        }
                    }
                }

                StatusId = String.IsNullOrEmpty(Message) ? 1 : 0;

                Message = "Missing: " + missing + ", Emails Not Sent: " + emailsNotSent + ", Emails Sent: " + emailsSent
                    + ", Total: " + SqlDb.GetRowCount(ds) + " records processed.";

                return StatusId == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("SendReminderEmails", ex);
                return false;
            }
        }

        public EDM.Common.MethodReturn SendReminder()
        {
            EDM.Common.MethodReturn mr = new EDM.Common.MethodReturn();
            try
            {
                EDM.Setting.Email cfg = new EDM.Setting.Email(ConfigKey, ProgramId);
                cfg.ObjectId = (int)EDM.Setting.Email.Type.CustReportReminder;
                cfg.GetById();

                String template = cfg.Body;
                eScoreReport dfs = new eScoreReport(Module, ConfigKey, ProgramId);
                if (!String.IsNullOrEmpty(template))
                {
                    if (!dfs.SendReminderEmails(template) && dfs.StatusId < 0)
                    {
                        Lg.Error("Process", new Exception(dfs.Message));
                        mr.Status = false;
                    }
                    else
                    {
                        Lg.Info("Process", dfs.Message);
                        mr.Status = true;
                    }
                    mr.Message += dfs.Message;
                }
                
                return mr;
            }
            catch (Exception ex)
            {
                mr.Status = false; mr.Message = ex.Message;
                Lg.Error("SendReminder", ex);
                return mr;
            }
        }
        #endregion

        #region --- Public Static Methods ---
        public static String GetUrl(int objectType, String objectId, String reportLocation, Boolean track = true
           , long programId = 0, String configKey = "", string Storage = "", string Module = "")
        {
            EDM.Common.Log lg = new EDM.Common.Log(configKey);
            lg.ModuleName = "EDM.DocFile.eScoreReport";
            String logParams = "ObjectType:" + objectType + "|ObjectId:" + objectId + "|ReportLocation:" + reportLocation + "|Track:" + track;
            try
            {
                if (objectId.Length <= 0 || reportLocation.Length <= 0) { return String.Empty; }
                if(programId==0) programId = EDM.Setting.Session.ProgramId;

                EDM.Setting.DB stg = new EDM.Setting.DB(configKey, programId);
                string portalURL = string.Empty;
                bool isCP = false;
                switch(Module)
                {
                    case EDM.Setting.Module.AdminPortal:
                        portalURL = stg.GetByKey(Setting.Key.APUrl);
                        break;
                    case EDM.Setting.Module.ContraPortal:
                        portalURL = stg.GetByKey(Setting.Key.CPUrl);
                        isCP = true;
                        break;
                    default:
                        portalURL = stg.GetByKey(Setting.Key.CPUrl);
                        isCP = true;
                        break;
                }
                if (isCP&&((programId & (long)EDM.Common.Programs.eScoreWithLI) == programId || (programId & (long)EDM.Common.Programs.eScoreWithLI) == (long)EDM.Common.Programs.eScoreWithLI))
                {
                    var url = portalURL + "/FileHandler?storage=" + Storage + "&fileName=View_efficiency_report.pdf&filePath=" + reportLocation;
                    url = EDM.Common.Helper.CustomNavigateURL(url);
                    return url;
                }
                return portalURL + "/ViewFile.aspx?"
                    + "&dtid=" + "1"
                    + "&ot=" + objectType
                    + "&oid=" + objectId
                    + "&rl=" + reportLocation
                    + "&stg=" + Storage
                    + "&track=" + (track ? "1" : "0");
            }
            catch (Exception ex)
            {
                lg.Info("GetUrl", logParams + "|Error:" + ex.Message);
                return String.Empty;
            }
        }

        public static String GetInspRptUrl(int objectType, String objectId, String reportLocation, Boolean track = true
          , long programId = 0, String configKey = "", string Storage = "", string Module = "")
        {
            EDM.Common.Log lg = new EDM.Common.Log(configKey);
            lg.ModuleName = "EDM.DocFile.eScoreReport";
            String logParams = "ObjectType:" + objectType + "|ObjectId:" + objectId + "|ReportLocation:" + reportLocation + "|Track:" + track;
            try
            {
                if (objectId.Length <= 0 || reportLocation.Length <= 0) { return String.Empty; }
                if (programId == 0) programId = EDM.Setting.Session.ProgramId;

                EDM.Setting.DB stg = new EDM.Setting.DB(configKey, programId);
                string portalURL = string.Empty;
                bool isCP = false;
                switch (Module)
                {
                    case EDM.Setting.Module.AdminPortal:
                        portalURL = stg.GetByKey(Setting.Key.APUrl);
                        break;
                    case EDM.Setting.Module.ContraPortal:
                        portalURL = stg.GetByKey(Setting.Key.CPUrl);
                        isCP = true;
                        break;
                    default:
                        portalURL = stg.GetByKey(Setting.Key.CPUrl);
                        isCP = true;
                        break;
                }
                if (isCP && ((programId & (long)EDM.Common.Programs.eScoreWithLI) == programId || (programId & (long)EDM.Common.Programs.eScoreWithLI) == (long)EDM.Common.Programs.eScoreWithLI))
                {
                    var url = portalURL + "/FileHandler?storage=" + Storage + "&fileName=View_Inspection_report.pdf&filePath=" + reportLocation;
                    url = EDM.Common.Helper.CustomNavigateURL(url);
                    return url;
                }
                return portalURL + "/ViewFile.aspx?"
                    + "&dtid=" + "1"
                    + "&ot=" + objectType
                    + "&oid=" + objectId
                    + "&rl=" + reportLocation
                    + "&stg=" + Storage
                    + "&track=" + (track ? "1" : "0");
            }
            catch (Exception ex)
            {
                lg.Info("GetInspRptUrl", logParams + "|Error:" + ex.Message);
                return String.Empty;
            }
        }
        #endregion

        #region --- Private Methods ---
        private Boolean SaveStatus()
        {
            String logParams = "DocTypeId:" + DocTypeId + "|ObjectTypeId:" + ObjectTypeId + "|ObjectId:" + ObjectId + "|StatusId:" + StatusId;
            try
            {
                if (String.IsNullOrEmpty(DocTypeId) || String.IsNullOrEmpty(ObjectTypeId) || String.IsNullOrEmpty(ObjectId) || StatusId < 0)
                {
                    Message = "Missing parameters.";
                    Lg.Info("SaveStatus", logParams + "|" + Message);
                    return false;
                }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectTypeId;
                prms["ObjectID"] = ObjectId;
                prms["StatusID"] = StatusId;

                Db.SetSql("p_AU_DocFileStatus", prms);
                Lg.Info("SaveStatus", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error saving record.";
                    Lg.Info("SaveStatus", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                long docFileId = MsSql.CheckLongDBNull(dr["DocFileID"]);
                if (docFileId > 0)
                {
                    StatusId = 1;
                    return true;
                }
                else
                {
                    StatusId = 0;
                    Lg.Error("SaveStatus", new Exception(Message), logParams);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("SaveStatus", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}
