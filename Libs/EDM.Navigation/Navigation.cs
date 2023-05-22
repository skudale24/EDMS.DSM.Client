using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using VTI.Common;

namespace EDM.Navigation
{
    public class Group
    {
        public const String ContractorDetailsMenu = "ContractorDetailsMenu";
        public const String EvalScheduling = "EvalScheduling";
        public const String InspScheduling = "InspScheduling";
        public const String CustomerMenu = "CustomerMenu";
        public const String RebateMenu = "RebateMenu";
        public const String ProgramMeasureMenu = "ProgramMeasureMenu";
        public const String ApplicationMenu = "ApplicationMenu";
        public const String BadgesMenu = "BadgesMenu";
    }

    public class Navigation
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;
        public long ParentID;

        public long NavID;
        public String NavName;
        public String NavUrl;
        public Boolean WriteFlag;
        public Boolean LeftNavDisplay;
        //public long ParentId;
        public String ParentName;
        public String GroupName;
        public int ExecSeq = 0;
        public int StatusID;
        public String StatusName;
        public String PgTitle;
        public String PgElement;
        public int Permission;
        public String LeftNavDisplayName;
        public string WriteFlagStatus;
        public string LeftNavDispStatus;
        public string TranDate;
        public string ByUser;
        public int NavSequence;
        #endregion

