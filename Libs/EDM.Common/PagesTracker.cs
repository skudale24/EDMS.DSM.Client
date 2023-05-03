using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.Common
{
    public class PagesTracker
    {
        #region --- Members ---
        public String Module;
        public String Message;

        private EDM.Common.Log Lg;
        private SqlDb Db;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Common.PagesTracker";
            }
        }
        #endregion

        #region --- Properties ---
        public long PagesTrackerID { get; set; }
        public long UserID { get; set; }
        public long RoleID { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public string Source { get; set; }
        public long SourceID { get; set; }

        #endregion

        #region --- Constructors ---
        public PagesTracker() { ConfigKey = String.Empty; }
        public PagesTracker(String module) { Module = module; ConfigKey = String.Empty; }
        #endregion

        #region --- Methods ---
        public Boolean Add()
        {
            String logParams = "UserID:" + UserID + "|RoleID:" + RoleID + "|PageTitle:" + PageTitle
                + "|PageURL:" + PageURL + "|Source:" + Source + "|SourceID:" + SourceID + "|Module:" + Module;
            try
            {
                if (UserID <= 0) { Message = "UserID is required."; return false; }
                if (RoleID <= 0) { Message = "RoleID is required."; return false; }                                

                Hashtable prms = new Hashtable();
                prms["UserID"] = UserID;
                prms["RoleID"] = RoleID;
                if (!String.IsNullOrEmpty(PageTitle)) prms["PageTitle"] = PageTitle;
                if (!String.IsNullOrEmpty(PageURL)) prms["PageURL"] = PageURL;
                if (!String.IsNullOrEmpty(Module)) prms["Module"] = Module;
                if (!String.IsNullOrEmpty(Source)) prms["Source"] = Source;
                if (SourceID>0) prms["SourceID"] = SourceID;

                Db.SetSql("p_A_PagesTracker", prms);
                Lg.Info("Add", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding page tracking record.";
                    Lg.Info("Add", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                return SqlDb.CheckLongDBNull(dr["PagesTrackerID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Add", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}
