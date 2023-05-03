using System;

namespace EDM.Common
{
    public class SessionKeys
    {
        #region --- Field Planner, Vacation Time Off, Scheduling ---
        public static String SelectedAdvisor = "SelectedAdvisor";
        public static String SchedulingType="SchedulingType";
        public static String advCount = "advCount";
        public static String AllAdvCount = "AllAdvCount";
        public static String VacationConflictProjects = "VacationConflictProjects";
        public static String Advisors = "Advisors";
        public static String rowCount = "rowCount";
        public static String SelectedTab = "SelectedTab";
        public static String HOSchedEvalDBdataset = "HOSchedEvalDBdataset";
        public static String HORoutOptiDBdataset = "HORoutOptiDBdataset";
        public static String ScheduleLogWeekView = "ScheduleLogWeekView";
        public static String ScheduleLogRouteOpti = "ScheduleLogRouteOpti";
        public static String radDS = "radDS";
        public static String Duration = "Duration";
        public static String DriveRestrictions = "DriveRestrictions";
        public static String AdvisorHrs = "AdvisorHrs";
        public static String AdvisorID = "AdvisorID";
        public static String AdvisorName = "AdvisorName";
        public static String AdvisorTZ = "AdvisorTZ";
        public static String SchedArgs = "SchedArgs";
        public static String Start="Start";
        public static String End = "End";
        public static String BestAdvisor = "BestAdvisor";
        public static String iCallLogId = "iCallLogId";
        public static String OldUrlToRedirect = "OldUrlToRedirect";
        public static String NewCustomerLPCId = "NewCustomerLPCId";
        public static String RecommendedFromDate = "RecommendedFromDate";
        public static String RecommendedToDate = "RecommendedToDate";
        public static String WeekviewFromDate = "WeekviewFromDate";
        public static String WeekviewToDate = "WeekviewToDate";
        public static String WeekviewFromDatePrev = "WeekviewFromDatePrev";
        public static String FromRadWindow = "FromRadWindow";
        public static String SiteIDClickUrlToRedirect = "SiteIDClickUrlToRedirect";
        public static String SelectedContractor = "SelectedContractor";
        #endregion

        #region --Tools=>Customer=>List--
        public static String SearchResult = "SearchResult";
        public static String OnSelectedFilterColumnName = "OnSelectedFilterColumnName";
        public static String OnselectedFiltercolumnNameText = "OnselectedFiltercolumnNameText";
        public static String TmpSA="TmpSA";
        public static String QSWithoutCallLog="QSWithoutCallLog";

        public static String CustomerDetails = "CustomerDetails";
        #endregion

        #region --Evaluation=>EditNotes--
        public static String ReasonCode = "ReasonCode";
        #endregion

        public static String ReferrerName = "AdvisorQA";
        public static String PageInformation = "PageInformation";

        public static String RefPreview_ToEmail = "RefPreview_ToEmail";
        public static String RefPreview_FrmEmail = "RefPreview_FrmEmail";
        public static String RefPreview_Body = "RefPreview_Body";
        public static String RefPreview_Subject = "RefPreview_Subject";

        public static String CustomerLoginID="CustomerLoginID";
        public static String StoreID = "StoreID";
        public static String StoreNumber ="StoreNumber";
        public static String ServiceAddressCount ="ServiceAddressCount";
        public static String StoreName ="StoreName";
        public static String LoginID ="LoginID";
        public static String Password ="Password";

        public static String StartTimeCallLog = "StartTimeCallLog";
        public static String PhotoListData = "PhotoListData";
        public static String EvalPhotoListData = "EvalPhotoListData";
        public static String InspPhotoListData = "InspPhotoListData";

        #region---HD & CP
        public static String SessionTimeoutQCN="SessionTimeoutQCN";
        public static String ProgramIncentiveID="ProgramIncentiveID";
        public static String SessionTimeoutHD="SessionTimeoutHD";
        #endregion

