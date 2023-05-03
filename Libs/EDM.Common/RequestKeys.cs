using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace EDM.Common
{
    public class RequestKeys
    {
        public static String ProgramID = "ProgramID";
        public const String SessKey = "SessKey";

        public static String CompanyID = "CompanyID";
        public static String CompanyType = "CompanyType";
        public static String ActionType = "ActionType";
        public static String Status="Status";
        public static String CompanyStatus="CompanyStatus";
        public static String action = "action";
        public static String ProjectID = "ProjectID";
        public static String MeasureID = "MeasureID";
        public static String UnitID = "UnitID";
        public static String CustomerID="CustomerID";
        public static String PremiseID = "PremiseID";
        public static String ZipCode = "ZipCode";
        public static String AutoStart = "AutoStart";
        public static String CustomerLoginID = "CustomerLoginID";
        public static String ServiceAddressID = "ServiceAddressID";
        public static String SiteID = "SiteID";
        public static String EventArgument = "__EVENTTARGET";
        public static String WorkflowName = "WorkflowName";
        public static String type="type";
        public static String Mode="Mode";
        public static String Duration = "Duration";
        public static String projectid = "projectid";
        public static String sid = "sid";
        public static String cid = "cid";
        public static String FCAID = "FCAID";
        public static String escore = "escore";
        public static String CustomerSAUniqueID = "CustomerSAUniqueID";
        public static String __EVENTTARGET = "__EVENTTARGET";
        public static String Action = "Action";
        public static String AuditorID = "AuditorID";
        public static String StartDate = "StartDate";
        public static String UserID = "UserID";
        public static String UserTypeID = "UserTypeID";

        public static String Sequence = "Sequence";
        public static String ProjectUniqueID = "ProjectUniqueID";
        public static String IsPopup = "IsPopup";
        public static String ShowPopup = "ShowPopup";

        public static String ThreadName = "ThreadName";
        public static String ThreadID = "ThreadID";
        public static String PIStatus = "PIStatus";
        public static String SAGHIApplication = "SAGHIApplication";
        public static String PIPTemplateID = "PIPTemplateID";
        public static String SelectedTab = "SelectedTab";
        public static String PIPStatus = "PIPStatus";
        public static String TrackingId = "TrackingId";

        #region ---Web API ---
        public static String Authorization = "Authorization";
        #endregion

        #region---HD & CP
        public static String SubMeasureName="SubMeasureName";
        public static String SubMeasureID="SubMeasureID";
        public static String PIMeasureID = "PIMeasureID";
        public static String MultiUnit = "MultiUnit";
        public static String ProgramIncentiveID = "ProgramIncentiveID";
        public static String refrence = "ref";
        #endregion

        #region --- WAP ---
        public static String ApplicationID = "ApplicationID";
        public static String CSAUtilId = "CSAUtilId";
        public static String AppStatus = "AppStatus";
        #endregion

        //
        #region --- ContributionFormID ---
        public static String ContributionFormID = "ContributionFormID";
        #endregion

        #region --- NEAT ---
        public static String MeasureNumber = "MeasureNumber";
        public static String WallCode = "WallCode";
        public static String WindowCode = "WindowCode";
        public static String SystemCode = "SystemCode";
        public static String DoorCode = "DoorCode";
        public static String AtticCode = "AtticCode";
        public static String FoundationCode = "FoundationCode";
        public static String WAPPressureID = "WAPPressureID";
        public static String LightCode = "LightCode";
        
        public static String WAPBlowerDoorID = "WAPBlowerDoorID";
        public static String WAPZonePressureID = "WAPZonePressureID";
        public static String WAPItemizedID = "WAPItemizedID";

        public static String TestID = "TestID";
        public static String WAPTestID = "WAPTestID";
        public static String PID = "PID";

        public static String Is1StRecord = "Is1StRecord";
        #endregion
        #region --- NEAT ---
        public static String Addition = "Addition";

        #endregion
        #region --- AWS ---
        public static String storage = "storage";
        public static String filePath = "filePath";
        public static String fileName = "fileName";
        public static String FileLocationType = "FileLocationType";
        #endregion
        #region PGE
        public static String Email = "Email";
        public static String EM = "EM";
        public static String TK = "TK";
        #endregion

        public static String OfferCode = "OfferCode";

        #region --- WorkOrder ---
        public static String WorkOrderID = "WorkOrderID";
        #endregion
        #region --- COVID Relief Fund ---
        public static String COVIDReliefFundID = "COVIDReliefFundID";
        #endregion
        
        public static String ContentKey = "ContentKey";
        public static String ContractID = "ContractID";

        public static String ObjectID = "ObjectID";
        public static String LoginID = "LoginID";
        public static String IsMFAEnabled = "IsMFAEnabled";
        public static String ObjectType = "ObjectType";
        public static String JobStatus = "JobStatus";
        public static String DuplicateAddress = "DuplicateAddress";
        public static String DuplicateAccount = "DuplicateAccount";

        public static String LPCID = "LPCID";
        public static String DPID = "DPID";
        public static string RoleID = "RoleID";


    }
}
