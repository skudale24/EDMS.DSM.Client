using System;
using System.Collections;
using System.Data;
using System.Web;
using VTI.Common;

namespace EDM.Navigation
{
    public class PermissionNav
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;

        public String ModuleId = String.Empty;
        public long RoleId;
        public String RoleName;
        public long NavId;
        public Boolean ReadAccess = false;
        public Boolean WriteAccess = false;
        public Boolean ArchiveAccess = false;
        public Boolean WriteFlagAccess = false;
        public Boolean LeftNavDisplayAccess = false;
        public int UpdateChildren = 0;
        public String LeftNavDisplay = String.Empty;
        public String SortExpression = String.Empty;
        public long ByUserId;
        #endregion

        #region --- Constructors ---
        public PermissionNav() { ProgramId = Setting.Session.ProgramId; }
        public PermissionNav(String module) : this() { Module = module; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module, ProgramId, NavId, RoleId, ByUserId are required.
        /// </summary>
        public Boolean Update()
        {
            String logParams = "ProgramId:" + ProgramId + "|NavId:" + NavId + "|RoleId:" + RoleId + "|ReadAccess:" + ReadAccess
                + "|WriteAccess:" + WriteAccess + "|ArchiveAccess:" + ArchiveAccess + "|ByUserId:" + ByUserId;

            try
            {
                if (NavId <= 0) { Message = "NavId is required."; return false; }
                if (RoleId <= 0) { Message = "RoleId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["NavID"] = NavId;
                prms["RoleID"] = RoleId;
                prms["ProgramID"] = ProgramId;
                prms["ReadAccess"] = DataUtils.Bool2Int(ReadAccess);
                prms["WriteAccess"] = DataUtils.Bool2Int(WriteAccess);
                prms["ArchiveAccess"] = DataUtils.Bool2Int(ArchiveAccess);
                prms["ByUserID"] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_PermissionNav", prms,out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "Update", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error updating record.";
                    Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "Update", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["NavID"]) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "Update", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module, ProgramId, RoleId are required.
        /// </summary>
        public DataSet GetAll()
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|RoleId:" + RoleId;

            try
            {
                if (Module.Length <= 0) { Message = "Module is required."; return null; }
                if (RoleId <= 0) { Message = "RoleId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;
                prms["RoleID"] = RoleId;
                if (ModuleId.Length > 0) prms["ParentID"] = ModuleId;
                if (SortExpression.Length > 0) prms["SortExp"] = SortExpression;
                if (LeftNavDisplay.Length > 0) prms["LeftNavDisplay"] = LeftNavDisplay;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PermissionNavs", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "GetAll", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "GetAll", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Module and RoleId or NavId are required.
        /// </summary>
        public DataSet Get4Dev(String all = "1")
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|RoleId:" + RoleId + "|NavId:" + NavId;

            try
            {
                if (Module.Length <= 0) { Message = "Module is required."; return null; }
                if (RoleId <= 0 && NavId <= 0) { Message = "RoleId or NavId are required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;

                String sql = String.Empty;
                String SqlforLog = string.Empty;
                if (RoleId > 0)
                {
                    prms["RoleID"] = RoleId;
                    sql = MsSql.GetSqlStmt("p_GET_PermissionNavs4Dev", prms, out SqlforLog);
                }
                else if (NavId > 0)
                {
                    prms["NavID"] = NavId;
                    sql = MsSql.GetSqlStmt("p_GET_PermissionRoles4Dev", prms, out SqlforLog);
                }
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "Get4Dev", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "Get4Dev", ex, logParams);
                return null;
            }
        }

        public DataSet Get4DevExport(String all = "1")
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|RoleId:" + RoleId + "|NavId:" + NavId;

            try
            {
                if (Module.Length <= 0) { Message = "Module is required."; return null; }
                if (RoleId <= 0 && NavId <= 0) { Message = "RoleId or NavId are required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;
                prms["RoleID"] = RoleId;
                prms["NavId"] = NavId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PermissionRolesExport", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "Get4DevExport", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "Get4DevExport", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Module, RoleId, NavId are required.
        /// Jul 14, 2017 | Nibha Kothari | ES-3489: WAP - Inspection View All Menu entry
        /// </summary>
        public Boolean GetByUrl(String navUrl)
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|RoleId:" + RoleId + "|navUrl:" + navUrl + "|NavId:" + NavId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required.";return false; }
                if (String.IsNullOrEmpty(Module)) { Message = "Module is required."; return false; }
                if (RoleId <= 0) { Message = "RoleId is required."; return false; }
                if (String.IsNullOrEmpty(navUrl)) { Message = "navUrl is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;
                prms["RoleID"] = RoleId;
                prms["NavUrl"] = navUrl;
                if (NavId > 0) prms["NavID"] = NavId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PermissionNav", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "GetByUrl", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "GetByUrl", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                ModuleId = MsSql.CheckStringDBNull(dr["ParentID"]);
                ReadAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["ReadAccess"]));
                WriteAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["WriteAccess"]));
                ArchiveAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["ArchiveAccess"]));

                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "GetByUrl", ex, logParams);
                return false;
            }
        }
        public Boolean GetPageNavigationPermissionByUrl(String navUrl)
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|RoleId:" + RoleId + "|navUrl:" + navUrl + "|NavId:" + NavId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (String.IsNullOrEmpty(Module)) { Message = "Module is required."; return false; }
                if (RoleId <= 0) { Message = "RoleId is required."; return false; }
                if (String.IsNullOrEmpty(navUrl)) { Message = "navUrl is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;
                prms["RoleID"] = RoleId;
                prms["NavUrl"] = navUrl;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PageNavigationPermission", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "GetPageNavigationPermissionByUrl", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.Navigation.PermissionNav", "GetPageNavigationPermissionByUrl", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                WriteFlagAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["WriteFlag"]));
                LeftNavDisplayAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["LeftNavDisplay"]));
                ReadAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["ReadAccess"]));
                WriteAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["WriteAccess"]));
                ArchiveAccess = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr["ArchiveAccess"]));

                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "GetPageNavigationPermissionByUrl", ex, logParams);
                return false;
            }
        }
        /* Mar 01, 2016 | Nibha Kothari | ES-739: Custom Pages */
        public Boolean GetByUrl(System.Web.UI.Page pg)
        {
            try
            {
                /* Jul 14, 2017 | Nibha Kothari | ES - 3489: WAP - Inspection View All Menu entry */
                if (!String.IsNullOrEmpty(pg.Request["NavID"])) long.TryParse(pg.Request["NavID"], out NavId);
                return GetByUrl(Navigation.GetNavUrl(pg.Request));
            }
            catch (Exception ex) { Common.Log.Error(Module, Module + ":EDM.Navigation.PermissionNav", "GetByUrl", ex); return false; }
        }
        /* END Mar 01, 2016 | Nibha Kothari | ES-739: Custom Pages */
        #endregion
    }
}
