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
    public class DocTypeStorage
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;

        public long DocTypeId;
        public String Storage = String.Empty;
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
                Lg.ModuleName = Module + ":EDM.ContentHandler.DocTypeStorage";
            }
        }
        #endregion

        #region --- Constructors ---
        public DocTypeStorage() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = Setting.Session.UserId; }
        public DocTypeStorage(String module) : this() { Module = module; }
        public DocTypeStorage(String module,String configKey) : this() { Module = module; ConfigKey = configKey; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module is required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "ProgramId:" + ProgramId + "|DocTypeId:" + DocTypeId;
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.DocTypeID] = DocTypeId;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;

                Db.SetSql("p_GET_DocTypeStorage", prms);
                Lg.Debug("GetById", Db.SqlStmt);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Record not found."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Storage = MsSql.CheckStringDBNull(dr["Storage"]);
                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler.DocTypeStorage", "GetById", ex, logParams);
                return false;
            }
        }
        
        #endregion
    }
}