        #region --- Constructors ---
        public Navigation() { ProgramId = Setting.Session.ProgramId; ByUserId = Setting.Session.UserId; }
        public Navigation(String module) : this() { Module = module; }
        public Navigation(String module, long navId) : this(module) { NavID = navId; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module, ProgramId are required. GroupName may be specified.
        /// </summary>
        public DataSet GetAll()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (!String.IsNullOrEmpty(GroupName)) prms["GroupName"] = GroupName;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigations", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetAll", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetAll", ex, logParams);
                return null;
            }
        }

        public DataSet GetByParentID()
        {
            String logParams = "ProgramId:" + ProgramId + "|ParentID:" + ParentID;
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.ParentID] = ParentID;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigation4Parent", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetByParentID", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetByParentID", ex, logParams);
                return null;
            }
        }
        /// <summary>
        /// Module, ProgramId are required.
        /// </summary>
        public DataSet GetAll4Dev()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.Module] = Module;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigations4Dev", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetAll4Dev", SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetAll4Dev", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Module, ProgramId, NavID are required.
        /// </summary>
        public DataSet GetAll4DevDragDrop()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }
                if (string.IsNullOrEmpty(Module)) { Message = "Module is required."; return null; }
                if (NavID <= 0) { Message = "NavID is required."; return null; }
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.Module] = Module;
                prms[EDM.Setting.Fields.NavID] = NavID;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigations4DragDrop", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetAll4DevDragDrop", SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetAll4DevDragDrop", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Module, ProgramId are required.
        /// </summary>
        public List<Navigation> GetAllList4Dev()
        {
            String logParams = "ProgramId:" + ProgramId + "Module:" + Module;
            List<Navigation> listNavigation = null;
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.Module] = Module;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigations4Dev", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetAllList4Dev", SqlforLog);
                DataSet ds = MsSql.ExecuteNoTransQuery(sql);

                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error fetching Navigations.";
                    return null;
                }
                else
                {
                    listNavigation = new List<Navigation>();
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        DataRow dr = ds.Tables[0].Rows[idx];
                        Navigation objNavigation = new Navigation();
                        objNavigation.ByUser = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.ByUser]);
                        objNavigation.ByUserId = MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.ByUserID]);
                        objNavigation.ExecSeq = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.ExecSeq]);
                        objNavigation.GroupName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.GroupName]);
                        objNavigation.LeftNavDisplay = MsSql.CheckBoolDBNull(dr[EDM.Setting.Fields.LeftNavDisplay]);
                        objNavigation.LeftNavDispStatus = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.LeftNavDispStatus]);
                        objNavigation.Module = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.Module]);
                        objNavigation.NavID = MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.NavID]);
                        objNavigation.NavName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.NavName]);
                        objNavigation.NavUrl = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.NavUrl]);
                        objNavigation.ParentName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.ParentName]);
                        objNavigation.StatusName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.StatusName]);
                        objNavigation.StatusID = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.StatusID]);
                        objNavigation.TranDate = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.TranDate]);
                        objNavigation.WriteFlagStatus = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.WriteFlagStatus]);
                        objNavigation.NavSequence = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.NavSequence]);

                        listNavigation.Add(objNavigation);
                    }
                }
                return listNavigation;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetAllList4Dev", ex, logParams);
                return listNavigation;
            }
        }

        /// <summary>
        /// ProgramId, Module, NavId are required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "ProgramId:" + ProgramId + "|NavId:" + NavID;
            try
            {
                if (NavID <= 0) { Message = "NavId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.NavID] = NavID;
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Navigation", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetById", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error fetching record.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                Module = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.Module]);
                NavName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.NavName]);
                NavUrl = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.NavUrl]);
                ParentID = MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.ParentID]);
                WriteFlag = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.WriteFlag]));
                LeftNavDisplay = DataUtils.Int2Bool(MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.LeftNavDisplay]));
                GroupName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.GroupName]);

                string strExecSeq = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.ExecSeq]);
                if (strExecSeq != "" || !string.IsNullOrEmpty(strExecSeq))
                {
                    ExecSeq = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.ExecSeq]);
                }
                else
                {
                    ExecSeq = -1;
                }
                StatusID = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.StatusID]);
                NavSequence = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.NavSequence]);

                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetById", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module is required. NavUrl is fetched from Request->Path. If Request is null, then NavUrl must be specified.
        /// </summary>
        public Boolean GetPgDetails(HttpRequest req = null)
        {
            String logParams = "Module:" + Module + "|ByUserId:" + ByUserId;
            try
            {
                if (req != null)
                {
                    NavUrl = GetNavUrl(req);
                    if (req["NavID"] != null)
                        NavID = Convert.ToInt64(req["NavID"]);
                }
                logParams += "|NavUrl:" + NavUrl;

                if (String.IsNullOrEmpty(Module)) { Message = "Module is required."; return false; }
                if (String.IsNullOrEmpty(NavUrl)) { Message = "NavUrl is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["Module"] = Module;
                prms["NavUrl"] = NavUrl;
                prms["NavName"] = NavName;
                if (ByUserId > 0) prms["ByUserID"] = ByUserId;
                if (NavID > 0) prms["NavID"] = NavID;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_PgTitle4Url", prms, out SqlforLog);
                Common.Log.Debug(Module + ":EDM.Navigation.Navigation", "GetPgTitle", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Record not found.";
                    Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetPgTitle", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                PgTitle = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.PgTitle]);
                PgElement = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.PgElement]);
                /* Jul 28, 2016 | Nibha Kothari | Fixing nav menu highlight */
                ParentID = MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.ParentID]);
                /* end Jul 28, 2016 | Nibha Kothari | Fixing nav menu highlight */
                ParentName = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.ParentName]);
                Permission = MsSql.CheckIntDBNull(dr[EDM.Setting.Fields.Permission]);

                return true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Module, GroupName, ByUserId are required. ExecSeq may be specified.
        /// </summary>
        public DataSet GetMenuByGroupName(long ProgramId = (long)EDM.Common.Programs.All)
        {
            String logParams = "GroupName:" + GroupName + "|ExecSeq:" + ExecSeq;
            try
            {
                if (String.IsNullOrEmpty(GroupName)) { Message = "GroupName is required."; return null; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["GroupName"] = GroupName;
                if (ExecSeq > 0) prms["ExecSeq"] = ExecSeq;
                prms["ByUserID"] = ByUserId;
                prms["ProgramID"] = ProgramId;
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Menu4Group", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetMenuByGroupName", SqlforLog);

                /* Jul 24, 2017 | Nibha Kothari | ES-3527: WAP: Implement Payout Process */
                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if(!MsSql.IsEmpty(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    NavID = MsSql.CheckLongDBNull(dr["NavID"]);
                    NavUrl = MsSql.CheckStringDBNull(dr["NavUrl"]);
                }

                return ds;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetMenuByGroupName", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// ProgramId, Module, NavName, ByUserId are required.
        /// </summary>
        public Boolean Add()
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|NavName:" + NavName + "|NavUrl:" + NavUrl
                + "|NavSequence:" + NavSequence + "|ParentID:" + ParentID + "|WriteFlag:" + WriteFlag + "|LeftNavDisplay:" + LeftNavDisplay
                + "|GroupName:" + GroupName + "|ByUserId:" + ByUserId + "|StatusId:" + StatusID;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (Module.Length <= 0) { Message = "Module is required."; return false; }
                if (NavName.Length <= 0) { Message = "NavName is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.Module] = Module;
                prms[EDM.Setting.Fields.NavName] = NavName;
                if (NavUrl.Length > 0) prms[EDM.Setting.Fields.NavUrl] = NavUrl;
                prms[EDM.Setting.Fields.ParentID] = ParentID;
                prms[EDM.Setting.Fields.WriteFlag] = DataUtils.Bool2Int(WriteFlag);
                prms[EDM.Setting.Fields.LeftNavDisplay] = DataUtils.Bool2Int(LeftNavDisplay);
                if (GroupName.Length > 0) prms[EDM.Setting.Fields.GroupName] = GroupName;
                if (ExecSeq >= 0) prms[EDM.Setting.Fields.ExecSeq] = ExecSeq;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;
                prms[EDM.Setting.Fields.StatusID] = StatusID;
                prms[EDM.Setting.Fields.NavSequence] = NavSequence;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Navigation", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "Add", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error adding record.";
                    Common.Log.Info(Module + ":EDM.Navigation.Navigation", "Add", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.Message]);
                NavID = MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.NavID]);

                return NavID > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "Add", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module, NavId, ByUserId are required.
        /// </summary>
        public Boolean Update()
        {
            String logParams = "ProgramId:" + ProgramId + "|Module:" + Module + "|NavName:" + NavName + "|NavUrl:" + NavUrl
                + "|NavSequence:" + NavSequence + "|ParentID:" + ParentID + "|WriteFlag:" + WriteFlag + "|LeftNavDisplay:" + LeftNavDisplay
                + "|GroupName:" + GroupName + "|ByUserId:" + ByUserId + "|StatusId:" + StatusID + "|ExecSeq:" + ExecSeq;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (NavID <= 0) { Message = "NavId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.NavID] = NavID;
                if (NavName.Length > 0) prms[EDM.Setting.Fields.NavName] = NavName;
                if (NavUrl.Length > 0) prms[EDM.Setting.Fields.NavUrl] = NavUrl;
                prms[EDM.Setting.Fields.ParentID] = ParentID;
                prms[EDM.Setting.Fields.WriteFlag] = DataUtils.Bool2Int(WriteFlag);
                prms[EDM.Setting.Fields.LeftNavDisplay] = DataUtils.Bool2Int(LeftNavDisplay);
                if (GroupName.Length > 0) prms[EDM.Setting.Fields.GroupName] = GroupName;
                if (ExecSeq >= 0) prms[EDM.Setting.Fields.ExecSeq] = ExecSeq;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;
                if (StatusID > 0) prms[EDM.Setting.Fields.StatusID] = StatusID;
                prms[EDM.Setting.Fields.NavSequence] = NavSequence;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Navigation", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "Update", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error updating record.";
                    Common.Log.Info(Module + ":EDM.Navigation.Navigation", "Update", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.Message]);

                return MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.NavID]) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "Update", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// ByUserId, XMLString are required.
        /// </summary>
        public Boolean UpdateInBulk(string xmlString)
        {
            String logParams = "|ByUserId:" + ByUserId + "|xmlString:" + xmlString;
            try
            {
                if (xmlString.Length <= 0) { Message = "XmlString is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;
                prms[EDM.Setting.Fields.XmlString] = xmlString;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_U_NavigationInBulk", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "UpdateInBulk", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = logParams + "|Error updating record.";
                    Common.Log.Info(Module + ":EDM.Navigation.Navigation", "UpdateInBulk", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr[EDM.Setting.Fields.Message]);

                return MsSql.CheckLongDBNull(dr[EDM.Setting.Fields.NavID]) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "UpdateInBulk", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// ProgramId are required.
        /// </summary>
        public DataSet GetDistinctModules()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_NavigationModules", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Navigation.Navigation", "GetDistinctModules", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Navigation.Navigation", "GetDistinctModules", ex, logParams);
                return null;
            }
        }
        #endregion

        #region --- Static Methods ---
        public static String GetNavUrl(HttpRequest req)
        {
            String navUrl = String.Empty;
            try
            {
                navUrl = req.Path;

                //if (navUrl.ToLower().Contains("custompage") && req["CustomPageID"] != null) navUrl += "?CustomPageID=" + req["CustomPageID"];
            }
            catch { }
            return navUrl;
        }

        public static String GetUrl(String navUrl, HttpRequest req, Boolean addQS = true, Boolean dropNavId = true)
        {
            try
            {
                if (!navUrl.Contains("?")) navUrl += "?";

                bool contains = System.Text.RegularExpressions.Regex.IsMatch(navUrl, @"\bContractorDocuments.aspx\b");

                if (!contains)
                {
                    if (navUrl.ToLower().Contains("custompage")) navUrl += "&CustomPageID=" + req["CustomPageID"];
                }

                if (addQS)
                {
                    foreach (String key in req.QueryString.Keys)
                    {
                        if (String.IsNullOrEmpty(key)) continue;
                        if (dropNavId && key == "NavID") continue;

                        if(!(contains && key == "CustomPageID"))
                        {
                            navUrl += "&" + key + "=" + req[key];
                        }  
                        
                    }
                }
            }
            catch { }
            return navUrl;
        }
        #endregion
    }

    public class Url
    {
        public const String IncentiveList = "/Accounting/Incentive/List.aspx";
        public const String IncentiveAutoApproved = "/Accounting/Incentive/AutoApproved.aspx";
        public const String EvaluationList = "/Evaluation/List.aspx";
        public const String InspectionList = "/Inspection/List.aspx";
        public const String InspectionCompleted = "/Inspection/Completed.aspx";
        public const String AdvisorSurvey = "/Reports/AdvisorSurveyResponse.aspx";
        public const String ContractorSurvey = "/Reports/ContractorSurveyResponse.aspx";
        public const String CustomerChangePassword = "/Tools/Customer/ChangePassword.aspx";
        public const String EvaluationDetailsWONav = "/Evaluation/HOEvalDetailWONav.aspx";
        public const String ApplicationList = "/Application/List.aspx";
        public const String CallLogList = "/Tools/CallLog/List.aspx";
        public const String QCNSearchCustomer = "/Contractor/QCNCustomerSearch.aspx";
        public const String QCNRebateStep1 = "/Contractor/Rebate/RebateStep1.aspx";
        public const String CustomerDocumentList = "/Tools/Customer/CustomerDocuments.aspx";
        public const String WorkOrderList = "/WorkOrder/List.aspx";
        public const String SendEmail = "/Tools/CustomEmail/SendEmail.aspx";
    }
}
