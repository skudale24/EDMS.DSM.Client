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
    public class MissingFileReport
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
        public MissingFileReport() { Init(); }
        public MissingFileReport(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MissingFileReport(String module) : this() { Module = module; }
        public MissingFileReport(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MissingFileReport(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new EDM.Common.Log(ConfigKey);
            Lg.ModuleName = configKey + ".Service.MissingFileReport";
        }
        #endregion --- Private Methods ----

        #region --- Public Methods ---
        public EDM.Common.MethodReturn ProcessMissingFilesReport()
        {
            EDM.Common.MethodReturn mr = new EDM.Common.MethodReturn();
            DataSet ds = null; long _imagecount = 0, _signimagecount = 0, _pdfcount = 0, _autoapprovedpdfcount = 0;
            try
            {
                Hashtable prms = new Hashtable();
                prms["FileDurationKey"] = "CSGFileCheckApp_Service_GetFileDuration";                 

                /********* Start Missing Image File Processing  *****************/
                Lg.Info("[ProcessMissingFilesReport]", "## Start Missing Image Processing. ##");
                Db.SetSql("p_AP_GET_AllImageFileInfo", prms);
                ds = Db.ExecuteNoTransQuery();
                if (!SqlDb.IsEmpty(ds))
                {
                    long count = ds.Tables[0].Rows.Count;
                    _imagecount = count;
                    Lg.Info("[ImageProcess]", "## Processing images of count :" + count + " ##");
                    EDM.DocFile.MissingImageFile objMissingImageFile = new EDM.DocFile.MissingImageFile(Module, ConfigKey, ProgramId);
                    objMissingImageFile.FindMissingImageFiles(ds);                    
                }
                else
                {
                    Lg.Info("[ImageProcess]", "No image records to process.");
                }          
                Lg.Info("[ProcessMissingFilesReport]", "## End Missing Image Processing. ##");
                /********* End Missing Image File Processing  *****************/


                /********* Start  Sign Image File Processing  *****************/
                Lg.Info("[ProcessMissingFilesReport]", "## Start Sign in Image Processing. ##");
                Db.SetSql("p_AP_GET_AllSignFileInfo", prms);
                ds = Db.ExecuteNoTransQuery();
                if (!SqlDb.IsEmpty(ds))
                {
                    long count = ds.Tables[0].Rows.Count;
                    _signimagecount = count;
                    Lg.Info("[ImageProcess]", "## Processing sign in images of count :" + count + " ##");
                    EDM.DocFile.MissingSignFile objMissingSignImageFile = new EDM.DocFile.MissingSignFile(Module, ConfigKey, ProgramId);
                    objMissingSignImageFile.FindMissingSignInImageFiles(ds);                    
                }
                else
                {
                    Lg.Info("[ImageProcess]", "No sign in image records to process.");
                }                 
                Lg.Info("[ProcessMissingFilesReport]", "## End  sign in Image Processing. ##");
                /********* End Missing  Sign Image Processing  *****************/

                /********* Start Missing PDF Processing  *****************/
                Lg.Info("[ProcessMissingFilesReport]", "## Start Missing PDF Processing. ##");
                Db.SetSql("p_AP_GET_AllPDFFilesInfo", prms);
                ds = Db.ExecuteNoTransQuery();
                if (!SqlDb.IsEmpty(ds))
                {
                    long count = ds.Tables[0].Rows.Count;
                    _pdfcount = count;
                    Lg.Info("[PDFProcess]", "## Processing pdf of count :" + count + " ##");
                    EDM.DocFile.MissingPdfFile objMissingPdfFile = new EDM.DocFile.MissingPdfFile(Module, ConfigKey, ProgramId);
                    objMissingPdfFile.FindMissingPDFFiles(ds);
                }
                else
                {
                    Lg.Info("[PDFProcess]", "No pdf records to process.");
                }                   
                Lg.Info("[ProcessMissingFilesReport]", "## End Missing PDF Processing. ##");
                /********* End Missing PDF Processing  *****************/

                /********* Start Missing AutoApprovedPDF Processing  *****************/
                Lg.Info("[ProcessMissingFilesReport]", "## Start Missing AutoApprovedPDF Processing. ##");
                Db.SetSql("p_AP_GET_AllAutoApprovedFileInfo", prms);
                ds = Db.ExecuteNoTransQuery();
                if (!SqlDb.IsEmpty(ds))
                {
                    long count = ds.Tables[0].Rows.Count;
                    _autoapprovedpdfcount = count;
                    Lg.Info("[AutoApprovedPDFProcess]", "## Processing AutoApprovedPDF of count :" + count + " ##");
                    EDM.DocFile.MissingAutoApprovedPdfFile objMissingPdfFile = new EDM.DocFile.MissingAutoApprovedPdfFile(Module, ConfigKey, ProgramId);
                    objMissingPdfFile.FindMissingAutoApprovedPDFFiles(ds);
                }
                else
                {
                    Lg.Info("[AutoApprovedPDFProcess]", "No AutoApproved PDF records to process.");
                }           
                Lg.Info("[ProcessMissingFilesReport]", "## End Missing AutoApprovedPDF Processing. ##");
                /********* End Missing AutoApprovedPDF Processing  *****************/

                string Message = " Missing Images: " + _imagecount + " records processed.";
                       Message += " Missing sign in images: " + _signimagecount + " records processed.";
                       Message += " Missing pdf: " + _pdfcount + " records processed.";
                       Message += " Missing AutoApprovedPDF: " + _autoapprovedpdfcount + " records processed.";              

                mr.Message = Message;
                mr.Status = true;
                return mr;
            }
            catch (Exception ex)
            {
                mr.Status = false; mr.Message = ex.Message;
                Lg.Error("ProcessMissingFilesReport", ex);
                return mr;
            }
        }
        #endregion
    }
}
