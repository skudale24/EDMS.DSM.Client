using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.ContentHandler
{
    public class MeasureDocs
    {
        #region --- Properties ---
        public String Message = String.Empty;
        public String Module = String.Empty;
        public long ProgramId = 0;
        public long ByUserId;

        private SqlDb Db;
        private Common.Log Lg;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.ContentHandler.MeasureDocs";
            }
        }
        
        #endregion

        #region --- Constructors ---
        public MeasureDocs()
        {
            ProgramId = Setting.Session.ProgramId;
            ByUserId = Setting.Session.UserId;
            ConfigKey = String.Empty;
        }
        public MeasureDocs(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public MeasureDocs(String module, String configKey) : this(module) { ConfigKey = configKey; }
        public MeasureDocs(String module, String configKey, long programId) : this(module, configKey) { ProgramId = programId; }
       
        #endregion

        #region --- Public Methods ---      
        
        public DataSet GetFileList2Upload(int BeforeHRS)
        {
            String logParams = string.Empty;
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.BeforeHRS] = BeforeHRS;

                Db.SetSql("p_GET_FileList2Upload", prms);
                Lg.Debug("GetFileList2Upload", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetFileList2Upload", ex, logParams);
                return null;
            }
        }
        public Boolean UpdateDocFiles(long DocFileID, string Storage,int ISUploaded)
        {
            String logParams = "DocFileID:" + DocFileID + "|Storage:" + Storage + "|ISUploaded:" + ISUploaded;

            try
            {
                Hashtable prms = new Hashtable();
                if (DocFileID > 0) prms["DocFileID"] = DocFileID;               
                prms["Storage"] = Storage;
                prms["ISUploaded"] = ISUploaded;

                Db.SetSql("p_U_DocFiles", prms);
                Lg.Info("UpdateDocFiles", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("UpdateDocFiles", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DocFileID = SqlDb.CheckIntDBNull(dr["DocFileID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return DocFileID > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("UpdateDocFiles", ex, logParams);
                return false;
            }
        }

        public Boolean UpdateProjectImage(long ProjectImageID, string Storage, int ISUploaded)
        {
            String logParams = "ProjectImageID:" + ProjectImageID + "|Storage:" + Storage + "|ISUploaded:" + ISUploaded;

            try
            {
                Hashtable prms = new Hashtable();
                if (ProjectImageID > 0) prms["ProjectImageID"] = ProjectImageID;
                prms["Storage"] = Storage;
                prms["ISUploaded"] = ISUploaded;

                Db.SetSql("p_U_ProjectImage", prms);
                Lg.Info("UpdateProjectImage", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("UpdateProjectImage", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                ProjectImageID = SqlDb.CheckIntDBNull(dr["ProjectImageID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return ProjectImageID > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("UpdateProjectImage", ex, logParams);
                return false;
            }
        }

        public Boolean UpdateCustomerSAImages(long CustomerSAIID, string Storage, int ISUploaded)
        {
            String logParams = "CustomerSAIID:" + CustomerSAIID + "|Storage:" + Storage + "|ISUploaded:" + ISUploaded;

            try
            {
                Hashtable prms = new Hashtable();
                if (CustomerSAIID > 0) prms["CustomerSAIID"] = CustomerSAIID;
                prms["Storage"] = Storage;
                prms["ISUploaded"] = ISUploaded;

                Db.SetSql("p_U_CustomerSAImages", prms);
                Lg.Info("UpdateCustomerSAImages", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("UpdateCustomerSAImages", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                CustomerSAIID = SqlDb.CheckIntDBNull(dr["CustomerSAIID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return CustomerSAIID > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("UpdateCustomerSAImages", ex, logParams);
                return false;
            }
        }
        
        #endregion
    }
}
