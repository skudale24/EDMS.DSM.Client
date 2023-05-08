using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.CommunicationTemplate
{
    public class DocumentTemplate
    {
        #region ---Variables---
        public string Module { get; set; }
        public string Message { get; set; } = String.Empty; 
        public long ProgramId { get; set; }
        public long ByUserId { get; set; }
        public long ProjectID { get; set; }
        public long ObjectID { get; set; }

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
                Lg.ModuleName = Module + ":EDM.CommunicationTemplate";
            }
        }
        #endregion

        #region ---Constructor---
        public DocumentTemplate()
        {
            ConfigKey = String.Empty;
        }
        #endregion

        #region ---Methods---

        public DataSet GetAllTemplates()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramId"] = ProgramId;
                prms["Module"] = Module;
                Db.SetSql("p_GET_DocumentTemplate", prms);
                Lg.Info("GetAllTemplates", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAllTemplates", ex);
                return null;
            }
        }


        #endregion
    }
}
