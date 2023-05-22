using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Navigation
{
    public class PermissionBlock
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;

        public long RoleId = 0;
        public String NavUrl = String.Empty;
        public Boolean Access = true;
        #endregion

        #region --- Constructors ---
        public PermissionBlock() { ProgramId = Setting.Session.ProgramId; }
        public PermissionBlock(String module) : this() { Module = module; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module, RoleId, NavUrl are required.
        /// </summary>
        public Boolean GetByUrl()
        {
            String logParams = "ProgramId:" + ProgramId + "|RoleId:" + RoleId + "|NavUrl:" + NavUrl;
            try
            {
                if (RoleId <= 0) { Message = "RoleId is required."; return false; }
                if (NavUrl.Length <= 0) { Message = "NavUrl is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["RoleID"] = RoleId;
                prms["NavUrl"] = NavUrl;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PermissionBlock", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionBlock", "GetByUrl", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Access = true;
                    return true;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                if (MsSql.CheckLongDBNull(dr["NavID"]) > 0) Access = false;

                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionBlock", "GetByUrl", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}