        public static String ProjectID = "ProjectID";
        public static String ProjectStatus = "ProjectStatus";
        public static String LastDaysValue = "LastDaysValue";
        public static String CheckedState = "CheckedState";
        public static String OfferCode = "OfferCode";

        /*Start 25 OCT 2018 |Swapnil Bhave| DSM-625: UV:14220-2: QCN Force Acknowledgement of Agreement*/
        public static String UserTermsNotification = "UserTermsNotification";
        /*End 25 OCT 2018 |Swapnil Bhave| DSM-625: UV:14220-2: QCN Force Acknowledgement of Agreement*/

        public static String FiltrProgramID = "FiltrProgramID";
        public static String FiltrDistrictID = "FiltrDistrictID";
        public static String FiltrLPCID = "FiltrLPCID";
        public static String FiltrSupervisorID = "FiltrSupervisorID";
        public static String FiltrAdvisorID = "FiltrAdvisorID";
        public static String FiltrSchedulingMethod = "FiltrSchedulingMethod";
        public static String FiltrProjectManagerID = "FiltrProjectManagerID";
        public static String FiltrProgramModel = "FiltrProgramModel";
        public static String FiltrLstProjectType = "FiltrLstProjectType";
    }

    public class ViewState
    {
        public static String StartDate = "StartDate";
        public static String EndDate = "EndDate";
        public static String LPCIDs = "LPCIDs";
        public static String AdvisorIDs = "AdvisorIDs";
        public static String DoesHaveKneewall = "_DoesHaveKneewall";
        public static String DoesHaveKneewallR11 = "_DoesHaveKneewallR11";
        public static String UnitID = "unitID";
        public static String ImageWidth = "ImageWidth";
        public static String ImageHeight = "ImageHeight";
        public static String FilterExp = "FilterExp";
        public static String SortExp = "SortExp";
        public static String FilterExpUtil = "FilterExpUtil";
        public static String SortExpUtil = "SortExpUtil";
        public static String SortExpAdmin = "SortExpAdmin";
        public static String FilterExpAdmin = "FilterExpAdmin";
        public static String FilterExpCust = "FilterExpCust";
        public static String SortExpCust = "SortExpCust";
        public static String FilterExpContra = "FilterExpContra";
        public static String SortExpContra = "SortExpContra";
        public static String FilterExpHD = "FilterExpHD";
        public static String SortExpHD = "SortExpHD";
        public static String FilterExpFin = "FilterExpFin";
        public static String SortExpFin = "SortExpFin";
        public static String FilterExpEval = "FilterExpEval";
        public static String SortExpEval = "SortExpEval";
        public static String FilterExpInsp = "FilterExpInsp";
        public static String SortExpInsp = "SortExpInsp";
        public static String VLastDaysValue = "VLastDaysValue";
        public static String ProjectStatusesEval = "ProjectStatusesEval";
        public static String ProjectStatusesInsp = "ProjectStatusesInsp";
        public static String CheckCommand = "CheckCommand";
        public static String LPCPIPID = "LPCPIPID";
        public static String ProgramLPCPrefs = "ProgramLPCPrefs";
        public static String Action = "Action";
        public static String CheckBoxActions = "CheckBoxActions";
        public static String CheckBoxDraftActions = "CheckBoxDraftsActions";

        public static String FilterExpApplication = "FilterExpApplication";
        public static String SortExpApplication = "SortExpApplication";
        public static String FilterExpEvalution = "FilterExpEvalution";
        public static String SortExpEvalution = "SortExpEvalution";
        public static String FilterExpWorkOrder = "FilterExpWorkOrder";
        public static String SortExpWorkOrder = "SortExpWorkOrder";
        public static String FilterExpProjectApplication = "FilterExpProjectApplication";
        public static String SortExpProjectApplication = "SortExpProjectApplication";
        public static String FilterExpInspection = "FilterExpInspection";
        public static String SortExpInspection = "SortExpInspection";
    }
}
