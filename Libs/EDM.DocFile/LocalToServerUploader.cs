using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;    
using VTI.Common;
using EDM.ContentHandler;
using System.Data;
using System.Collections;
using System.IO;

namespace EDM.DocFile
{
    public class LocalToServerUploader
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
        public LocalToServerUploader() { Init(); }
        public LocalToServerUploader(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public LocalToServerUploader(String module) : this() { Module = module; }
        public LocalToServerUploader(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public LocalToServerUploader(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new EDM.Common.Log(ConfigKey);
            Lg.ModuleName = configKey + ".Service.LocalToServerUploader";
        }
        #endregion --- Private Methods ----

        #region --- Public Methods ---
        public EDM.Common.MethodReturn ProcessLocalToServerUploader()
        {
            EDM.Common.MethodReturn mr = new EDM.Common.MethodReturn();
            try
            {
                string Message = string.Empty;
                UploadFiles(Lg, ConfigKey, ProgramId);
                Message = "LocalToServerUploader processed done.";
                mr.Message = Message;
                mr.Status = true;
                return mr;
            }
            catch (Exception ex)
            {
                mr.Status = false;
                mr.Message = ex.Message;
                Lg.Error("ProcessLocalToServerUploader", new Exception(ex.Message));
                return mr;
            }
        }
        #endregion

        #region ---Private Method ---
        private void UploadFiles(EDM.Common.Log lg, String configKey, long programId)
        {
            try
            {
                EDM.Setting.DB stg = new EDM.Setting.DB(configKey, programId);
                int hours = Convert.ToInt32(stg.GetByKey(EDM.Setting.Key.LocalFile2ServerHRS));

                List<DocLocation> lstDocFiles_NotFound = new List<DocLocation>();
                List<DocLocation> lstProjectImages_NotFound = new List<DocLocation>();
                List<DocLocation> lstCustomerSAImages_NotFound = new List<DocLocation>();

                EDM.ContentHandler.MeasureDocs md = new EDM.ContentHandler.MeasureDocs(Module, configKey, programId);
                DataSet ds = md.GetFileList2Upload(hours);
                if (ds != null)
                {
                    #region ---Table DocFiles---    
                    long DocTypeId = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DocLocation objDocLocation = new DocLocation();
                        bool isExist = IsFileExist(lg, dr, ref objDocLocation, "DocFileID", "Location", configKey);
                        if (isExist)
                        {
                            bool isUpdate = upload(lg, DocTypeId, ref objDocLocation, configKey, programId);
                            if (isUpdate)
                            {
                                //Update to DB
                                int ISUploaded = 9;//9=File Uploaded to server
                                md.UpdateDocFiles(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                            }
                            else
                            {
                                //fail log
                                string logParams = "ObjectID:" + objDocLocation.ObjectID + "|Location:" + objDocLocation.Location + "|SystemName:" + objDocLocation.SystemName + "|Storage:" + objDocLocation.Storage;
                                lg.Info("LocalToServerUploader:DocFiles", logParams);
                            }
                        }
                        else
                        {
                            lstDocFiles_NotFound.Add(objDocLocation);
                            // update in db, its not found
                            int ISUploaded = SqlDb.CheckIntDBNull(dr[EDM.Setting.Fields.ISUploaded]) + 1;
                            md.UpdateDocFiles(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                        }
                    }
                    #endregion ---Table DocFiles---

                    #region ---Table ProjectImages---
                    DocTypeId = 0;
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DocLocation objDocLocation = new DocLocation();
                        bool isExist = IsFileExist(lg, dr, ref objDocLocation, "ProjectImageID", "RelLocation", configKey);
                        if (isExist)
                        {
                            bool isUpdate = upload(lg, DocTypeId, ref objDocLocation, configKey, programId);
                            if (isUpdate)
                            {
                                //Update to DB
                                int ISUploaded = 9;//9=File Uploaded to server
                                md.UpdateProjectImage(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                            }
                            else
                            {
                                //fail log
                                string logParams = "ObjectID:" + objDocLocation.ObjectID + "|Location:" + objDocLocation.Location + "|SystemName:" + objDocLocation.SystemName + "|Storage:" + objDocLocation.Storage;
                                lg.Info("LocalToServerUploader:ProjectImages", logParams);
                            }
                        }
                        else
                        {
                            lstProjectImages_NotFound.Add(objDocLocation);
                            // update in db, its not found
                            int ISUploaded = SqlDb.CheckIntDBNull(dr[EDM.Setting.Fields.ISUploaded]) + 1;
                            md.UpdateProjectImage(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                        }
                    }
                    #endregion ---Table ProjectImages---

                    #region ---Table CustomerSAImages---
                    DocTypeId = 0;
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        DocLocation objDocLocation = new DocLocation();
                        bool isExist = IsFileExist(lg, dr, ref objDocLocation, "CustomerSAIID", "RelLocation", configKey);
                        if (isExist)
                        {
                            bool isUpdate = upload(lg, DocTypeId, ref objDocLocation, configKey, programId);
                            if (isUpdate)
                            {
                                //Update to DB
                                int ISUploaded = 9;//9=File Uploaded to server
                                md.UpdateCustomerSAImages(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                            }
                            else
                            {
                                //fail log
                                string logParams = "ObjectID:" + objDocLocation.ObjectID + "|Location:" + objDocLocation.Location + "|SystemName:" + objDocLocation.SystemName + "|Storage:" + objDocLocation.Storage;
                                lg.Info("LocalToServerUploader:CustomerSAImages", logParams);
                            }
                        }
                        else
                        {
                            lstCustomerSAImages_NotFound.Add(objDocLocation);
                            // update in db, its not found
                            int ISUploaded = SqlDb.CheckIntDBNull(dr[EDM.Setting.Fields.ISUploaded]) + 1;
                            md.UpdateCustomerSAImages(objDocLocation.ObjectID, objDocLocation.Storage, ISUploaded);
                        }
                    }
                    #endregion ---Table CustomerSAImages---

                    #region ---Send Email---
                    string strFileRow = string.Empty;
                    foreach (var obj in lstDocFiles_NotFound)
                    {
                        strFileRow += "<tr><td>DocFiles</td><td>" + obj.ObjectID + "</td><td>" + obj.Location + obj.SystemName + "</td></tr>";
                    }
                    foreach (var obj in lstProjectImages_NotFound)
                    {
                        strFileRow += "<tr><td>ProjectImages</td><td>" + obj.ObjectID + "</td><td>" + obj.Location + obj.SystemName + "</td></tr>";
                    }
                    foreach (var obj in lstCustomerSAImages_NotFound)
                    {
                        strFileRow += "<tr><td>CustomerSAImages</td><td>" + obj.ObjectID + "</td><td>" + obj.Location + obj.SystemName + "</td></tr>";
                    }
                    if (!string.IsNullOrEmpty(strFileRow))
                    {
                        SendMail(lg, configKey, programId, strFileRow);
                    }
                    #endregion ---Send Email---
                }
            }
            catch (Exception ex)
            {
                lg.Error("LocalToServerUploader:UploadFiles", new Exception(ex.Message));
            }
        }

        private void SendMail(EDM.Common.Log lg, string configKey, long programId, string strFileRow)
        {
            try
            {
                EDM.Setting.Email cfg = new EDM.Setting.Email(configKey, programId);
                cfg.ObjectId = (int)EDM.Setting.Email.Type.FileListNotFound;
                cfg.GetById();

                String logParams = "ObjectId:" + cfg.ObjectId + "|ToEmail:" + cfg.ToEmail + "|Subject:" + cfg.Subject
                + "|emailType:" + cfg.ObjectType;

                string template = cfg.Body;
                string body = string.Empty;

                if (!String.IsNullOrEmpty(template) && cfg.StatusId == EDM.Setting.Status.Active)
                {
                    body = template;
                    body = body.Replace("%%FILELIST%%", strFileRow);
                }
                EDM.Email.MailMessage mm = new EDM.Email.MailMessage(configKey, programId);
                if (!mm.SendWLog(Module + ":EDM.DocFile.LocalToServerUploader", cfg, programId, cfg.ToEmail, body, cfg.Subject, bccEmail: cfg.BccEmail))
                {
                    lg.Error("LocalToServerUploader:SendEmail", new Exception(mm.Message), logParams);
                }
                else
                {
                    lg.Info("LocalToServerUploader:SendEmail", logParams + "|Email Sent.");
                }
            }
            catch (Exception ex)
            {
                lg.Error("LocalToServerUploader:SendMail", new Exception(ex.Message));
            }
        }

        private bool upload(EDM.Common.Log lg, long DocTypeId, ref DocLocation obj, String configKey, long programId)
        {
            try
            {

                EDM.Setting.DB stg = new EDM.Setting.DB(configKey, programId);
                string baseLoc = stg.GetByKey(EDM.Setting.Key.UploadFilePath);

                string Storage = "";
                string PhysicalLoc = baseLoc + obj.Location + obj.SystemName;
                FileFactory fileFactory = new FileHandlerCreator(Module, configKey: configKey);
                IFileHandler fileHndl = fileFactory.GetFileUploadInstance(DocTypeId, out Storage);
                if (Storage == "AWS")
                {
                    bool isUploaded = fileHndl.UploadFile(PhysicalLoc, obj.Location, obj.SystemName);
                    try
                    {
                        if (File.Exists(PhysicalLoc))
                            File.Delete(PhysicalLoc);
                    }
                    catch (Exception ex)
                    {
                        lg.Error("LocalToServerUploader:upload", new Exception(ex.Message));
                    }
                }
                obj.Storage = Storage;
                return true;
            }
            catch (Exception ex)
            {
                lg.Error("LocalToServerUploader:upload", new Exception(ex.Message));
                return false;
            }
        }

        private bool IsFileExist(EDM.Common.Log lg, DataRow dr, ref DocLocation objDocLocation, string ObjectID, string Location, String configKey)
        {
            bool isExist = false;
            try
            {
                objDocLocation.ObjectID = SqlDb.CheckLongDBNull(dr[ObjectID]);
                objDocLocation.Storage = SqlDb.CheckStringDBNull(dr[EDM.Setting.Fields.Storage]);
                objDocLocation.Location = SqlDb.CheckStringDBNull(dr[Location]);
                objDocLocation.SystemName = SqlDb.CheckStringDBNull(dr[EDM.Setting.Fields.SystemName]);

                FileFactory fileFactory = new FileHandlerCreator(Module, configKey: configKey);
                IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(objDocLocation.Storage);
                isExist = fileHndl.IsFileExists(objDocLocation.Location + objDocLocation.SystemName);
            }
            catch (Exception ex)
            {
                lg.Error("LocalToServerUploader:IsFileExist", new Exception(ex.Message));
            }
            return isExist;
        }
        #endregion ---Private Method ---
    }
    public class DocLocation
    {
        public long ObjectID { get; set; }
        public string Location { get; set; }
        public string SystemName { get; set; }
        public string Storage { get; set; }
    }
}
