using System;
using System.Collections;
using System.Data;
using VTI.Common;
using EDMS.DSM.Data;
using System.Data.SqlClient;

namespace EDM.Setting
{

    public static class DailyRecapType
    {
        public const String Application = "Application";
        public const String Evaluation = "Evaluation";
        public const String Workorder = "Workorder";
        public const String ProgramIncentive = "ProgramIncentive";
        public const String Inspection = "Inspection";
        public const String ViewMore = "ViewMore";
    }
    public static class Key
    {
        
        public const String SiteUrl = "SiteUrl";
        public const String LandingUrl = "LandingUrl";
        public const String APUrl = "APUrl";
        public const String CPUrl = "CPUrl";
        public const String ImageUrl = "ImageUrl";
        public const String UploadFilePath = "UploadFilePath";
        public const String InvoiceUpload = "InvoiceUpload";
        public const String AdminEmail = "AdminEmail";
        public const String SmtpHost = "SmtpHost";
        /* Aug 16, 2017 | Nibha Kothari | ES-3710: Implement SMTP Port Setting */
        public const String SmtpPort = "SmtpPort";
        /* end Aug 16, 2017 | Nibha Kothari | ES-3710: Implement SMTP Port Setting */
        public const String SmtpUN = "SmtpUN";
        public const String SmtpPwd = "SmtpPwd";
        public const String ContractorSelfInstall = "ContractorSelfInstall";
       // public const String LPCRebatePDFPathPhysical = "LPCRebatePDFPathPhysical";
        public const String LPCRebatePDFPath = "LPCRebatePDFPath";
        public const String VMFUrl = "VMFUrl";
        public const String ScheduleLockDays = "ScheduleLockDays";
        public const String ScheduleDateRangeDays = "ScheduleDateRangeDays";
        public const String PayoutPayorDescription = "PayoutPayorDescription";
        public const String BingKey = "BingKey";
        public const String GoogleKey = "GoogleKey";
        public const String LocationTool = "LocationTool";
        public const String DurationHVACTuneUp = "DurationHVACTuneUp";
        public const String SchedEvalActionText = "SchedEvalActionText";
        public const String SchedInspActionText = "SchedInspActionText";
        public const String SchedCollectPaymentProjectType = "SchedCollectPaymentProjectType";
        public const String SchedCollectPaymentProjectType1 = "SchedCollectPaymentProjectType1";
        public const String SchedCollectPaymentProjectType2 = "SchedCollectPaymentProjectType2";
        public const String SchedCollectPaymentProjectType3 = "SchedCollectPaymentProjectType3";
        public const String SaveCCDetails = "SaveCCDetails";
        public const String DotNetChargeSecretKey = "DotNetChargeSecretKey";
        public const String DotNetChargeStoreNumber = "DotNetChargeStoreNumber";
        public const String DotNetChargeLoginID = "DotNetChargeLoginID";
        public const String DotNetChargePassword = "DotNetChargePassword";
        public const String DotNetChargeMode = "DotNetChargeMode";
        public const String DotNetChargeStoreName = "DotNetChargeStoreName";
        public const String SAGIRequired = "SAGIRequired";
        public const String ExternalScheduler = "ExternalScheduler";
        public const String ExternalSchedulerRegion = "ExternalSchedulerRegion";
        public const String ExternalSchedulerAdvisorCount = "ExternalSchedulerAdvisorCount";
        public const String ExternalSchedulerLunchBreak = "ExternalSchedulerLunchBreak";
        public const String ExternalSchedulerGradeCap = "ExternalSchedulerGradeCap";
        public const String ExternalSchedulerUID = "ExternalSchedulerUID";
        public const String ExternalSchedulerPwd = "ExternalSchedulerPwd";
        public const String ExternalSchedulerNumber = "ExternalSchedulerNumber";    /* Mar 31, 2016 | Nibha Kothari | Click task.Number */
        public const String FromEmail = "FromEmail";
        public const String ReschedLink = "ReschedLink";
        public const String SendSurveyEmail = "SendSurveyEmail";
        public const String QCNProfileAdminEmail = "QCNProfileAdminEmail";
        public const String ReportTemplateLocation = "ReportTemplateLocation";
        public const String HomeDepotID = "HomeDepotID";
        public const String MLGWServiceUrl = "MLGWServiceUrl";
        public const String EDMAccessToken = "EDMAccessToken";
        public const String ServiceVisit = "ServiceVisit";
        public const String ReportLocation = "ReportLocation";
        public const String InvoiceTemplateLocation = "InvoiceTemplateLocation";
        public const String InvoiceLocation = "InvoiceLocation";
        public const String InvoiceZipTemp = "InvoiceZipTemp";
        public const String InvoiceZipPath = "InvoiceZipPath";
        public const String FundingReconciliationTemplate = "FundingReconciliationTemplate";
        /* Jun 06, 2016 | Nibha Kothari | ES-1534: Status Change for NON Rebated : Time based automation */
        public const String Approved2NonRebatedDays = "Approved2NonRebatedDays";
        public const String ApkVersionError = "ApkVersionError";
        /* Jun 27, 2016 | Nibha Kothari | ES-1605: UV:11618 QCN Volume Awards - Addition of Graphic to Admin Portal and Contractor Portal */
        public const String StickyBanner = "StickyBanner";
        public const String UtilDataImportPath = "UtilDataImportPath";
        public const String UtilDataImportFailOnError = "UtilDataImportFailOnError";
        /* Dec 01, 2016 | Nibha Kothari | ES-2438: Vectren: File Import for Eligibility */
        public const String UtilDataImportDataTableLimit = "UtilDataImportDataTableLimit";
        /* end Dec 01, 2016 | Nibha Kothari | ES-2438: Vectren: File Import for Eligibility */
        /* Nov 22, 2016 | Snehal Vaidya | ES-2357: Optimized View: Changes for 30 day period */
        public const String RouteOptiDefaultDays = "RouteOptiDefaultDays";
        public const String OffsetDefaultDays4Scheduling = "OffsetDefaultDays4Scheduling";
        public const String SchedulingMaxDaysAllowed = "SchedulingMaxDaysAllowed";
        /* Start Feb 05, 2019 | Swapnil Bhave | DSM-879: Self Schedule Updates */
        public const String SchedulingMaxDaysAllowed4Cust = "SchedulingMaxDaysAllowed4Cust";
        /* End Feb 05, 2019 | Swapnil Bhave | DSM-879: Self Schedule Updates */
        /* Nov 08, 2016 | Nibha Kothari | ES-2208: Scheduling: Resource Capacity */
        public const String WeekStart = "WeekStart";
        /* Dec 02, 2016 | Snehal Vaidya | ES-2401 : SUP - 153 : UV:13894 Customer Summary Data Report- District Data Not Exporting */
        public const String QueryTimeout = "QueryTimeout";
        /* Jan 09, 2017 | Nibha Kothari | ES-1386: Customer Appointment Reminder */
        public const String CustSchedAppt = "CustSchedAppt";
        /* end Jan 09, 2017 | Nibha Kothari | ES-1386: Customer Appointment Reminder */
        /* Jan 12, 2017 | Snehal Vaidya | ES-2357: ES-1639: Scheduling: Appointment Duration flexibility */
        public const String MaximumAppointmentDuration = "MaximumAppointmentDuration";
        /* END Jan 12, 2017 | Snehal Vaidya | ES-2357: ES-1639: Scheduling: Appointment Duration flexibility */
        /* Feb 06, 2017 | Snehal Vaidya | ES-2744: Scheduling: Restore Route Optimized tab */
        public const String RouteOptimizedTab = "RouteOptimizedTab";
        /* END Feb 06, 2017 | Snehal Vaidya | ES-2744: Scheduling: Restore Route Optimized tab */
        public const String CCPayment = "PaymentLanguage";
        public const String SelfAuditURL = "SelfAuditURL";
        public const String FindContractorURL = "FindContractorURL";
        /* Mar 03, 2017 | Nibha Kothari | ES-1546: EDMS.Service - Remove Entries from Web.Config- Use DB */
        public const String LPCPayoutNotiCheckGap = "LPCPayoutNotiCheckGap";
        public const String LockAdvisorSchedule = "LockAdvisorSchedule";
        public const String DefaultUploadLocation = "DefaultUploadLocation";
        public const String ReportPage1_4D_10 = "ReportPage1_4D_10";
        public const String ReportPage1_3D_10 = "ReportPage1_3D_10";
        public const String ReportPage1_10 = "ReportPage1_10";
        public const String ReportPage1_4D = "ReportPage1_4D";
        public const String ReportPage1_3D = "ReportPage1_3D";
        public const String ReportPage1 = "ReportPage1";
        public const String ReportTemplateImgLocation = "ReportTemplateImgLocation";
        public const String p2air = "p2air";
        public const String p2attic = "p2attic";
        public const String p2duct = "p2duct";
        public const String p2lighting = "p2lighting";
        public const String p2heating = "p2heating";
        public const String p2appliances = "p2appliances";
        public const String p2water = "p2water";
        public const String p2refrigerator = "p2refrigerator";
        public const String p2windows = "p2windows";
        public const String p2wall = "p2wall";
        public const String ScaleLocation = "ScaleLocation";
        public const String dollar = "dollar";
        public const String GemBoxKey = "GemBoxKey";
        /* Jun 01, 2017 | Snehal Vaidya | ES-3260 : Customer Address Common Control */
        public const String AllStateListString = "AllStateListString";
        public const String SmartStreetHTMLKey = "SmartStreetHTMLKey";
        public const String SmartyStreetAuthToken = "SmartyStreetAuthToken";
        public const String SmartyKey = "SmartyKey"; //March 17 2022 | DSM-5360 Ganesh Jamdurkar | New Smarty Key meant to be used with all frontend calls directly to SmartyStreet from web-browser (javascript) or mobile app
        /* end Jun 01, 2017 | Snehal Vaidya | ES-3260 : Customer Address Common Control */
        /* Jun 07, 2017 | Nibha Kothari | ES-3286: CP: Self Schedule Change Request */
        public const String CustSelfSchedStartAfterDays = "CustSelfSchedStartAfterDays";
        public const String CustSelfSchedEndAfterDays = "CustSelfSchedEndAfterDays";
        public const String CustVirtualSelfSchedStartAfterDays = "CustVirtualSelfSchedStartAfterDays";
        public const String CustVirtualSelfSchedEndAfterDays = "CustVirtualSelfSchedEndAfterDays";
        /* end Jun 07, 2017 | Nibha Kothari | ES-3286: CP: Self Schedule Change Request */
        /* Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
        public const String CCErrorMessageVisible = "CCErrorMessageVisible";
        /* end Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
        /* Oct 19, 2017 | Nibha Kothari | ES-3949: Get EDMS legacy scheduling system working */
        public const String DisplayEST = "DisplayEST";
        public const String ScheduleUseLocation = "ScheduleUseLocation";
        /* end Oct 19, 2017 | Nibha Kothari | ES-3949: Get EDMS legacy scheduling system working */
        /* Oct 09, 2017 | Snehal Vaidya | ES-3859: Braintree Implementation */
        public const String CCProcessEnabled = "CCProcessEnabled";
        public const String CCProcessGateway = "CCProcessGateway";
        public const String BraintreeMerchantID = "BraintreeMerchantID";
        public const String BraintreePublicKey = "BraintreePublicKey";
        public const String BraintreePrivateKey = "BraintreePrivateKey";
        public const String BraintreeMode = "BraintreeMode";
        /* end Oct 09, 2017 | Snehal Vaidya | ES-3859: Braintree Implementation */
        /* Nov 08, 2017 | Snehal Vaidya | ES-4114: (ES-4063) Your Energy Partner Plan Page */
        public const String UploadImagePath = "UploadImagePath";

        /* Start March 15, 2019 | Atul More | DSM-974 : Create a Module (EDM.Audit) to communicate with API */
        public const String LimitedIncomeAuditApiURL = "LimitedIncomeAuditApiURL";

        /* Start Aug 29, 2019 | Atul More | DSM-1356: Integrating https://www.addthis.com/ */
        public const String AddThis_WidgetJsPath = "AddThis_WidgetJsPath";
        public const String AddThis_PostPagePath = "AddThis_PostPagePath";         
        public const String AddThis_PostTitle = "AddThis_PostTitle";              
        public const String AddThis_PostDescription = "AddThis_PostDescription";
        public const String AddThis_IsActive = "AddThis_IsActive";
        public const String AddThis_Customer_PostURL = "AddThis_Customer_PostURL";  //DSM-2840

        /* Start May 19, 2020 | Atul More | DSM-2483: eScore - Add NPS sharing for Contractor Survey */
      
        public const String AddThis_QCNSurvey_IsActive = "AddThis_QCNSurvey_IsActive";

        public const String AddThis_Common_PostImageUrl = "AddThis_Common_PostImageUrl";
        public const String AddThis_Twitter_PostImageUrl = "AddThis_Twitter_PostImageUrl";
        public const String AddThis_Pinterest_PostImageUrl = "AddThis_Pinterest_PostImageUrl";

        public const String SmartyStreetAuthID = "SmartyStreetAuthID";
        public const String EPBAdvisorUserName = "EPBAdvisorUserName";
        public const String EPBAdvisorUserPassword = "EPBAdvisorUserPassword";
        public const String DailyRecapViewMoreRows = "DailyRecapViewMoreRows";

        #region --- WS ---

        public const String eScoreApkVersion = "eScoreApkVersion";

        #endregion --- WS ---

        #region --- Web API ---

        public const String WebApiAuthenticate = "WebApiAuthenticate";
        public const String WebApiKey = "WebApiKey";
        public const String WebApiSecretKey = "WebApiSecretKey";
        public const String WebApiVersion = "WebApiVersion";

        #endregion --- Web API ---

        /* Mar 23, 2017 | Nibha Kothari | ES-2972 WAP: Application Management */
        #region --- WAP ---
        public const String WAPProgramStart = "WAPProgramStart";
        public const String DisplayQCNProgramBudget = "DisplayQCNProgramBudget";
        public const String DisplayQCNQualityContractorImage = "DisplayQCNQualityContractorImage";
        public const String QCNLogoClickURL = "QCNLogoClickURL";
        //ES-4329 Nilesh K
        public const String EDMSDatabaseName = "EDMSDatabaseName";
        #endregion --- WAP ---
        public const String LPCID = "LPCID";
        public const String CFLReceived = "CFLReceived";
        public const String SHReceived = "SHReceived";
        public const String OrderedQty = "OrderedQty";
        public const String ReceivedQty = "ReceivedQty";

        #region --- AWS Keys ---
        public const String AWS_AccessKeyId = "AWS_AccessKeyId";
        public const String AWS_SecretAccessKeyId = "AWS_SecretAccessKeyId";
        public const String AWS_DefaultUploadLocation = "AWS_DefaultUploadLocation";
        public const String AWS_ReportLocation = "AWS_ReportLocation";
        public const String AWS_UploadFilePath = "AWS_UploadFilePath";
       // public const String AWS_LPCRebatePDFPathPhysical = "AWS_LPCRebatePDFPathPhysical";
        public const String AWS_RegionEndpoint = "AWS_RegionEndpoint";
        #endregion --- AWS Keys ---
        public const String LocalFile2ServerHRS = "LocalFile2ServerHRS";    
        public const String Zendesk_IsActive = "Zendesk_IsActive";          
        public const String GPP_CPUrl = "GPP_CPUrl";

        public const String ReliefFundEmail = "ReliefFundEmail";
        #region PandaDoc and DocuSign API Keys
        public const String PandaDocAPIURL = "PandaDocAPIURL";
        public const String DPPCTemplateID = "DPPCTemplateID";
        public const String W9TemplateID = "W9TemplateID";

        public const String DPPCSendEmailSubject = "DPPCSendEmailSubject";
        public const String DPPCSendEmailMessage = "DPPCSendEmailMessage";
        public const String W9SendEmailSubject = "W9SendEmailSubject";
        public const String W9SendEmailMessage = "W9SendEmailMessage";
        public const String ApproverFirstName = "ApproverFirstName";
        public const String ApproverLastName = "ApproverLastName";
        public const String ApproverEmail = "ApproverEmail";
        public const String ReviewerFirstName = "ReviewerFirstName";
        public const String ReviewerLastName = "ReviewerLastName";
        public const String ReviewerEmail = "ReviewerEmail";
        public const String ContributionRequestFormTVAFullName = "ContributionRequestFormTVAFullName";
        public const String ContributionRequestFormTVAEmail = "ContributionRequestFormTVAEmail";

        public const String OnlineApplicationSigningAuthorityVendor = "OnlineApplicationSigningAuthorityVendor";
        public const String Docusign = "docusign";
        public const String PandaDoc = "pandadoc";

        public const String DocuSignAPIURL = "DocuSignAPIURL";
        public const String DocuSignAccountID = "DocuSignAccountID";
        public const String DPPCDocuSignTemplateID = "DPPCDocuSignTemplateID";
        public const String W9DocuSignTemplateID = "W9DocuSignTemplateID";
        public const String IAADocuSignTemplateID = "IAADocuSignTemplateID";
        public const String IAA_IA_DocuSignTemplateID = "IAA_IA_DocuSignTemplateID";
        public const String IAA_IA_DocuSignWebhookURL = "IAA_IA_DocuSignWebhookURL";
        public const String DocuSignEnvelopeStatusCreated = "created";
        public const String DocuSignEnvelopeStatusSent = "sent";
        public const String DocusignBrandID = "DocusignBrandID";
        public const String DocuSignUserId = "DocuSignUserId";
        public const String DocuSignClientId = "DocuSignClientId";
        public const String DocuSignAuthServer = "DocuSignAuthServer";
        public const String DocuSignPrivateKey = "DocuSignPrivateKey";
        public const String DocuSignWebhookAPIURL = "DocuSignWebhookAPIURL";
        public const String IADocuSignURL = "IADocuSignURL";
        public const String IAADocuSignURL = "IAADocuSignURL";
        public const String LandlordDocuSignURL = "LandlordDocuSignURL";
        public const String ContributionRequestFormTemplateID = "ContributionRequestFormTemplateID";
        public const String RESTAPI_BASEURL = "RESTAPI_BASEURL";
        #endregion PandaDoc and DocuSign API Keys

        public const String HostAppId = "HostAppID";
        #region ---- PasswordPolicy ----
        public static String PasswordPolicy_MinimumLength = "PasswordPolicy_MinimumLength";
        public static String PasswordPolicy_RequireUppercase = "PasswordPolicy_RequireUppercase";
        public static String PasswordPolicy_RequireLowercase = "PasswordPolicy_RequireLowercase";
        public static String PasswordPolicy_RequireDigit = "PasswordPolicy_RequireDigit";
        public static String PasswordPolicy_RequireSpecialCharacters = "PasswordPolicy_RequireSpecialCharacters";
        public static String PasswordPolicy_SpecialCharacters = "PasswordPolicy_SpecialCharacters";
        public static String PasswordPolicy_ExpirationDays = "PasswordPolicy_ExpirationDays";
        #endregion ---- PasswordPolicy ----

        public const String Twilio_accountSid = "Twilio_accountSid";
        public const String Twilio_authToken = "Twilio_authToken";
        public const String Twilio_From_NUMBER = "Twilio_From_NUMBER";
        public const String SupportMobileNumber = "SupportMobileNumber";
        public const String WorkEnvironment = "WorkEnvironment";

        public const String Savings_DefaultBeginDate = "Savings_DefaultBeginDate";
        public const String Savings_DefaultEndDate = "Savings_DefaultEndDate";
        public const String QCNUrl = "QCNUrl";

        public const String MFASecretKey = "MFASecretKey";
        public const String MFAInterval = "MFAInterval";
        public const string MFAissuer = "MFAissuer";
        public const string MFAIsActive = "MFAIsActive";
        public const string QCNMFAIsActive = "QCNMFAIsActive";
        public const string GCMFAQCNIsActive = "GCMFAQCNIsActive";
        public const string GPPMFAIsActive = "GPPMFAIsActive";

        public const string FPLAMIData_API_Token = "FPLAMIData_API_Token";
        public const string FPLAMIData_API_GetFipsCodeURL = "FPLAMIData_API_GetFipsCodeURL";
        public const string FPLAMIData_API_GetAMIDataURL = "FPLAMIData_API_GetAMIDataURL";
        public const string FPLAMIData_API_StateCodeParameter = "FPLAMIData_API_StateCodeParameter";
        public const string FPLAMIData_API_FipsCodeParameter = "FPLAMIData_API_FipsCodeParameter";
        public const String HUPRenterEnglishDocuSignTemplateID = "HUPRenterEnglishDocuSignTemplateID";
        public const String HUPHomeownerEnglishDocuSignTemplateID = "HUPHomeownerEnglishDocuSignTemplateID";
        public const String HUPRenterEnglishDocuSignReadonlyTemplateID = "HUPRenterEnglishDocuSignReadonlyTemplateID";
        public const String HUPHomeownerEnglishDocuSignReadonlyTemplateID = "HUPHomeownerEnglishDocuSignReadonlyTemplateID";
        public const String HUPLandlordApplicationTemplateID = "HUPLandlordApplicationTemplateID";

        public const string EnrollmentPortalURL = "EnrollmentPortalURL";
        public const string AntiForgeryValidateErrorMsg = "AntiForgeryValidateErrorMsg";

        #region --- PowerBI ---
        public const string PowerBIRequestUrl = "PowerBI_RequestURL";
        public const string PowerBIResource = "PowerBI_ResourceURL";
        public const string PowerBIClientId = "PowerBI_ClientID";
        public const string PowerBIClientSecret = "PowerBI_ClientSecret";
        public const string PowerBIReport_API = "PowerBI_APIUrl_Reports";
        public const string PowerBIWorkspaceId = "PowerBI_WorkspaceID";
        public const string PowerBIAPIUrl = "PowerBI_APIUrl";
        public const string PowerBIUserName = "PowerBI_UserName";
        public const string PowerBIPassword = "PowerBI_Password";
        public const string PowerBIScope = "PowerBI_Scope";
        public const string PowerBI_ReportID_Dashboard = "PowerBI_ReportID_Dashboard";
        public const string PowerBI_ReportID_PAR = "PowerBI_ReportID_PAR";
        #endregion --- PowerBI ---
    }

    public class HupApplicationLanguages
    {
        public const String Spanish = "Spanish";
        public const String Kurdish = "Kurdish";
        public const String English = "English";
    }

    public class HupApplicationTemplates
    {
        public const String HUPHomeownerEnglishDocuSignReadonlyTemplateID = "HUPHomeownerEnglishDocuSignReadonlyTemplateID";
        public const String HUPRenterEnglishDocuSignReadonlyTemplateID = "HUPRenterEnglishDocuSignReadonlyTemplateID";

        public const String HUPHomeownerSpanishDocuSignReadonlyTemplateID = "HUPHomeownerSpanishDocuSignReadonlyTemplateID";
        public const String HUPRenterSpanishDocuSignReadonlyTemplateID = "HUPRenterSpanishDocuSignReadonlyTemplateID";

        public const String HUPHomeownerKurdishDocuSignReadonlyTemplateID = "HUPHomeownerKurdishDocuSignReadonlyTemplateID";
        public const String HUPRenterKurdishDocuSignReadonlyTemplateID = "HUPRenterKurdishDocuSignReadonlyTemplateID";
    }

    public class DB
    {
        #region --- Properties ---
        public String Message = String.Empty;
        public String ConfigKey = String.Empty;
        public SqlDb DbObj;

        public long SettingId;
        public long ProgramId = 0;
        public long LpcId = 0;
        public String Module = String.Empty;
        public String Key;
        public String Value;
        public String Name = String.Empty;
        public String Extra = String.Empty;
        public String Description = String.Empty;
        public int StatusId;
        public long ByUserId;
        public static string ConnectionString;
        #endregion --- Properties ---

        #region --- Constructors ---
        public DB() { ProgramId = Session.ProgramId; Init(); }
        public DB(String module) : this() { Module = module; Init(); }
        public DB(String configKey, long programId) : this() { ProgramId = programId; Init(configKey); }
        public DB(long settingId) : this() { SettingId = settingId; }
        #endregion --- Constructors ---

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            DbObj = new SqlDb(ConfigKey);
        }
        /* Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
        private void Init(DataRow dr)
        {
            try
            {
                Value = SqlDb.CheckStringDBNull(dr["Value"]);
                if (dr.Table.Columns.Contains("ProgramID")) ProgramId = SqlDb.CheckLongDBNull(dr["ProgramID"]);
                if (dr.Table.Columns.Contains("LpcId")) LpcId = SqlDb.CheckLongDBNull(dr["LpcId"]);
                if (dr.Table.Columns.Contains("Module")) Module = SqlDb.CheckStringDBNull(dr["Module"]);
                if (dr.Table.Columns.Contains("Key")) Key = SqlDb.CheckStringDBNull(dr["Key"]);
                if (dr.Table.Columns.Contains("Name")) Name = SqlDb.CheckStringDBNull(dr["Name"]);
                if (dr.Table.Columns.Contains("Description")) Description = SqlDb.CheckStringDBNull(dr["Description"]);
                if (dr.Table.Columns.Contains("StatusID")) StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);
                /* Nov 20, 2017 | Nibha Kothari | ES-3858: CPS: AP: Legacy Scheduling & Advisor Specific Calendar */
                if (dr.Table.Columns.Contains("Extra")) Extra = SqlDb.CheckStringDBNull(dr["Extra"]);
                /* end Nov 20, 2017 | Nibha Kothari | ES-3858: CPS: AP: Legacy Scheduling & Advisor Specific Calendar */
            }
            catch { }
        }
        /* end Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
        #endregion --- Private Methods ---

        #region --- Public Methods ---
        /// <summary>
        /// Key, Value, StatusId are required. SettingId is required in case of Edit. Name, Description, LpcId, Module are optional.
        /// </summary>
        public Boolean Save()
        {
            try
            {
                if (SettingId <= 0 && Key.Length <= 0 && Value.Length <= 0) { Message = "Either SettingId or Key+Value are required."; return false; }

                Hashtable prms = new Hashtable();
                if (SettingId > 0) prms["SettingID"] = SettingId;
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["Module"] = Module;
                prms["Key"] = Key;
                prms["Value"] = Value;
                prms["Name"] = Name;
                prms["Extra"] = Extra;
                prms["Description"] = Description;
                prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                DbObj.SetSql("p_AU_Setting", prms);

                DataSet ds = DbObj.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = Key + "|" + Value + "|Error saving record.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                SettingId = SqlDb.CheckLongDBNull(dr["SettingID"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return SettingId <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.Message; return false; }
        }

        /// <summary>
        /// ProgramId is required.
        /// </summary>
        public DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                if (Name.Length > 0) prms["Name"] = Name;
                if (LpcId > 0) prms["LPCID"] = LpcId;

                DbObj.SetSql("p_GET_Settings", prms);

                return DbObj.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; return null; }
        }

        /// <summary>
        /// SettingId is required.
        /// </summary>
        public Boolean GetById()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["SettingID"] = SettingId;

                DbObj.SetSql("p_GET_Setting", prms);

                DataSet ds = DbObj.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = SettingId + "|Error fetching details.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                /* Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
                Init(dr);

                return true;
            }
            catch (Exception ex) { Message = ex.Message; return false; }
        }

        /// <summary>
        /// Module, Key are required. ProgramId is fetched from Session, if unavailable please specify. LpcId may be specified.
        /// This method is used for module-specific settings. If you want global settings, use the static GetByName method.
        /// </summary>
        public String GetByKey(String key)
        {
            try
            {
                if (String.IsNullOrEmpty(key)) throw new Exception("Key is required");

                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                if (!String.IsNullOrEmpty(Module)) prms["Module"] = Module;
                prms["Key"] = key;

                DbObj.SetSql("p_GET_Setting", prms);

                DataSet ds = DbObj.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) return String.Empty;

                DataRow dr = ds.Tables[0].Rows[0];
                /* Aug 08, 2017 | Nibha Kothari | ES-3666: UV:17321 Add Language on Customer Portal for CC Processing Error */
                Init(dr);

                return Value;
            }
            catch (Exception ex) { Message = ex.Message; return String.Empty; }
        }

        /// <summary>
        /// Key is required. If programId is not specified, it is picked up from Session. LPCId may be specified.
        /// This method is used for global settings. If you want module-specific settings, use the GetByKey method and specify the module.
        /// </summary>
        public static String GetByName(String key, long programId = 0, long lpcId = 0, String configKey = "", string connectionString = "")
        {
            try
            {
                if (key.Length <= 0) throw new Exception("Key is required");

                if (programId <= 0)
                {
                    long prgId = Session.ProgramId;
                    programId = prgId > 0 ? prgId : programId;
                }

                Hashtable prms = new Hashtable();
                if (programId > 0) prms["ProgramID"] = programId;
                if (lpcId > 0) prms["LPCID"] = lpcId;
                prms["Key"] = key;

                SqlDb db = new SqlDb(configKey);
                db.SetSql("p_GET_Setting", prms);

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ProgramID", SqlDbType.Int);
                parameters[0].Value = programId;
                parameters[1] = new SqlParameter("@LPCID", SqlDbType.Int);
                parameters[1].Value = lpcId;
                parameters[2] = new SqlParameter("@Key", SqlDbType.VarChar, 64);
                parameters[2].Value = key;
                DataSet ds = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(ConnectionString, "p_GET_Setting", parameters);
                //DataSet ds = db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) return String.Empty;

                DataRow dr = ds.Tables[0].Rows[0];
                return SqlDb.CheckStringDBNull(dr["Value"]);
            }
            catch { return String.Empty; }
        }

        public static Boolean UseClick(long programId = 0)
        {
            int extScheduler=Convert.ToInt32(EDM.Setting.DB.GetByName(Setting.Key.ExternalScheduler, programId));
            if (programId <= 0)
            {
                long prgId = Session.ProgramId;
                programId = prgId > 0 ? prgId : programId;
            }
            if (((extScheduler & programId) == extScheduler) || ((extScheduler& programId) == programId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean UseExtSched()
        {
            int extScheduler = Convert.ToInt32(GetByKey(Setting.Key.ExternalScheduler));
            if (((extScheduler & ProgramId) == extScheduler) || ((extScheduler & ProgramId) == ProgramId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Jun 27, 2016 | Nibha Kothari | ES-1605: UV:11618 QCN Volume Awards - Addition of Graphic to Admin Portal and Contractor Portal */
        public static String CheckStickyBanner()
        {
            String url = String.Empty;
            try
            {
                String sbPeriod = GetByName(Setting.Key.StickyBanner);
                if (sbPeriod.Length <= 0) return String.Empty;

                String[] temp = sbPeriod.Split('|');
                if (temp.Length <= 0) return String.Empty;

                DateTime? start = DateTime.Now.AddDays(-1), end = DateTime.Now.AddDays(1);
                if (temp[0] != null && temp[0].Length > 0) start = DateTime.Parse(temp[0]);

                if (temp.Length > 1)
                {
                    if (temp[1] != null && temp[1].Length > 0) end = DateTime.Parse(temp[1]);
                }

                if (DateTime.Now >= start && DateTime.Now < end)
                {
                    url = DB.GetByName(EDM.Setting.Key.ImageUrl) + "/Sticky_Banner.png";
                }
            }
            catch { return String.Empty; }
            return url;
        }
        /* end Jun 27, 2016 | Nibha Kothari | ES-1605: UV:11618 QCN Volume Awards - Addition of Graphic to Admin Portal and Contractor Portal */

        public static DateTime GetWeekStartPast()
        {
            try
            {
                DateTime st = DateTime.Today;
                int ctr = 0;

                String weekStart = GetByName(Setting.Key.WeekStart);
                if (String.IsNullOrEmpty(weekStart)) weekStart = "Monday";

                while (st.DayOfWeek.ToString() != weekStart && ctr < 9)
                {
                    st = st.AddDays(-1);
                    ctr++;
                }
                return st;
            }
            catch { return DateTime.Today; }
        }

        #endregion --- Public Methods ---
    }

    public class Module
    {
        public const String AdminPortal = "AP";
        public const String CustPortal = "CP";
        public const String ContraPortal = "CP";
        public const String RetailPortal = "CP";
        public const String WebService = "WS";
        public const String RESTAPI = "RESTAPI";
        public const String WinService = "UT";
        public const String VMF = "VMF";
        public const String SelfAudit = "CP";
        public const String WebAPI = "API";
        public const String HomeDepot = "HD";
        public const String QCN = "QCN";
        public const String NEATAPI = "NEATAPI";
    }  
    public class NavModule
    {
        public const String Admin = "AP";
        public const String Customer = "CP";
        public const String Contractor = "QCN";
        public const String HomeDepot = "HD";
        public const String Finance = "VMF";

        public Hashtable GetAll()
        {
            Hashtable prms = new Hashtable();
            prms[Admin] = "Admin";
            prms[Customer] = "Customer";
            prms[Contractor] = "Contractor";
            prms[HomeDepot] = "Home Depot";
            prms[Finance] = "Finance";
            return prms;
        }
    }

    public class Refer3
    {
        public const String AdminPortal = "ADMIN";
        public const String CustPortal = "CUSTOMER";
        public const String QCNPortal = "CONTRACTOR";
        public const String HDPortal = "UPGRADEAPP";
        public const String Kiosk = "KIOSK";
    }

    public static class Status
    {
        public const int Active = 1;
        public const int Disabled = 2;
    }
    public static class MFAStatus
    {
        public const int Active = 1;
        public const int Disabled = 0;
    }
    public static class AccessType
    {
        public const int Public = 1;
        public const int Private = 2;
    }
    public static class LPCAccess
    {
        public const int Yes = 1;
        public const int No = 2;
    }
    public static class HistoryKey
    {
        public const int AdminUser = 1;  //For Reset MFA Admin User
        public const int GPPCustomer = 8;   //Reset MFA for GPPCustomer
        public const string ResetMFA = "HistoryKey";   
    }
    public static class StatusName
    {
        public const String Active = "Active";
    }

    public static class Choice
    {
        public const int Yes = 1;
        public const int No = 2;
    }

    public static class HouseRental
    {
        public const int Yes = 0;
        public const int No = 1;
    }

    public static class ApplicationStatus
    {
        public const int Open = 1;
        public const int Closed = 2;
    }

    public class DocuSignRoleName
    {
        public const String Renter = "Renter";
        public const String Homeowner = "Homeowner";
    }

    public class DocuSignKey
    {
        public const String RecipientId = "1";
        public const String RoutingOrder = "1";
    }
    public class Fields
    {
        public static String Success = "Successs";
        public static String Failed = "Failed";
        public static String StrType = "strType";
        public static string OptedIn = "opted in";
        public static string OptedOut = "opted out";

        public static String CodeID = "CodeID";
        public static String CodeName = "CodeName";
        public static String CodeValue = "CodeValue";
        public static String CodeSeq = "CodeSeq";
        public static String UserID = "UserID";
        public static String Password = "Password";
        public static String StatusID = "StatusID";
        public static String RecapEmailsStatus = "RecapEmailsStatus";
        public static String Status = "Status";
        public static String TranDate = "TranDate";
        public static String CompanyStatus = "CompanyStatus";
        public static String PageNum = "PageNum";
        public static String PageSize = "PageSize";
        public static String ParentID = "ParentID";
        public static String ParentName = "ParentName";
        public static String ObjectID = "ObjectID";
        public static String ObjectName = "ObjectName";
        public static String UnitID = "UnitID";
        public static String UnitName = "UnitName";
        public static String ProgramModel = "ProgramModel";
        public static String ProgramModelDesc = "ProgramModelDesc";
        public static String EquipCapacity = "EquipCapacity";
        public static String OldUrlToRedirect = "OldUrlToRedirect";
        public static String RetailProcessedBy = "RetailProcessedBy";
        public static String ProjectManagerID = "ProjectManagerID";
        public static String FieldSupervisorID = "FieldSupervisorID";

        #region Advisor call followp mahesh

        #region mahesh-6/6/2014 popup coming from site id and customer name

        public static string FromRadWindow = "FromRadWindow";
        public static string ShowPopupTrue = "1";
        public static string SiteIDClickUrlToRedirect = "SiteIDClickUrlToRedirect";
        public static string EndCall = "EndCall";

        #endregion mahesh-6/6/2014 popup coming from site id and customer name

        #endregion Advisor call followp mahesh

        public static String GroupID = "GroupID";
        public static String GroupName = "GroupName";
        public static String GroupType = "GroupType";

        public static String Address1 = "Address1";
        public static String Address2 = "Address2";
        public static String City = "City";
        public static String StateID = "StateID";
        public static String State = "State";
        public static String StateAbbr = "StateAbbr";
        public static String StateFullName = "StateFullName";
        public static String Zip = "Zip";
        public static String Zip2 = "Zip2";
        public static String ZipCode = "ZipCode";
        public static String Longitude = "Longitude";
        public static String Latitude = "Latitude";
        public static String Phone = "Phone";
        public static String Email = "Email";

        public static String CompanyLogoPath = "CompanyLogoPath";
        public static String NewHome = "NewHome";

        public static String FullName = "FullName";
        public static String FirstName = "FirstName";
        public static String LastName = "LastName";
        public static String UserTypeID = "UserTypeID";
        public static String Token = "Token";

        public static String IsMFAEnabled = "IsMFAEnabled";

        public static String RoleID = "RoleID";
        public static String RoleName = "RoleName";
        public static String GrandParentName = "GrandParentName";
        public static String ReadAccess = "ReadAccess";
        public static String WriteAccess = "WriteAccess";
        public static String UserCount = "UserCount";

        public static String RegionID = "RegionID";
        public static String RegionName = "RegionName";
        public static String StartDate = "StartDate";
        public static String EndDate = "EndDate";

        public static String MeasureID = "MeasureID";
        public static String MeasureName = "MeasureName";

        public static String ProgramID = "ProgramID";
        public static String ProgramName = "ProgramName";
        public static String ProgramCode = "ProgramCode";
        public static String Programs = "Programs";

        public static String SubMeasureID = "SubMeasureID";
        public static String ProgramSMIID = "ProgramSMIID";
        public static String SubMeasureName = "SubMeasureName";
        public static String ProgramIncentiveDataID = "ProgramIncentiveDataID";

        public static String NavID = "NavID";
        public static String NavName = "NavName";
        public static String NavUrl = "NavUrl";
        //public static String NavSeq = "NavSeq";

        public static String LogName = "LogName";
        public static String Year = "Year";
        public static String Month = "Month";

        public static String CodeDesc = "CodeDesc";
        public static String CodeText = "CodeText";

        public static String DocTypeID = "DocTypeID";
        public static String DocTypeName = "DocTypeName";
        public static String DocTypeParam = "DocTypeParam";
        public static String DocTypeParamValue = "DocTypeParamValue";
        public static String DocTypeVersion = "DocTypeVersion";
        public static String DocTypeSeq = "DocTypeSeq";
        public static String DocTypeGroupID = "DocTypeGroupID";

        public static String CompanyID = "CompanyID";
        public static String CompanyType = "CompanyType";
        public static String CompanyName = "CompanyName";
        public static String CompanyCode = "CompanyCode";
        public static String Website = "Website";

        public static String SettingID = "SettingID";

        public static String ProjectID = "ProjectID";
        public static String ProjectStatus = "ProjectStatus";
        public static String ProjectUniqueID = "ProjectUniqueID";
        public static String eScore = "Residential Services";
        public static String HUP = "Home Uplift Program";
        public static String GPP = "Green Connect";
        public static String ScheduleNotes = "ScheduleNotes";
        public static String StartDt = "StartDt";
        public static String SelfSchedule = "SelfSchedule";
        public static String Scheduled = "Scheduled";

        public static String MeasureResult = "MeasureResult";
        public static String CFLReceived = "CFLReceived";
        public static String SHReceived = "SHReceived";

        public static String LPCID = "LPCID";
        public static String SchedulingMethod = "SchedulingMethod";
        public static String HearAboutProgram = "HearAboutProgram";
        public static String HearAboutProgram1 = "HearAboutProgram1";
        public static String HearAboutProgram2 = "HearAboutProgram2";

        public static String CallLog = "CallLog";
        public static String AutoStart = "AutoStart";
        public static String StartCallLog = "StartCallLog";

        public static String ServiceAddressID = "ServiceAddressID";
        public static String CustomerSAID = "CustomerSAID";
        public static String CustomerSAUniqueID = "CustomerSAUniqueID";
        public static String EmailAddress = "EmailAddress";
        public static String Title = "Title";
        public static String LeadsSentTo = "LeadsSentTo";
        public static String FieldSuperVisor = "FieldSuperVisor";
        public static String LeadResolution = "LeadResolution";
        public static String DisplayScoreCard = "DisplayScoreCard";
        public static String CustomerLoginID = "CustomerLoginID";
        public static String AccountNumber = "AccountNumber";
        public static String LPCName = "LPCName";
        public static String AvgBillAmt = "AvgBillAmt";
        public static String CustomerID = "CustomerID";
        public static String DoNotContact = "DoNotContact";
        public static String CSAID = "CSAID";
        public static String SAID = "SAID";
        public static String SANickName = "SANickName";
        public static String HouseType = "HouseType";

        public static String LookupConfig = "LookupConfig";

        public static String Advisor = "Advisor";
        public static String AdvisorEmail = "AdvisorEmail";
        public static String AuditorID = "AuditorID";
        public static String Subject = "Subject";
        public static String Start = "Start";
        public static String End = "End";
        public static String ScheduleDateTime = "ScheduleDateTime";
        public static String TimeSlotStart = "TimeSlotStart";
        public static String TimeSlotEnd = "TimeSlotEnd";
        public static string AdvisorID = "AdvisorID";
        public static string StartDateTime = "StartDateTime";
        public static string EndDateTime = "EndDateTime";
        public static string AdvisorTimeOffID = "AdvisorTimeOffID";

        public static string SiteID = "SiteID";

        public static String DamagedID = "DamagedID";

        public static String PIStatus = "PIStatus";
        public static String PID = "PID";

        public static String PayoutID = "PayoutID";
        public static String PayoutName = "PayoutName";
        public static String PayoutIncentiveID = "PayoutIncentiveID";
        public static String Amount = "Amount";

        public static String ProgramIncentiveID = "ProgramIncentiveID";
        public static String InstallDate = "InstallDate";
        public static String PIMeasureID = "PIMeasureID";
        public static String MultiUnit = "MultiUnit";
        public static String MaxUnit = "MaxUnit";
        public static String InstallCost = "InstallCost";
        public static String SubMeasureQty = "SubMeasureQty";
        public static String EditQty = "EditQty";

        public static String CustomPageID = "CustomPageID";

        public static String QCNNotificationPageID = "QCNNotificationPageID";
        public static String LeadID = "LeadID";

        //ES-944
        public static String TitleKey = "TitleKey";

        public static String TitleValue = "TitleValue";
        public static String TitleDisplay = "TitleDisplay";

        public static String CUSTOMER_ID = "CUSTOMER_ID";
        public static String PREMISE_ID = "PREMISE_ID";
        public static String PremiseID = "PremiseID";

        public static String ProductID = "ProductID";
        public static String ProductName = "ProductName";

        public static String ValidateName = "ValidateName";
        public static String SubmittedBy = "SubmittedBy";

        public static String FromDate = "FromDate";
        public static String WithAdvisor = "WithAdvisor";
        public static String ModelID = "ModelID";
        public static String Customer = "Customer";
        public static String CustomerAddress = "CustomerAddress";
        public static String ScheduleEndDateTime = "ScheduleEndDateTime";
        public static String MilesDriven = "MilesDriven";
        public static String Distance = "Distance";
        public static String ScheduleDay = "ScheduleDay";
        public static String ModelTimeSlotStart = "ModelTimeSlotStart";
        public static String ModelTimeSlotEnd = "ModelTimeSlotEnd";
        public static String AdvisorAddress = "AdvisorAddress";
        public static String AdvisorLatitude = "AdvisorLatitude";
        public static String AdvisorLongitude = "AdvisorLongitude";
        public static String AdvisorStartTime = "AdvisorStartTime";
        public static String AdvisorEndTime = "AdvisorEndTime";

        public static String VisitType = "VisitType";
        public static String ApptType = "ApptType";
        public static String CustomerSiteID = "CustomerSiteID";
        public static String Description = "Description";
        public static String Sequence = "Sequence";
        public static String ByUserID = "ByUserID";
        public static String StrFieldPlanner = "fieldplanner";
        public static String StrAdvisorVacation = "advisorvacation";
        public static String DefaultStrType = "timeoffmulti";
        public static String DefaultTime = "23:59:59";
        public static String AdvisorName = "AdvisorName";
        public static String WeekView = "WeekView";
        public static String Recommended = "Recommended";
        public static String AddressType = "AddressType";
        public static String VacationType = "VacationType";
        public static String Vacation = "Vacation";
        public static String Daily = "Daily";
        public static String Weekly = "Weekly";
        public static String LunchMins = "LunchMins";
        public static String FuelCost = "FuelCost";
        public static String FuelEconomy = "FuelEconomy";
        public static String TimeSlotFor7 = "07:00";
        public static String TimeZone = "TimeZone";
        public static String Scope = "Scope";
        public static String NormalScope = "normal";
        public static String MaxGrade = "MaxGrade";
        public static String MaxGradeDateTime = "MaxGradeDateTime";
        public static String MinGrade = "MinGrade";
        public static String MinGradeDateTime = "MinGradeDateTime";
        public static String SchedArgs = "SchedArgs";
        public static String Schedule = "Schedule";

        public static String FromMin = "fromMin";
        public static String FromMax = "fromMax";
        public static String From = "from";
        
        public static String TOMin = "toMin";        
        public static String TOMax = "toMax";
        public static String TO = "to";

        public static String LnkSch = "lnkSch";
        public static String ShowNew = "Show New";
        public static String ShowAll = "Show All";

        public static String Relocated = "Relocated";
        public static String TimeOff = "TimeOff";
        public static String CustomerName = "CustomerName";
        public static String ServiceAddress = "ServiceAddress";
        public static String ProjectTypeID = "ProjectTypeID";
        public static String StartEST = "StartEST";
        public static String Auditor = "Auditor";
        public static String AuditorName = "AuditorName";
        public static String EndEST = "EndEST";
        public static String UserName = "UserName";
        public static String SSID = "SSID";

        public static String CompanySubType = "CompanySubType";
        public static String CellPhone = "CellPhone";
        public static String Fax = "Fax";
        public static String MailingAddress1 = "MailingAddress1";
        public static String MailingAddress2 = "MailingAddress2";
        public static String MailingCity = "MailingCity";
        public static String MailingStateID = "MailingStateID";
        public static String MailingZip = "MailingZip";
        public static String MailingZip2 = "MailingZip2";
        public static String MailingPhone = "MailingPhone";
        public static String MailingCellPhone = "MailingCellPhone";
        public static String MailingFax = "MailingFax";
        public static String YearsInBusiness = "YearsInBusiness";
        public static String NoOfEmployees = "NoOfEmployees";
        public static String MoreInfo = "MoreInfo";
        public static String AcctMgrID = "AcctMgrID";
        public static String IsYourMailingAddressSame = "IsYourMailingAddressSame";
        public static String FedTaxID = "FedTaxID";
        public static String BankName = "BankName";
        public static String RoutingNo = "RoutingNo";
        public static String AccountNo = "AccountNo";
        public static String VendorNumber = "VendorNumber";
        public static String CompanyLogo = "CompanyLogo";
        public static String DistrictID = "DistrictID";
        public static String AccountType = "AccountType";
        public static String AllowLookup = "AllowLookup";
        public static String AcctMgrEmail = "AcctMgrEmail";
        public static String AcctMgrName = "AcctMgrName";
        public static String AcctMgrPhone = "AcctMgrPhone";
        public static String CompanyContactEmail = "CompanyContactEmail";
        public static String CompanyContactName = "CompanyContactName";
        public static String MailingStateAbbr = "MailingStateAbbr";

        public static String SAName = "SAName";
        public static string QCNUserID = "QCNUserID";

        public static String DPID = "DPID";
        public static String ProgramDelivery = "ProgramDelivery";
        public static String DeliveryModel = "DeliveryModel";
        public static String UserType = "UserType";
        public static String ApplicationStatus = "ApplicationStatus";
        public static String ApplicationContact = "ApplicationContact";
        public static String QualificationPreference = "QualificationPreference";
        public static String QualificationPrefAMI = "QualificationPrefAMI"; 
        public static String QualificationPrefFPL = "QualificationPrefFPL";
        public static String QualificationPrefNone = "QualificationPrefNone";
        public static String QualificationPrefBlend = "QualificationPrefBlend";
        public static String AllowRentals = "AllowRentals";
        public static String RecruitmentStrategy = "RecruitmentStrategy";
        public static String PointOfContact = "PointOfContact";
        public static int SmallBusiness = 1;
        public static int ValleyBusiness = 5;
        public static String BusinessCertification = "Business Certification";
        public static String TrainingID = "TrainingID";
        public static String TrackingId = "TrackingId";

        #region --- HD Performance Report ---

        public static String CurrPeriodEnd = "CurrPeriodEnd";
        public static String PrevPeriodEnd = "PrevPeriodEnd";
        public static String PrevPeriodStart = "PrevPeriodStart";
        public static String CurrPeriodStart = "CurrPeriodStart";
        public static String Rank = "Rank";
        public static String District = "District";
        public static String StoreNumber = "StoreNumber";
        public static String StoreName = "StoreName";
        public static String CurrJobsSold = "CurrJobsSold";
        public static String PrevJobsSold = "PrevJobsSold";
        public static String CurrSales = "CurrSales";
        public static String PrevSales = "PrevSales";
        public static String CurrRefrigeratorsCount = "CurrRefrigeratorsCount";
        public static String PrevRefrigeratorsCount = "PrevRefrigeratorsCount";
        public static String CurrClothesWasherCount = "CurrClothesWasherCount";
        public static String PrevClothesWasherCount = "PrevClothesWasherCount";
        public static String CurrDishWasherCount = "CurrDishWasherCount";
        public static String PrevDishWasherCount = "PrevDishWasherCount";
        public static String CurrDoorCount = "CurrDoorCount";
        public static String PrevDoorCount = "PrevDoorCount";
        public static String CurreScoresCompleted = "CurreScoresCompleted";
        public static String PreveScoresCompleted = "PreveScoresCompleted";
        public static String CurrLeadInfo = "CurrLeadInfo";
        public static String PrevLeadInfo = "PrevLeadInfo";
        public static String ProviderName = "ProviderName";
        public static String CurrHVACCount = "CurrHVACCount";
        public static String PrevHVACCount = "PrevHVACCount";
        public static String CurrTuneUpCount = "CurrTuneUpCount";
        public static String PrevTuneUpCount = "PrevTuneUpCount";
        public static String CurrDuctCount = "CurrDuctCount";
        public static String PrevDuctCount = "PrevDuctCount";
        public static String CurrAtticCount = "CurrAtticCount";
        public static String PrevAtticCount = "PrevAtticCount";
        public static String CurrAirSealingCount = "CurrAirSealingCount";
        public static String PrevAirSealingCount = "PrevAirSealingCount";
        public static String CurrWaterHeatingCount = "CurrWaterHeatingCount";
        public static String PrevWaterHeatingCount = "PrevWaterHeatingCount";
        public static String HDEName = "HDEName";
        public static String Territory = "Territory";
        public static String CurrWindowCount = "CurrWindowCount";
        public static String PrevWindowCount = "PrevWindowCount";

        #endregion --- HD Performance Report ---

        #region --- Find Contractor - CP ---

        public static String IsQCNCompany = "IsQCNCompany";
        public static String CreatedDate = "CreatedDate";
        public static String AddToWishlistDate = "AddToWishlistDate";
        public static String Message = "Message";
        public static String CustomerPhone = "CustomerPhone";
        public static String CCID = "CCID";

        #endregion --- Find Contractor - CP ---

        public static String Force = "Force";
        public static String InspExists = "InspExists";
        public static String SelfRebate = "SelfRebate";
        public static String InstallQty = "InstallQty";
        public static String StoreID = "StoreID";
        public static String PayoutCustomer = "PayoutCustomer";
        public static String Referral1 = "Referral1";
        public static String Referral2 = "Referral2";
        public static String Notes = "Notes";
        public static String InternalNotes = "InternalNotes";
        public static String AuthorizeTVAasLPC = "AuthorizeTVAasLPC";
        public static String ApprovedforSharedDocuments = "ApprovedforSharedDocuments";
        public static String ActionReason = "ActionReason";
        public static String DppNotes = "DppNotes";
        public static String TargetNoOfHomes = "TargetNoOfHomes";
        public static String CoverPageComments = "CoverPageComments";
        public static String CallDisposition = "CallDisposition";
        public static String CallDuration = "CallDuration";
        public static String CallType = "CallType";
        public static String CallLogID = "CallLogID";
        public static String ApptStart = "ApptStart";
        public static String ID = "ID";
        public static String NotifyCust = "NotifyCust";
        public static String NotifyContractor = "NotifyContractor";
        public static String AuditorTimeZone = "AuditorTimeZone";
        public static String Duration = "Duration";
        public static String Policy = "Policy";
        public static String PICRCUnitID = "PICRCUnitID";
        public static String ReasonDescription = "ReasonDescription";
        public static String InspectionCodes = "InspectionCodes";
        public static String ClientSelectColumn = "ClientSelectColumn";
        public static String CheckNum = "CheckNum";
        public static String TranNo = "TranNo";
        public static String TranNoW = "TranNoW";
        public static String AccountNum = "AccountNum";
        public static String DateCleared = "DateCleared";
        public static String ChangeStatusW = "ChangeStatusW";
        public static String PaidOn = "PaidOn";
        public static String ChangeStatus = "ChangeStatus";
        public static String SelectChk = "SelectChk";
        public static String CustomerAmount = "CustomerAmount";
        public static String LPCAmount = "LPCAmount";
        public static String CoOpDollars = "CoOpDollars";
        public static String IncentiveAmountPaid = "IncentiveAmountPaid";
        public static String ReCalculateRebate = "ReCalculateRebate";            
        public static String HeatPumpAmount = "HeatPumpAmount";
        public static String LastVisitAmount = "LastVisitAmount";
        public static String FirstVisitAmount = "FirstVisitAmount";
        public static String InQCRaw = "InQCRaw";
        public static String InQC = "InQC";
        public static String grdAdmin = "Admin";
        public static String grdCustomer = "Customer";
        public static String grdContractor = "Contractor";
        public static String grdHomeDepot = "Home Depot";
        public static String grdFinanace = "Finance";
        public static String StatusName = "StatusName";
        public static String LeftNavDisplayStatus = "LeftNavDisplayStatus";
        public static String LeftNavDisplay = "LeftNavDisplay";
        public static String ExecSeq = "ExecSeq";
        public static String ByUser = "ByUser";
        public static String LeftNavDispStatus = "LeftNavDispStatus";
        public static String Module = "Module";
        public static String WriteFlagStatus = "WriteFlagStatus";
        public static String NavSequence = "NavSequence";
        public static String WriteFlag = "WriteFlag";
        public static String XmlString = "XmlString";
        public static String Permission = "Permission";
        public static String PgElement = "PgElement";
        public static String PgTitle = "PgTitle";
        public static String DeliveryCost = "DeliveryCost";

        public static String ReqESBtnName = "ReqESBtnName";
        public static String ReqESBtnVisible = "ReqESBtnVisible";
        public static String ReqESBtnPerm = "ReqESBtnPerm";
        public static String ReqPIBtnName = "ReqPIBtnName";
        public static String ReqPIBtnUrl = "ReqPIBtnUrl";
        public static String ReqPIBtnVisible = "ReqPIBtnVisible";
        public static String ReqPIBtnPerm = "ReqPIBtnPerm";
        public static String InUse = "InUse";
        public static String TranDateFtd = "TranDateFtd";

        public static String ApprovedStartDate = "ApprovedStartDate";
        public static String ApprovedEndDate = "ApprovedEndDate";
        public static String CreatedStartDate = "CreatedStartDate";
        public static String CreatedEndDate = "CreatedEndDate";

        public static String LPCIDs = "LPCIDs";
        public static String DistrictIDs = "DistrictIDs";
        public static String Todate = "Todate";
        public static String ContractorCompanyID = "ContractorCompanyID";
        public static String Days = "Days";
        public static String DocumentStatus = "DocumentStatus";
        public static String p_StartDate = "p_StartDate";
        public static String p_EndDate = "p_EndDate";
        public static String ContractorID = "ContractorID";
        public static String ContractorStatus = "ContractorStatus";
        public static String Validation = "Validation";
        public static String Required = "Required";
        public static String PhotoPath = "PhotoPath";

        public static String FIRST_NAME = "FIRST_NAME";
        public static String LAST_NAME = "LAST_NAME";
        public static String EMAIL = "EMAIL";
        public static String PHONE_1 = "PHONE_1";
        public static String BUSINESS_NAME = "BUSINESS_NAME";
        public static String ADDRESS1 = "ADDRESS1";
        public static String ACCOUNT_ID = "ACCOUNT_ID";
        public static String RATE_CODE = "RATE_CODE";
        public static String ADDRESS2 = "ADDRESS2";
        public static String SERVICE_CITY = "SERVICE_CITY";
        public static String SERVICE_STATE = "SERVICE_STATE";
        public static String SERVICE_ZIP_CODE = "SERVICE_ZIP_CODE";
        public static String SERVICE_ZIP_CODE2 = "SERVICE_ZIP_CODE2";
        public static String LATITUDE = "LATITUDE";
        public static String LONGITUDE = "LONGITUDE";
        public static String MAILING_ADDRESS1 = "MAILING_ADDRESS1";
        public static String MAILING_ADDRESS2 = "MAILING_ADDRESS2";
        public static String MAILING_CITY = "MAILING_CITY";
        public static String MAILING_STATE = "MAILING_STATE";
        public static String MAILING_ZIP = "MAILING_ZIP";
        public static String MAILING_ZIP2 = "MAILING_ZIP2";

        public static String Type = "Type";
        public static String ProjectDate = "ProjectDate";
        public static String InspectionRequest = "InspectionRequest";
        public static String InspectionNo = "InspectionNo";
        public static String EvalRequest = "EvalRequest";
        public static String NexteScore = "NexteScore";
        public static String ESEID = "ESEID";
        public static String TranDateRaw = "TranDateRaw";
        public static String LoginId = "LoginId";
        public static String DepartmentID = "DepartmentID";
        public static String RegionIDs = "RegionIDs";
        public static String InExternal = "InExternal";
        public static String SelectedAdvisor = "SelectedAdvisor";
        public static String FutureAppts = "FutureAppts";

        public static String AddressRequired = "AddressRequired";
        public static String IsLoginEmail = "IsLoginEmail";
        public static String ShowDistrict = "ShowDistrict";
        public static String DistrictRequired = "DistrictRequired";
        public static String InnerNavUrl = "InnerNavUrl";

        #region WAP
        public static String ApplicationID = "ApplicationID";
        public static String HouseholdMembers = "HouseholdMembers";
        public static String FPLID = "FPLID";
        public static String AnnualIncome = "AnnualIncome";
        public static String Usage = "Usage";
        public static String Unit = "Unit";
        public static String AmtPerMonth = "AmtPerMonth";
        public static String CSAGIId = "CSAGIId";
        public static String MemberID = "MemberID";
        public static String HasIncome = "HasIncome";
        public static String BirthDate = "BirthDate";
        public static String Citizenship = "Citizenship";
        public static String Encrypt = "Encrypt";
        public static String AuditType = "AuditType";
        public static String FileName = "FileName";
        public static String WeatherFile = "WeatherFile";
        public static String IsApplicant = "IsApplicant";

        #endregion

        #region NEAT
        public static String Category = "Category";
        public static String Form = "Form";
        public static String Location = "Location";
        public static String Field = "Field";

        public static String WallCode = "WallCode";
        public static String OldWallCode = "OldWallCode";
        public static String NewWallCode = "NewWallCode";
        public static String JobID = "JobID";
        public static String StudSizeID = "StudSizeID";
        public static String OrientationID = "OrientationID";
        public static String WallExposureID = "WallExposureID";
        public static String WallExteriorID = "WallExteriorID";
        public static String WallLoadTypeID = "WallLoadTypeID";
        public static String Area = "Area";
        public static String WallExistInsulationID = "WallExistInsulationID";
        public static String RValue = "RValue";
        public static String WallAddInsulationID = "WallAddInsulationID";
        public static String Cost = "Cost";
        public static String MeasureNumber = "MeasureNumber";
        public static String Comment = "Comment";

        public static String WindowCode = "WindowCode";
        public static String NewWindowCode = "NewWindowCode";
        public static String WindowTypeID = "WindowTypeID";
        public static String WindowFrameTypeID = "WindowFrameTypeID";
        public static String WindowGlazingTypeID = "WindowGlazingTypeID";
        public static String InteriorShadeTypeID = "InteriorShadeTypeID";
        public static String ExteriorShadeTypeID = "ExteriorShadeTypeID";
        public static String WindowLeakinessID = "WindowLeakinessID";
        public static String Width = "Width";
        public static String Height = "Height";
        public static String Number = "Number";
        public static String WindowRetrofitStatusID = "WindowRetrofitStatusID";
        public static String IncludeInSIR = "IncludeInSIR";
        public static String CostWeatherize = "CostWeatherize";
        public static String CostReplace = "CostReplace";
        public static String CostAddStorm = "CostAddStorm";
        public static String CostLowE = "CostLowE";
        public static String SystemCode = "SystemCode";
        public static String NewSystemCode = "NewSystemCode";

        public static String DoorCode = "DoorCode";
        public static String NewDoorCode = "NewDoorCode";
        public static String DoorTypeID = "DoorTypeID";
        public static String DoorStormID = "DoorStormID";
        public static String DoorLeakinessID = "DoorLeakinessID";
        public static String ReplacementRequired = "ReplacementRequired";


        public static String AtticCode = "AtticCode";
        public static String NewAtticCode = "NewAtticCode";
        public static String AtticTypeID = "AtticTypeID";
        public static String FinAtticTypeID = "FinAtticTypeID";
        public static String AtticExistInslID = "AtticExistInslID";
        public static String ExistDepth = "ExistDepth";
        public static String AtticAddInslID = "AtticAddInslID";
        public static String MaxDepth = "MaxDepth";
        public static String AddedR = "AddedR";
        public static String RoofColorID = "RoofColorID";

        public static String JoistSpacing = "JoistSpacing";

        public static String FoundationCode = "FoundationCode";
        public static String NewFoundationCode = "NewFoundationCode";
        public static String FoundationTypeID = "FoundationTypeID";
        public static String Perimeter = "Perimeter";
        public static String CeilingRValue = "CeilingRValue";
        public static String SillPerimeter = "SillPerimeter";
        public static String FloorJoistHeight = "FloorJoistHeight";
        public static String PerimeterExposed = "PerimeterExposed";
        public static String WallHeight = "WallHeight";
        public static String WallExposed = "WallExposed";
        public static String WallRValue = "WallRValue";
        public static String FoundationInslModeID = "FoundationInslModeID";
        public static String AddSillInsulID = "AddSillInsulID";
        public static String AddFloorInsulID = "AddFloorInsulID";
        public static String AddFoundationlInsulID = "AddFoundationlInsulID";
        public static String FloorCost = "FloorCost";
        public static String SillCost = "SillCost";
        public static String WallCost = "WallCost";
        public static String CoolingEquipTypeID = "CoolingEquipTypeID";
        public static String AreaCooled = "AreaCooled";
        public static String OutputCapacity = "OutputCapacity";
        public static String SEER = "SEER";
        public static String YearPurchased = "YearPurchased";
        public static String Manufacturer = "Manufacturer";
        public static String Model = "Model";
        public static String Replace = "Replace";
        public static String TuneUp = "TuneUp";


        public static String PhotoType = "PhotoType";
        public static String UnitSequence = "UnitSequence";
        public static String Comments = "Comments";
        public static String Original_Name = "Original_Name";
        public static String System_Name = "System_Name";
        public static String BaseLocation = "BaseLocation";
        public static String RelLocation = "RelLocation";
        public static String ReportFlag = "ReportFlag";
        public static String Storage = "Storage";
        public static String ReportSequence = "ReportSequence";
        public static String NewUnitName = "NewUnitName";
        public static String PhotoMasterId = "PhotoMasterId";
        public static String SystemName = "SystemName";
        public static String ISUploaded = "ISUploaded";
        public static String ImagePath = "ImagePath";

        public static String WAPPressureID = "WAPPressureID";
        public static String InitialPressure = "InitialPressure";
        public static String FinalPressure = "FinalPressure";
        public static String NewWAPPressureID = "NewWAPPressureID";
        public static String PressureID = "PressureID";

        public static String DoorSealConditionID = "DoorSealConditionID";
        public static String ExistDefrostID = "ExistDefrostID";
        public static String ExistManufacturer = "ExistManufacturer";
        public static String ExistModel = "ExistModel";
        public static String ExistSize = "ExistSize";
        public static String ExistHeight = "ExistHeight";
        public static String ExistWidth = "ExistWidth";
        public static String ExistStyleID = "ExistStyleID";
        public static String LocationID = "LocationID";
        public static String LabelYearID = "LabelYearID";
        public static String LabelkWhPerYear = "LabelkWhPerYear";
        public static String MeterEnergyReading = "MeterEnergyReading";
        public static String MeterEnergyInterval = "MeterEnergyInterval";
        public static String MeterIncludesDefrost = "MeterIncludesDefrost";
        public static String MeterManualDefrost = "MeterManualDefrost";
        public static String MeterTemperature = "MeterTemperature";
        public static String ReplaceManufacturer = "ReplaceManufacturer";
        public static String ReplaceModel = "ReplaceModel";
        public static String ReplaceCapacity = "ReplaceCapacity";
        public static String ReplaceHeight = "ReplaceHeight";
        public static String ReplaceWidth = "ReplaceWidth";
        public static String ReplaceDepth = "ReplaceDepth";
        public static String ReplacekWhPerYear = "ReplacekWhPerYear";
        public static String ReplaceMCost = "ReplaceMCost";
        public static String ReplaceLCost = "ReplaceLCost";
        public static String ReplaceLife = "ReplaceLife";
        public static String ReplaceStyleID = "ReplaceStyleID";
        public static String ReplaceDefrostID = "ReplaceDefrostID";
        public static String SupplyMaterialID = "SupplyMaterialID";
        public static String SupplyID = "SupplyID";
        public static String Code = "Code";
        public static String Library = "Library";
        public static String Units = "Units";
        public static String UnitCost = "UnitCost";
        public static String SupplierID = "SupplierID";
        public static String Capacity = "Capacity";
        public static String Depth = "Depth";
        public static String kWhPerYear = "kWhPerYear";
        public static String Life = "Life";
        public static String StyleID = "StyleID";
        public static String WAPBlowerDoorID = "WAPBlowerDoorID";
        public static String NewWAPBlowerDoorID = "NewWAPBlowerDoorID";
        public static String WAPZonePressureID = "WAPZonePressureID";
        public static String NewWAPZonePressureID = "NewWAPZonePressureID";
        public static String BlowerDoorTest = "BlowerDoorTest";

        public static String BlowerDoorID = "BlowerDoorID";
        public static String Date = "Date";
        public static String ConductedDuringID = "ConductedDuringID";
        public static String Equipment = "Equipment";
        public static String MeasuredCFM = "MeasuredCFM";
        public static String MeasuredPressure = "MeasuredPressure";
        public static String CorrectedCFM = "CorrectedCFM";
        public static String DefrostID = "DefrostID";
        public static String ModelYears = "ModelYears";
        public static String Style = "Style";
        public static String Defrost = "Defrost";

        public static String LightCode = "LightCode";
        public static String NewLightCode = "NewLightCode";
        public static String CFWatts = "CFWatts";
        public static String ExistWatts = "ExistWatts";
        public static String LightLocationID = "LightLocationID";
        public static String LightTypeID = "LightTypeID";
        public static String OnHous = "OnHous";
        public static String Room = "Room";
        public static String EvaluateDuctSealing = "EvaluateDuctSealing";
        public static String DuctLeakageMethodID = "DuctLeakageMethodID";
        public static String AirLeakRedCost = "AirLeakRedCost";
        public static String DuctSealCost = "DuctSealCost";
        public static String PreDuctSealSupplyPa = "PreDuctSealSupplyPa";
        public static String PreDuctSealReturnPa = "PreDuctSealReturnPa";
        public static String PostDuctSealSupplyPa = "PostDuctSealSupplyPa";
        public static String PostDuctSealReturnPa = "PostDuctSealReturnPa";
        public static String PreDuctSealTotCfm = "PreDuctSealTotCfm";
        public static String PreDuctSealTotDuctPa = "PreDuctSealTotDuctPa";
        public static String PreDuctSealOutsideCfm = "PreDuctSealOutsideCfm";
        public static String PreDuctSealOutsideDuctPa = "PreDuctSealOutsideDuctPa";
        public static String PostDuctSealTotCfm = "PostDuctSealTotCfm";
        public static String PostDuctSealTotDuctPa = "PostDuctSealTotDuctPa";
        public static String PostDuctSealOutsideCfm = "PostDuctSealOutsideCfm";
        public static String PostDuctSealOutsideDuctPa = "PostDuctSealOutsideDuctPa";
        public static String PreDuctSealClosePa = "PreDuctSealClosePa";
        public static String PostDuctSealClosePa = "PostDuctSealClosePa";
        public static String PreInfCfm = "PreInfCfm";
        public static String PreInfPa = "PreInfPa";
        public static String PostInfCfm = "PostInfCfm";
        public static String PostInfPa = "PostInfPa";
        public static String PostDuctSealCfm = "PostDuctSealCfm";
        public static String PostDuctSealPa = "PostDuctSealPa";
        public static String PreDuctSealCloseCfm = "PreDuctSealCloseCfm";
        public static String PreDuctSealCloseDiffPa = "PreDuctSealCloseDiffPa";
        public static String PostDuctSealCloseCfm = "PostDuctSealCloseCfm";
        public static String PostDuctSealCloseDiffPa = "PostDuctSealCloseDiffPa";

        public static String ZonePressureID = "ZonePressureID";
        public static String Pressure = "Pressure";
        public static String DuctsPresent = "DuctsPresent";
        public static String HouseSmokeDetector = "HouseSmokeDetector";
        public static String HouseCOMonitor = "HouseCOMonitor";
        public static String HouseCOHeating = "HouseCOHeating";
        public static String HouseCOWaterHeat = "HouseCOWaterHeat";
        public static String HouseCOLiving = "HouseCOLiving";
        public static String HouseCOKitchen = "HouseCOKitchen";
        public static String HouseComment = "HouseComment";
        public static String CookCOOven = "CookCOOven";
        public static String CookCOBurner1 = "CookCOBurner1";
        public static String CookCOBurner2 = "CookCOBurner2";
        public static String CookCOBurner3 = "CookCOBurner3";
        public static String CookCOBurner4 = "CookCOBurner4";
        public static String CookGasLeak = "CookGasLeak";
        public static String WoodStove = "WoodStove";
        public static String WoodVentIncorrect = "WoodVentIncorrect";
        public static String WoodCombustionAir = "WoodCombustionAir";
        public static String ExBathMissing = "ExBathMissing";
        public static String ExBathBroken = "ExBathBroken";
        public static String ExBathVent = "ExBathVent";
        public static String ExKitMissing = "ExKitMissing";
        public static String ExKitBroken = "ExKitBroken";
        public static String ExKitVent = "ExKitVent";
        public static String ExDryerVent = "ExDryerVent";
        public static String ExAirExist = "ExAirExist";
        public static String ExAirBroken = "ExAirBroken";
        public static String EquipComment = "EquipComment";
        public static String AtticRecessedLights = "AtticRecessedLights";
        public static String AtticFlueShield = "AtticFlueShield";
        public static String AtticWiring = "AtticWiring";
        public static String AtticVent = "AtticVent";
        public static String AtticWaterLeak = "AtticWaterLeak";
        public static String AtticMoisture = "AtticMoisture";
        public static String AtticOther = "AtticOther";
        public static String AtticVermiculite = "AtticVermiculite";
        public static String WallWiring = "WallWiring";
        public static String WallWaterLeak = "WallWaterLeak";
        public static String WallMoisture = "WallMoisture";
        public static String WallLeadPaint = "WallLeadPaint";
        public static String WallSidingAsbestos = "WallSidingAsbestos";
        public static String WallOther = "WallOther";
        public static String BaseBarrier = "BaseBarrier";
        public static String BaseWiring = "BaseWiring";
        public static String BaseWaterLeak = "BaseWaterLeak";
        public static String BasePlumbingLeak = "BasePlumbingLeak";
        public static String BaseMoisture = "BaseMoisture";
        public static String BaseOther = "BaseOther";
        public static String ShellComment = "ShellComment";

        public static String RegisterNumber = "RegisterNumber";
        public static String RegisterType = "RegisterType";
        public static String SortOrder = "SortOrder";
        public static String TypeAbrev = "TypeAbrev";
        public static String UDF = "UDF";

        public static String FuelID = "FuelID";
        public static String Fuel = "Fuel";
        public static String Input = "Input";
        public static String InputUnitsID = "InputUnitsID";
        public static String EnergyFactor = "EnergyFactor";
        public static String RecoverFactor = "RecoverFactor";

        public static String ExistTankLocationID = "ExistTankLocationID";
        public static String ExistFuelTypeID = "ExistFuelTypeID";
        public static String ExistGal = "ExistGal";
        public static String ExistEF = "ExistEF";
        public static String ExistRE = "ExistRE";
        public static String ExistInput = "ExistInput";
        public static String ExistInputUnitsID = "ExistInputUnitsID";
        public static String ExistInsulTypeID = "ExistInsulTypeID";
        public static String ExistInsulThick = "ExistInsulThick";
        public static String ExistPipeInsul = "ExistPipeInsul";
        public static String ExistTankWrap = "ExistTankWrap";
        public static String ExistRValue = "ExistRValue";
        public static String ShowerHeads = "ShowerHeads";
        public static String ShowerUsagePerDay = "ShowerUsagePerDay";
        public static String ShowerGPM = "ShowerGPM";
        public static String ReplaceFuelTypeID = "ReplaceFuelTypeID";
        public static String ReplaceGal = "ReplaceGal";
        public static String ReplaceEF = "ReplaceEF";
        public static String ReplaceRE = "ReplaceRE";
        public static String ReplaceInput = "ReplaceInput";
        public static String ReplaceInputUnitsID = "ReplaceInputUnitsID";
        public static String OpsCombustionInletTemp = "OpsCombustionInletTemp";
        public static String OpsFlueTemp = "OpsFlueTemp";
        public static String OpsNetStackTemp = "OpsNetStackTemp";
        public static String OpsPercentOxygen = "OpsPercentOxygen";
        public static String OpsPercentDioxide = "OpsPercentDioxide";
        public static String OpsSmokeNumber = "OpsSmokeNumber";
        public static String OpsSteadyStateEfficiency = "OpsSteadyStateEfficiency";
        public static String OpsCOFlue = "OpsCOFlue";
        public static String OpsCOFreeAir = "OpsCOFreeAir";
        public static String OpsCombustionInletTempInsp = "OpsCombustionInletTempInsp";
        public static String OpsFlueTempInsp = "OpsFlueTempInsp";
        public static String OpsNetStackTempInsp = "OpsNetStackTempInsp";
        public static String OpsPercentOxygenInsp = "OpsPercentOxygenInsp";
        public static String OpsPercentDioxideInsp = "OpsPercentDioxideInsp";
        public static String OpsSmokeNumberInsp = "OpsSmokeNumberInsp";
        public static String OpsSteadyStateEfficiencyInsp = "OpsSteadyStateEfficiencyInsp";
        public static String OpsCOFlueInsp = "OpsCOFlueInsp";
        public static String OpsCOFreeAirInsp = "OpsCOFreeAirInsp";
        public static String OpsComment = "OpsComment";
        public static String VentDamperDiameter = "VentDamperDiameter";
        public static String VentDamperTypeID = "VentDamperTypeID";
        public static String VentDamperConditionID = "VentDamperConditionID";
        public static String VentChimneyTypeID = "VentChimneyTypeID";
        public static String VentChimneyConditionID = "VentChimneyConditionID";
        public static String VentFlueTypeID = "VentFlueTypeID";
        public static String VentFlueConditionID = "VentFlueConditionID";
        public static String VentAirIntakeIntakeID = "VentAirIntakeIntakeID";
        public static String VentProblems = "VentProblems";
        public static String VentTestNormOutdoorTemp = "VentTestNormOutdoorTemp";
        public static String VentTestNormDraft = "VentTestNormDraft";
        public static String VentTestNormSpillTime = "VentTestNormSpillTime";
        public static String VentTestNormOutdoorTempInsp = "VentTestNormOutdoorTempInsp";
        public static String VentTestNormDraftInsp = "VentTestNormDraftInsp";
        public static String VentTestNormSpillTimeInsp = "VentTestNormSpillTimeInsp";
        public static String VentComment = "VentComment";
        public static String HSClearance = "HSClearance";
        public static String HSElectricSwitchConditionID = "HSElectricSwitchConditionID";
        public static String HSGasLeak = "HSGasLeak";
        public static String HSFuelShutoffMissing = "HSFuelShutoffMissing";
        public static String HSDripLegMissing = "HSDripLegMissing";
        public static String HSHotWaterTemp = "HSHotWaterTemp";
        public static String HSAdjustTemp = "HSAdjustTemp";
        public static String HSReliefPiping = "HSReliefPiping";
        public static String HSWaterLeak = "HSWaterLeak";
        public static String HSOtherProblem = "HSOtherProblem";
        public static String HSComment = "HSComment";
        public static String TypeID = "TypeID";
        public static String HeatingEquipTypeID = "HeatingEquipTypeID";
        public static String FuelTypeID = "FuelTypeID";
        public static String HeatingEquipLocationID = "HeatingEquipLocationID";
        public static String PercentSupplied = "PercentSupplied";
        public static String EfficiencyUnitsID = "EfficiencyUnitsID";
        public static String Efficiency = "Efficiency";
        public static String HeatingUnitsID = "HeatingUnitsID";
        public static String InputRating = "InputRating";
        public static String SerialNumber = "SerialNumber";
        public static String HeatingEquipConditionID = "HeatingEquipConditionID";
        public static String Primary = "Primary";
        public static String ReplaceSystem = "ReplaceSystem";
        public static String HSPF = "HSPF";
        public static String SSEfficiency = "SSEfficiency";
        public static String SmartThermostat = "SmartThermostat";
        public static String PilotLight = "PilotLight";
        public static String PilotLightinSummer = "PilotLightinSummer";
        public static String IntermittentIgnition = "IntermittentIgnition";
        public static String RetentionHead = "RetentionHead";
        public static String RetentionHeadRecommended = "RetentionHeadRecommended";
        public static String PowerBurner = "PowerBurner";
        public static String HeatingRetroStatusID = "HeatingRetroStatusID";
        public static String RetroAFUEStandardEff = "RetroAFUEStandardEff";
        public static String RetroAFUEHighEff = "RetroAFUEHighEff";
        public static String RetroFuelTypeID = "RetroFuelTypeID";
        public static String LaborCostStandardEff = "LaborCostStandardEff";
        public static String LaborCostHighEff = "LaborCostHighEff";
        public static String MaterialCostStandardEff = "MaterialCostStandardEff";
        public static String MaterialCostHighEff = "MaterialCostHighEff";
        public static String DuctLength = "DuctLength";
        public static String DuctPerimeter = "DuctPerimeter";
        public static String DuctTypeID1 = "DuctTypeID1";
        public static String DuctTypeID2 = "DuctTypeID2";
        public static String DuctTypeID3 = "DuctTypeID3";
        public static String DuctLength1 = "DuctLength1";
        public static String DuctLength2 = "DuctLength2";
        public static String DuctLength3 = "DuctLength3";
        public static String DuctDiameter1 = "DuctDiameter1";
        public static String DuctDiameter2 = "DuctDiameter2";
        public static String DuctDiameter3 = "DuctDiameter3";
        public static String DuctWidth1 = "DuctWidth1";
        public static String DuctWidth2 = "DuctWidth2";
        public static String DuctWidth3 = "DuctWidth3";
        public static String DuctHeight1 = "DuctHeight1";
        public static String DuctHeight2 = "DuctHeight2";
        public static String DuctHeight3 = "DuctHeight3";
        public static String HeatingDuctLocationID = "HeatingDuctLocationID";
        public static String OpsEfficiency = "OpsEfficiency";
        public static String OpsReturnTemperature = "OpsReturnTemperature";
        public static String OpsSupplyTemperature = "OpsSupplyTemperature";
        public static String OpsNetRise = "OpsNetRise";
        public static String OpsEfficiencyInsp = "OpsEfficiencyInsp";
        public static String OpsReturnTemperatureInsp = "OpsReturnTemperatureInsp";
        public static String OpsSupplyTemperatureInsp = "OpsSupplyTemperatureInsp";
        public static String OpsNetRiseInsp = "OpsNetRiseInsp";
        public static String OpsLabelRise = "OpsLabelRise";
        public static String VentDamper = "VentDamper";
        public static String VentDamperRecommended = "VentDamperRecommended";
        public static String VentDamperDiameterInfo = "VentDamperDiameterInfo";
        public static String VentCombustionSysTypeID = "VentCombustionSysTypeID";
        public static String EqBurnerTypeID = "EqBurnerTypeID";
        public static String EqBurnerConditionID = "EqBurnerConditionID";
        public static String EqPilotTypeID = "EqPilotTypeID";
        public static String EqPilotConditionID = "EqPilotConditionID";
        public static String DistBlowerTypeID = "DistBlowerTypeID";
        public static String DistBlowerConditionID = "DistBlowerConditionID";
        public static String DistMotorAmps = "DistMotorAmps";
        public static String DistBeltConditionID = "DistBeltConditionID";
        public static String DistBeltSize = "DistBeltSize";
        public static String DistBeltPlay = "DistBeltPlay";
        public static String AccHumidifierConditionID = "AccHumidifierConditionID";
        public static String AccAirCleanerConditionID = "AccAirCleanerConditionID";
        public static String AccCoilConditionID = "AccCoilConditionID";
        public static String DistFilterSize = "DistFilterSize";
        public static String DistFilterConditionID = "DistFilterConditionID";
        public static String EqComment = "EqComment";
        public static String HSLimitControlAdjustible = "HSLimitControlAdjustible";
        public static String HSFanOnSetting = "HSFanOnSetting";
        public static String HSFanOffSetting = "HSFanOffSetting";
        public static String HSHighLimitSetting = "HSHighLimitSetting";
        public static String HSLimitControlBroken = "HSLimitControlBroken";
        public static String HSExchangerLeak = "HSExchangerLeak";
        public static String ThermostatTypeID = "ThermostatTypeID";
        public static String ThermostatDaySetPoint = "ThermostatDaySetPoint";
        public static String ThermostatNightSetPoint = "ThermostatNightSetPoint";
        public static String ThermostatRelocate = "ThermostatRelocate";
        public static String ThermostatAnticipatorAmps = "ThermostatAnticipatorAmps";
        public static String ThermostatAnticipator = "ThermostatAnticipator";
        public static String ThermostatAnticipatorAdjust = "ThermostatAnticipatorAdjust";
        public static String ThermostatComment = "ThermostatComment";
        public static String BoilerDistSystemTypeID = "BoilerDistSystemTypeID";
        public static String BoilerDistPumpLocID = "BoilerDistPumpLocID";
        public static String BoilerDistExpandTankConditionID = "BoilerDistExpandTankConditionID";
        public static String BoilerDistDrainValveConditionID = "BoilerDistDrainValveConditionID";
        public static String BoilerDistConditionID = "BoilerDistConditionID";
        public static String BoilerDistAsbestos = "BoilerDistAsbestos";
        public static String BoilerDistAsbestosConditionID = "BoilerDistAsbestosConditionID";
        public static String BoilerControlTPGauge = "BoilerControlTPGauge";
        public static String BoilerControlPressureReading = "BoilerControlPressureReading";
        public static String BoilerControlLowWaterSwitch = "BoilerControlLowWaterSwitch";
        public static String BoilerControlAquaStatSetting = "BoilerControlAquaStatSetting";
        public static String BoilerConvectorTypeID = "BoilerConvectorTypeID";
        public static String BoilerConvectorsInEachRoom = "BoilerConvectorsInEachRoom";
        public static String BoilerConvectorsInUnconditioned = "BoilerConvectorsInUnconditioned";
        public static String BoilerConvectorsZoneValves = "BoilerConvectorsZoneValves";
        public static String BoilerConvectorsZoneValveType = "BoilerConvectorsZoneValveType";
        public static String BoilerConvectorsZoneValveConditionID = "BoilerConvectorsZoneValveConditionID";
        public static String BoilerConvectorsUserKey = "BoilerConvectorsUserKey";
        public static String BoilerComment = "BoilerComment";
        public static String RLaborCost = "RLaborCost";
        public static String RMaterialCost = "RMaterialCost";
        public static String RLife = "RLife";

        public static String FloorArea = "FloorArea";
        public static String ConditionedStories = "ConditionedStories";


        public static String WAPItemizedID = "WAPItemizedID";
        public static String Material = "Material";
        public static String EnergySavings = "EnergySavings";
        public static String EnergyUnitsID = "EnergyUnitsID";
        public static String NewWAPItemizedID = "NewWAPItemizedID";

        public static String TestID = "TestID";
        public static String ConductedOn = "ConductedOn";
        public static String Draft = "Draft";
        public static String SpillTime = "SpillTime";
        public static String OutdoorTemp = "OutdoorTemp";


        public static String LibID = "LibID";
        public static String MeasureTypeID = "MeasureTypeID";
        public static String Active = "Active";
        public static String DefaultContractorID = "DefaultContractorID";
        public static String DefaultCostCenterID = "DefaultCostCenterID";
        public static String EnergySaved = "EnergySaved";
        public static String EnergySavingsID = "EnergySavingsID";
        public static String EnergySavingsUnitsID = "EnergySavingsUnitsID";
        public static String LibMeasureID = "LibMeasureID";
        public static String MFEAFlag = "MFEAFlag";
        public static String MHEAFlag = "MHEAFlag";
        public static String NEATFlag = "NEATFlag";
        public static String Order = "Order";
        public static String MeasureType = "MeasureType";
        public static String Shade = "Shade";

        public static String UploadedDate = "UploadedDate";
        public static String WAPTestID = "WAPTestID";
        public static String MeasureCount = "MeasureCount";
        public static String CCost = "CCost";
        public static String CSIR = "CSIR";
        public static String CumulativeEstCost = "CumulativeEstCost";
        public static String CumulativeActualCost = "CumulativeActualCost";
        public static String TimeStamp = "TimeStamp";

        public static String MaterialID = "MaterialID";
        public static String Components = "Components";
        public static String MaterialTypeID = "MaterialTypeID";
        public static String QtyEst = "QtyEst";
        public static String QtyAct = "QtyAct";
        public static String UnitCostEst = "UnitCostEst";
        public static String UnitCostAct = "UnitCostAct";
        public static String EstTotal = "EstTotal";
        public static String MaterialType = "MaterialType";

        public static String CodeValueText = "CodeValueText";
        public static String DateFormat = "MM/dd/yyyy hh:mm:ss tt";

        public static String MRID = "MRID";
        public static String CreatedByID = "CreatedByID";
        public static String ModifiedDate = "ModifiedDate";
        public static String ModifiedByID = "ModifiedByID";
        public static String PRCID = "PRCID";
        public static String ReasonCode = "ReasonCode";

        public static String RunID = "RunID";
        public static String MakeWorkOrder = "MakeWorkOrder";
        public static String MeasureLibID = "MeasureLibID";
        public static String MeasureTypeNum = "MeasureTypeNum";
        public static String CostCenterID = "CostCenterID";
        public static String HeatingMBTU = "HeatingMBTU";
        public static String HeatingSave = "HeatingSave";
        public static String CoolingKWH = "CoolingKWH";
        public static String CoolingSave = "CoolingSave";
        public static String BaseloadKWH = "BaseloadKWH";
        public static String BaseloadSave = "BaseloadSave";
        public static String TotalSavingsPW = "TotalSavingsPW";
        public static String CodeDescWithText = "CodeDescWithText";

        public static String LogID = "LogID";
        public static String FieldName = "FieldName";
        public static String FieldValue = "FieldValue";
        public static String SqlQuery = "SqlQuery";
        public static String DeviceID = "DeviceID";
        public static String DeviceModel = "DeviceModel";
        public static String OSVersion = "OSVersion";
        public static String OSPlatform = "OSPlatform";
        public static String AppVersion = "AppVersion";
        public static String ActTotal = "ActTotal";
        public static String IncentiveAmount = "IncentiveAmount";
        #endregion

        #region MHEA
        public static String Length = "Length";
        public static String WindShieldingID = "WindShieldingID";
        public static String LeakyID = "LeakyID";
        public static String WaterHeaterCloset = "WaterHeaterCloset";
        public static String JobName = "JobName";
        public static String Addition = "Addition";
        public static String Min = "Min";
        public static String Max = "Max";

        #region--Wall----
        public static String UninsulatableArea = "UninsulatableArea";
        public static String BattInsulThickness = "BattInsulThickness";
        public static String LooseInsulThickness = "LooseInsulThickness";
        public static String FoamInsulThickness = "FoamInsulThickness";
        public static String AddCost = "AddCost";
        public static String WallVentID = "WallVentID";
        public static String PorchLength = "PorchLength";
        public static String PorchWidth = "PorchWidth";
        public static String PorchOrientationID = "PorchOrientationID";
        public static String HeightMax = "HeightMax";
        public static String HeightMin = "HeightMin";
        public static String WallConfigID = "WallConfigID";
        #endregion

        #region--Window----
        public static String NumNorth = "NumNorth";
        public static String NumSouth = "NumSouth";
        public static String NumEast = "NumEast";
        public static String NumWest = "NumWest";
        public static String CostAddGlassStorm = "CostAddGlassStorm";
        public static String CostAddPlasticStorm = "CostAddPlasticStorm";
        public static String GlazingTypeID = "GlazingTypeID";
        #endregion

        #region----Door---
        public static String Storm = "Storm";
        #endregion

        #region---Ceiling----
        public static String CeilingTypeID = "CeilingTypeID";
        public static String JoistSizeID = "JoistSizeID";
        public static String CeilingHeight = "CeilingHeight";
        public static String PitchAddInsul = "PitchAddInsul";
        public static String PercentCathedral = "PercentCathedral";
        public static String StepWallOrientationID = "StepWallOrientationID";
        public static String CeilingColorID = "CeilingColorID";
        #endregion

        #region----Floor---
        public static String JoistDirectionID = "JoistDirectionID";
        public static String InsulationDirectionID = "InsulationDirectionID";
        public static String Skirt = "Skirt";
        public static String WingJoistSizeID = "WingJoistSizeID";
        public static String WingBattInsulThickness = "WingBattInsulThickness";
        public static String WingLooseInsulThickness = "WingLooseInsulThickness";
        public static String WingInsulLocationID = "WingInsulLocationID";
        public static String BellyJoistSizeID = "BellyJoistSizeID";
        public static String BellyBattInsulThickness = "BellyBattInsulThickness";
        public static String BellyLooseInsulThickness = "BellyLooseInsulThickness";
        public static String BellyInsulLocationID = "BellyInsulLocationID";
        public static String BellyConditionID = "BellyConditionID";
        public static String BellyConfigurationID = "BellyConfigurationID";
        public static String BellyDepth = "BellyDepth";
        public static String AddFloorTypeID = "AddFloorTypeID";
        public static String InsulLocationID = "InsulLocationID";
        public static String AvailtInsulThickness = "AvailtInsulThickness";
        #endregion

        #region--Heating Primary----
        public static String EquipmentTypeID = "EquipmentTypeID";
        public static String DaySetPoint = "DaySetPoint";
        public static String NightSetPoint = "NightSetPoint";
        public static String DuctLocationID = "DuctLocationID";
        public static String DuctInsulID = "DuctInsulID";
        public static String PercentHeated = "PercentHeated";
        public static String DistFilterLocationID = "DistFilterLocationID";
        #endregion
        #region--Heating Secondary----
        public static String InclCosts = "InclCosts";
        public static String LaborCost = "LaborCost";
        public static String MaterialCost = "MaterialCost";
        #endregion

        #region--Cooling Primary----
        public static String COP = "COP";
        public static String PercentCooled = "PercentCooled";
        #endregion

        #region--Recomended report----
        public static String PeriodDays = "PeriodDays";
        public static String ConsumpActual = "ConsumpActual";
        public static String ConsumpPredicted = "ConsumpPredicted";
        public static String DDActual = "DDActual";
        public static String DDPredicted = "DDPredicted";
        public static String Index = "Index";
        public static String Measure = "Measure";
        public static String TotalMBTU = "TotalMBTU";
        public static String Savings = "Savings";
        public static String SIR = "SIR";
        public static String CSav = "CSav";
        public static String Quantity = "Quantity";
        public static String PreHeat = "PreHeat";
        public static String PreCool = "PreCool";
        public static String PreBase = "PreBase";
        public static String PostHeat = "PostHeat";
        public static String PostCool = "PostCool";
        public static String PostBase = "PostBase";
        public static String Name = "Name";
        public static String PreLoad = "PreLoad";
        public static String PostLoad = "PostLoad";
        public static String AreaVolume = "AreaVolume";
        public static String Day = "Day";

        #endregion


        #endregion

        public static String RequestedOn = "RequestedOn";
        public static String ScheduleDateTimeRaw = "ScheduleDateTimeRaw";
        public static String WorkflowName = "WorkflowName";
        public static String ContractorName = "Contractor Name";
        public static String ContractorCode = "Contractor Code";
        public static String Services = "Services";
        public static String AccountManager = "Account Manager";
        public static String QCNMemberSince = "QCNMemberSince";
        public static String Measures = "Measures";
        public static String SmallBusinessType = "SmallBusinessType";
        public static String ContractorName1 = "ContractorName";
        public static String BeforeHRS = "BeforeHRS";
        public static String QCNMember = "QCNMember";
        public static String JobTitle = "JobTitle";
        #region PaymentProcess
        public static String SecretKey = "SecretKey";
        public static String LoginID = "LoginID";
        public static String Mode = "Mode";
        public static String MerchantID = "MerchantID";
        public static String PublicKey = "PublicKey";
        public static String PrivateKey = "PrivateKey";
        public static String BraintreeMode = "BraintreeMode";
        public static String CCProcessGateway = "CCProcessGateway";
        #endregion

        public static String QACount = "QACount";
        public static String Paid = "Paid";
        public static String Refund = "Refund";
        public static String ProjectType = "ProjectType";
        public static String DistrictName = "DistrictName";
        public static String ActionText = "ActionText";
        public static String LPCApproved = "LPCApproved";
        public static String QtyUsed = "QtyUsed";
        public static String OrderType = "OrderType";
        public static String LPCApprovalStatus = "LPCApprovalStatus";
        public static String AppStatus = "AppStatus";
        public static String USCitizen = "USCitizen";
        public static String Owner = "Owner";
        public static String LandlordConsent = "LandlordConsent";
        public static String PriorWAP = "PriorWAP";
        public static String Reason = "Reason";
        public static String SavingsAdjFactor = "SavingsAdjFactor";
        public static String EstSIR = "EstSIR";
        public static String ActSIR = "ActSIR";

        public static String Upgrade = "Upgrade";
        public static String HomeOwnerRebate = "HomeOwnerRebate";
        public static String Limitation = "Limitation";

        //ES-4329 Nilesh K
        public static String FromProjectID = "FromProjectID";
        public static String FromDBName = "FromDBName";
        public static String UserNotFound = "UserNotFound";
        public static String OriginalName = "OriginalName";
        public static String DocFileID = "DocFileID";
        public static String ObjectType = "ObjectType";

        public static String CRID = "CRID";
        public static String IPAddress = "IPAddress";

  
        public static String BadgeID = "BadgeID";
        public static String BadgeTypeID = "BadgeTypeID";
        public static String BadgeStatus = "BadgeStatus";
        public static String BadgeType = "BadgeType";
        public static String BadgeTitle = "BadgeTitle";
        public static String BadgeDescription = "BadgeDescription";
        public static String OriginalImageName = "OriginalImageName";
        public static String SystemImageName = "SystemImageName";
        public static String BadgeIDs = "BadgeIDs";

        /* Start - DSM-624: Contractor: Upgrade App: Financing Information */
        public static String IsFinanced = "IsFinanced";
        public static String FinancedAmount = "FinancedAmount";
        public static String FinancedBy = "FinancedBy";

        /* End - DSM-624: Contractor: Upgrade App: Financing Information */

        /* Start - DSM-547: Open Enrollment template creation and web version of PIP*/
        public static String PIPName = "PIPName";
        public static String PIPTemplateID = "PIPTemplateID";
        public static String PIPStatus = "PIPStatus";
        public static String ApprovedOn = "ApprovedOn";
        public static String EnrollStart = "EnrollStart";
        public static String EnrollEnd = "EnrollEnd";
        public static String LiveDate = "LiveDate";
        public static String Header = "Header";
        public static String FinePrint = "FinePrint";
        public static String FinancingOnBill = "FinancingOnBill";
        public static String FinancingOffBill = "FinancingOffBill";
        public static String FinancingOnBillSchedule = "FinancingOnBillSchedule";
        public static String FinancingOffBillSchedule = "FinancingOffBillSchedule";
        public static String FinancingOther = "FinancingOther";
        public static String FinancingNone = "FinancingNone";
        public static String TKAdditional = "TKAdditional";
        public static String LPCAdditional = "LPCAdditional";
        public static String TKQuestions = "TKQuestions";
        public static String LPCQuestions = "LPCQuestions";
        public static String CreatedBy = "CreatedBy";
        public static String ModifiedBy = "ModifiedBy";
        public static String OldPIPTemplateID = "OldPIPTemplateID";

        public static String PIPTemplateItemID = "PIPTemplateItemID";
        public static String ItemType = "ItemType";
        public static String ItemDesc = "ItemDesc";
        public static String TKModify = "TKModify";
        public static String LPCModify = "LPCModify";
        public static String DisplayStandardRebate = "DisplayStandardRebate";

        public static String PIPLPCID = "PIPLPCID";
        public static String ProgramSubModel = "ProgramSubModel";
        public static String FinanceModel = "FinanceModel";
        public static String Weatherization = "Weatherization";
        public static String EvalFee = "EvalFee";
        public static String EvalFeeRetained = "EvalFeeRetained";
        public static String ContactNumber = "ContactNumber";
        public static String ContractDate = "ContractDate";
        public static String AllowRetail = "AllowRetail";
        public static String Additional = "Additional";
        public static String Questions = "Questions";

        public static String PIPLPCDetailID = "PIPLPCDetailID";
        public static String AllowStandardRebate = "AllowStandardRebate";
        public static String ReportHeader = "ReportHeader";

        /* End - DSM-547: Open Enrollment template creation and web version of PIP*/
        public static String FundSourceID = "FundSourceID";
        public static String ReferenceCode = "ReferenceCode";
        public static String SourceOfFund = "SourceOfFund";
        public static String FundName = "FundName";
        public static String ShortCode = "ShortCode";
        public static String FundDescription = "FundDescription";
        public static String UtilityID = "UtilityID";
        public static String ToDate = "ToDate";
        public static String PIPHUPID = "PIPHUPID";
        public static String PIPHUPDetailID = "PIPHUPDetailID";
        public static String TVAContributionAmount = "TVAContributionAmount";
        public static String AdminAmount = "AdminAmount";
        public static String MiscellaneousAmount = "MiscellaneousAmount";
        public static String TotalAmount = "TotalAmount";
        public static String County = "County";
        public static String HUPTerritoryID = "HUPTerritoryID";
        public static String TotalNetAmount = "TotalNetAmount";
        public static String IsReviewNewPIP = "IsReviewNewPIP";
        public static String PIPTemplateHistoryID = "PIPTemplateHistoryID";
        public static String WithdrawnFund = "WithdrawnFund";
        public static String AddedFund = "AddedFund";
        public static String IsPhysicalFile = "IsPhysicalFile";
        public static String AvgDeliveryCostPerHome = "AvgDeliveryCostPerHome";
        public static String PartnerID = "PartnerID";

        public static String CalculatedCostPerHome = "CalculatedCostPerHome";
        public static String CalculatedHomes = "CalculatedHomes";
        public static String CalculatedHomesAdjusted = "CalculatedHomesAdjusted";
        public static String CalculatedDeliveryPerHome = "CalculatedDeliveryPerHome";
        public static String CalculatedTotalDeliveryCost = "CalculatedTotalDeliveryCost";
        public static String CalculatedInKindPerHome = "CalculatedInKindPerHome";
        public static String AvailableInHomeBudget = "AvailableInHomeBudget";
        public static String CostPerInHome = "CostPerInHome";
        public static String CalculatedTargetHome = "CalculatedTargetHome";
        public static String TotalDeliveryCost = "TotalDeliveryCost";
        public static String AvgBudgetPerHome = "AvgBudgetPerHome";
        public static String ContributionFormID = "ContributionFormID";

        #region --- Application---
        public static String ServiceAddressGIID = "ServiceAddressGIID";
        public static String SecondaryContactName = "SecondaryContactName";
        public static String SecondaryContactPhone = "SecondaryContactPhone";
        public static String DateReceived = "DateReceived";
        public static String ReceivedBy = "ReceivedBy";
        public static String DeclaredIncome = "DeclaredIncome";
        public static String CalculatedIncome = "CalculatedIncome";
        public static String ApplicationApprovedOn = "ApplicationApprovedOn";
        public static String Terms = "Terms";
        public static String HMID = "HMID";
        public static String IsLimitAllowed = "IsLimitAllowed";

        public static String ReceivedByName = "ReceivedByName";
        public static String NoOfOccupants = "NoOfOccupants";
        public static String FemaleHeadedHousehold = "FemaleHeadedHousehold";
        public static String HispanicOrigin = "HispanicOrigin";
        public static String IsApprovalToShare = "IsApprovalToShare";
        public static String NoOfHouseHoldMembers = "NoOfHouseHoldMembers";        

        public static String HomeOwnersIdentification = "HomeOwnersIdentification";
        public static String ProofOfIncome = "ProofOfIncome";
        public static String ProofOfOwnership = "ProofOfOwnership";
        public static String IsValid = "IsValid";
        public static String AdditionalReference1 = "AdditionalReference1";
        public static String AdditionalReference2 = "AdditionalReference2";
        public static String FPL = "FPL";
        public static String FPLYear = "FPLYear";
        public static String FPLStateAbbr = "FPLStateAbbr";
        #endregion

        #region --- HUP Client Application---
        public static String WidgetApplicationID = "WidgetApplicationID";
        #endregion

        #region --- GPPContracts---
        public static String GPPContractId = "GPPContractId";
        //public static String GPPRecordId = "GPPRecordId";
        public static String PartID = "PartID";
        public static String PANum = "PANum";
        //public static String OldPANum = "OldPANum";
        public static String CRRProjectType = "CRRProjectType";
        public static String CaseType = "CaseType";
        public static String CapacityOfferingYear = "CapacityOfferingYear";
        public static String ContractEndDate = "ContractEndDate";
        //public static String DistribtrRptname = "DistribtrRptname";
        public static String Participant = "Participant";
        public static String GPPEmail = "Email";
        public static String ParticipantPhone = "ParticipantPhone";
        public static String SysAdd1 = "SysAdd1";
        public static String SysAdd2 = "SysAdd2";
        public static String SysCity = "SysCity";
        public static String SysSt = "SysSt";
        public static String SysZip1 = "SysZip1";
        public static String SysZip2 = "SysZip2";
        public static String MailingAddFlag = "MailingAddFlag";
        public static String BillAdd1 = "BillAdd1";
        public static String BillAdd2 = "BillAdd2";
        public static String BillCity = "BillCity";
        public static String BillSt = "BillSt";
        public static String BillZip1 = "BillZip1";
        public static String BillZip2 = "BillZip2";
        public static String Distributor = "Distributor";
        public static String BillingOption = "BillingOption";
        public static String GPPDistrict = "District";
        public static String PartType = "PartType";
        public static String ParticipantSubType = "ParticipantSubType";
        public static String ParticipantRate = "ParticipantRate";
        //public static String PartRateDesc = "PartRateDesc";
        //public static String PartRateAmt = "PartRateAmt";
        public static String EnrgyType = "EnrgyType";
        public static String SubEnergy = "SubEnergy";
        public static String InitCon = "InitCon";
        public static String InitConStrDt = "InitConStrDt";
        public static String NewContractEndDate = "ContractEndDate";
        //public static String InitConEndDt = "InitConEndDt";
        public static String CurCon = "CurCon";
        public static String CurConPrem = "CurConPrem";
        public static String CurConStrDt = "CurConStrDt";
        public static String CurConEndDt = "CurConEndDt";
        public static String CurConPremEndDt = "CurConPremEndDt";
        public static String CurConInspDt = "CurConInspDt";
        public static String CURCONINBXDT = "CURCONINBXDT";
        public static String CURCONDISTDT = "CURCONDISTDT";
        public static String TVAReviewDate = "TVAReviewDate";
        public static String CURCONTVADT = "CURCONTVADT";
        //public static String AnnualUsagekWh = "AnnualUsagekWh";
        public static String MaxNamePlateCapacitykW = "MaxNamePlateCapacitykW";
        public static String CurConkW = "CurConkW";
        public static String CurConStatus = "CurConStatus";
        public static String CurConNote = "CurConNote";
        public static String CurConActisMon = "CurConActisMon";
        public static String CurConActisFY = "CurConActisFY";
        public static String OneYr_kWH = "OneYr_kWH";
        public static String FCASTDOLS = "FCASTDOLS";
        public static String DiscloseInfo = "DiscloseInfo";
        public static String Demand = "Demand";
        public static String IncentivePayment = "IncentivePayment";
        public static String CurConMetID = "CurConMetID";
        public static String eCD = "eCD";
        public static String CapFac = "CapFac";
        public static String ElectricServiceAccountNumber = "ElectricServiceAccountNumber";
        public static String MeterType = "MeterType";

        public static String DocumentType = "DocumentType";

        public static String MeteringConnection = "MeteringConnection";
        public static String IntervalMeterOption = "IntervalMeterOption";
        public static String RemoteCommunicationsType = "RemoteCommunicationsType";
        public static String AnnualElectricUsage = "AnnualElectricUsage";
        public static String InstallerCompany = "InstallerCompany";
        public static String SystemInstallerName = "SystemInstallerName";
        public static String InstallerTelephone = "InstallerTelephone";
        public static String InstallerAddress = "InstallerAddress";
        public static String InstallerWebsite = "InstallerWebsite";
        public static String SystemOwnerFlag = "SystemOwnerFlag";
        public static String SystemownerCompany = "SystemownerCompany";
        public static String SystemOwnerWebsite = "SystemOwnerWebsite";
        public static String SystemownerUsername = "SystemownerUsername";
        public static String SystemOwnerRepName = "SystemOwnerRepName";
        public static String SystemOwnerEmail = "SystemOwnerEmail";
        public static String SystemOwnerTelephone = "SystemOwnerTelephone";
        public static String SystemOwnerAddress1 = "SystemOwnerAddress1";
        public static String SystemOwnerAddress2 = "SystemOwnerAddress2";
        public static String SystemOwnerCity = "SystemOwnerCity";
        public static String SystemOwnerState = "SystemOwnerState";
        //public static String SystemOwnerStateCd = "SystemOwnerStateCd";
        public static String SystemOwnerZip = "SystemOwnerZip";
        public static String Rate = "Rate";
        public static String GPPCreatedBy = "CreatedBy";
        public static String GPPModifiedBy = "ModifiedBy";
        public static String ContractStatus = "ContractStatus";
        public static String LPCNo = "LPCNo";
        #endregion

        #region --- WorkOrder---
        public static String WorkOrderID = "WorkOrderID";
        public static String WorkOrderDetailsID = "WorkOrderDetailsId";
        public static String FundingSourceID = "FundingSourceID";
        public static String FundingSourceAmount = "FundingSourceAmount";
        public static String Labor = "Labor";
        public static String Total = "Total";
        public static String WorkOrderFundingSourceID = "WorkOrderFundingSourceID";
        public static String WOStatus = "WOStatus";
        public static String LogoURL = "LogoURL";
        public static String FieldTransactionID = "FieldTransactionID";
        public static String InstallDateTime = "InstallDateTime";
        public static String TargetCompletionlDateTime = "TargetCompletionDateTime";

        #endregion
        #region --- RelifFund--
        public static String COVIDReliefFundID = "COVIDReliefFundID";
        public static String ContactName= "ContactName";        
        public static String Address = "Address";
        public static String StreetAddress = "StreetAddress";
        public static String AmountRequested = "AmountRequested";
        public static String CharityName = "CharityName";
        public static String CharityContactName = "CharityContactName";
        public static String CharityPhone = "CharityPhone";
        public static String CharityEmail = "CharityEmail";
        public static String CharityAddress = "CharityAddress";
        public static String CharityStreetAddress = "CharityStreetAddress";
        public static String CharityCity = "CharityCity";
        public static String CharityState = "CharityState";
        public static String CharityCounty = "CharityCounty";
        public static String FocusAreaOther = "FocusAreaOther";
        public static String Partner = "Partner";
        public static String MatchingFunds = "MatchingFunds";
        public static String SourceFunds = "SourceFunds";
        public static String TotalFunds = "TotalFunds";
        public static String RequestDescription = "RequestDescription";
        public static String AttachedW9 = "AttachedW9";
        public static String OnBoardNonProfit = "OnBoardNonProfit";
        public static String CertifyInfoCorrectness = "CertifyInfoCorrectness";
        public static String MatchFunds = "MatchFunds";
        public static String CheckNumber = "CheckNumber";
        public static String CheckAmount = "CheckAmount";
        public static String DeliverCheckTo = "DeliverCheckTo";
        public static String LpcZipCode = "LpcZipCode";
        public static String CharityZipCode = "CharityZipCode";
        public static String IssuedBy = "IssuedBy";
        public static String IssueDate = "IssueDate";
        public static String TVAApprovalFrom = "TVAApprovalFrom";
        public static String TVAApprovalDate = "TVAApprovalDate";
        public static String AttachedW9Mail = "AttachedW9Mail";
        public static String CertifyInfoCorrectnessMail = "CertifyInfoCorrectnessMail";
        public static String MatchFundsMail = "MatchFundsMail";
        public static String IssueDateMail = "IssueDateMail";
        public static String LPCCodeMail = "LPCCodeMail";
        public static String CreatedDateMail = "CreatedDateMail";
        public static String ModifiedDateMail = "ModifiedDateMail";
        public static String AdditionalInfo = "AdditionalInfo";

        public static String Education = "Education";
        public static String DiversityInclusion = "DiversityInclusion";
        public static String HealthAndHumanService = "HealthAndHumanService";
        public static String WorkforceDevelopment = "WorkforceDevelopment";
        public static String CommunityEnrichment = "CommunityEnrichment";
        public static String ArtsCulture = "ArtsCulture";
        public static String DisasterRelief = "DisasterRelief";
        public static String InnovationTech = "InnovationTech";
        public static String OtherExplain = "OtherExplain";
        public static String OnBoardNonProfitMail = "OnBoardNonProfitMail";
        public static String FocusAreaMail = "FocusAreaMail";
        #endregion
        #region --- QuickQuote---
        public static String QuickQuoteID = "QuickQuoteID";
        public static String QuickQuoteDetailsID = "QuickQuoteDetailsId";
        public static String QQStatus = "QQStatus";
        public static String CustomerConcerns = "CustomerConcerns";

        #endregion

        #region --- UserProfile---
        public static String UserProfileId = "UserProfileId";
        #endregion
        public static String ProgramCompanyID = "ProgramCompanyID";
        public static String SchedulingType = "SchedulingType";
        public static String FromPage = "FromPage";
        public static string CompanyTypeId = "CompanyTypeId";

        #region --- GPP Application---
        public static String IsOwnerOfBuilding = "IsOwnerOfBuilding";
        public static String IsProjectContact = "IsProjectContact";
        public static String IsOwnerOfSystem = "IsOwnerOfSystem";
        public static String IsPaymentReceived = "IsPaymentReceived";
        public static String IsSignIA = "IsSignIA";
        public static String IsSignIAA = "IsSignIAA";
        public static String IsSignLPCIA = "IsSignLPCIA";
        public static String IsSignLPCIAA = "IsSignLPCIAA";
        public static String IsReportGenerated = "IsReportGenerated";
        public static String ElectricAccountNumber = "ElectricAccountNumber";
        public static String OutPutDelegation = "OutPutDelegation";
        public static String OwnerOfBuildingName = "OwnerOfBuildingName";
        public static String CIID = "CIID";
        public static String RepresentativeName = "RepresentativeName";
        public static String OutputDelegationProgramID = "OutputDelegationProgramID";
        public static String GCApplicationIncompleteReasons = "GCApplicationIncompleteReasons";
        //public static String IncompleteReasonID = "IncompleteReasonID";
        public static String IncompleteReason = "IncompleteReason";
        public static String SignIADate = "SignIADate";
        public static String SignIAADate = "SignIAADate";
        public static String SignLPCIADate = "SignLPCIADate";
        public static String SignLPCIAADate = "SignLPCIAADate";
        public static String SignLPCIAAByID = "SignLPCIAAByID";
        #endregion
        public static string Access = "Access";


        #region --- Project Images ---
        public static String ProjectImageID = "ProjectImageID";
        public static String SequenceID = "SequenceID";
        #endregion

        #region --- DPP Conract---
        public static String ContractID = "ContractID";
        public static String ESSContractNumber = "ESSContractNumber";
        public static String IsLPCIACompleted = "IsLPCIACompleted";
        public static String FacilityType = "FacilityType";
        public static String GenerationType = "GenerationType";
        public static String PeerReviewID = "PeerReviewID";
        public static String SystemSize = "SystemSize";
        public static String MaxEnergy = "MaxEnergy";
        public static String DeliveryDate = "DeliveryDate";    
        public static String IsSign = "IsSign";
        public static String IsSignCR = "IsSignCR";
        public static String IsSignTVA = "IsSignTVA";
        public static String DPPContractStatus = "DPPContractStatus";
        public static String ContractApprovedOn = "ContractApprovedOn";
        public static String ContractEffectiveDate = "ContractEffectiveDate";
        public static String Approver = "Approver";
        public static String Reviewer = "Reviewer";
        public static String DocumentID = "DocumentID";
        public static String GCApplicationID = "GCApplicationID";
        public static String IsMailingAddressSame = "IsMailingAddressSame";
        public static String FacilityName = "FacilityName";
        public static String BusinessName = "BusinessName";        
        #endregion
        #region ---- History ----
        public static String HistoryData = "HistoryData";
        public static String HistoryType = "HistoryType";
        public static String HistoryKey = "HistoryKey";
        public static String User = "User";
        #endregion ---- History ----
        public static String AdvisorSurveyDocuments = "AdvisorSurveyDocuments";
        public static String ContractorSurveyDocuments = "ContractorSurveyDocuments";
        public static String HealthSurveyDocuments = "HealthSurveyDocuments";

        public static String AvailabilityType = "AvailabilityType";
        public static String ApptVisitType ="AppointmentVisitType";

        public static String AppointmentID = "AppointmentID";
        public static String RecurrenceRule ="RecurrenceRule";
        public static String RecurrenceParentID = "RecurrenceParentID";
        public static String TerritoryType = "TerritoryType";
        public static String TerritoryValue ="TerritoryValue";
        public static String Annotations = "Annotations";
        public static String TimeZoneID = "TimeZoneID";
        public static String Reminder = "Reminder";
        public static String AppointmentCnt = "AppointmentCnt";
        public static String FSSupervisorID = "FSSupervisorID";
        #region ---- Program Submeasure Savings ----
        public static String PSSID = "PSSID";
        public static String PrimaryHeatingTypeID = "PrimaryHeatingTypeID";        
        public static String PrimaryHeatingType = "PrimaryHeatingType";
        public static String SqFootage = "SqFootage";
        public static String CalEffectiveBeginDate = "CalEffectiveBeginDate";
        public static String CalEffectiveEndDate = "CalEffectiveEndDate";
        public static String kW = "kW";
        public static String kWhSavings = "kWhSavings";
        public static String kWhElectrification = "kWhElectrification";
        public static String Therms = "Therms";
        public static String Gallons = "Gallons";
        public static String CO2 = "CO2";
        public static String Trees = "Trees";        
        public static String CarsOffTheRoad = "CarsOffTheRoad";
        public static String MeasureLife = "MeasureLife";
        public static String HouseTypeSA = "HouseTypeSA";
        public static String SqFootageSA = "SqFootageSA";
        public static String RowNum = "RowNum";

        #endregion ---- Program Submeasure Savings ----

        public static String CodeValueID = "CodeValueID";
        public static String ProjectInspectionID = "ProjectInspectionID";

        public static String NMID = "NMID";
        public static String NTID = "NTID";
        public static String NotificationContent = "NotificationContent";

        #region ---- SMSTracking-----
        public static String SMSTrackingID = "SMSTrackingID";
        public static String SMSSID = "SMSSID";
        public static String SMSFrom = "SMSFrom";
        public static String SMSTo = "SMSTo";
        public static String SMSBody = "SMSBody";
        public static String SMSStatus = "SMSStatus";
        public static String SMSCreatedOn = "SMSCreatedOn";
        public static String SMSSentOn = "SMSSentOn";
        public static String SMSUpdatedOn = "SMSUpdatedOn";
        public static String SMSDirection = "SMSDirection";
        public static String SMSNumMedia = "SMSNumMedia";
        public static String SMSNumSegments = "SMSNumSegments";
        public static String SMSMediaURL = "SMSMediaURL";
        public static String SMSURL = "SMSURL";
        public static String SMSErrorCode = "SMSErrorCode";
        public static String SMSErrorMessage = "SMSErrorMessage";
        public static String SMSAccountSID = "SMSAccountSID";
        public static String SMSMessageSID = "SMSMessageSID";
        public static String SMSAPIVersion = "SMSAPIVersion";
        #endregion

        public static String NewSurveyEmail = "NewSurveyEmail";
        public static String NewSurveyPhone = "NewSurveyPhone";
        public static String NewSendSurveySms = "NewSendSurveySms";

        public static String JobType = "JobType";
        
        //Pg Title for GNRL Appt and InProgress Insp
        public static String GeneralAppointment = "General Appointment: ";
        public static String InProgressInspection = "In Progress Inspection: ";
        public static String HOCustomerAddress = "Customer Address: ";
        public static String HomeInformation = "Home Information: ";
        public static String HomeSchedApptVisitType = "Home Schedule Appointment Visit Type: ";
        public static String HomeActivityMeasures= "Home Activity Measures: ";
        public static String NoOfRecords = "NoOfRecords";

        public static String RequestedURL = "RequestedURL";
        public static String PageLoadTime = "PageLoadTime";



    }
    public class HDPerformanceRpt
    {
        public const String HDPerformanceReport = "HDPerformanceReport";
        public const String Year = "RY ";
        public const String Week = "Week ";

        #region --- Tags ---

        public const String Tag1 = "► ";
        public const String Tag2 = "○ ";
        public const String Tag3 = "■ ";
        public const String Tag4 = "• ";
        public const String Tag5 = "- ";

        #endregion --- Tags ---

        #region --- Measures ---

        public const String Refrigerators = "Refrigerators";
        public const String ClothesWashers = "Clothes Washers";
        public const String Dishwashers = "Dishwashers";
        public const String Doors = "Doors";
        public const String HVACUnit = "HVAC Unit";
        public const String TuneUp = "Tune Up";
        public const String DuctSystem = "Duct System";
        public const String AtticInsulation = "Attic Insulation";
        public const String AirSealing = "Air Sealing";
        public const String WaterHeating = "Water Heating";
        public const String Windows = "Windows";

        #endregion --- Measures ---

        #region --- Column Titles ---

        public const String PreviousStart = "Previous Period Start :";
        public const String CurrentStart = "Current Period Start :";
        public const String PreviousEnd = "Previous Period End :";
        public const String CurrentEnd = "Current Period End :";

        public const String GrpColumn1Title = "Total Jobs Submitted and Sales $";
        public const String GrpColumn2Title = "Reported Upgrades";

        public const String GrpColumn1SubTitle1 = "Jobs Reported";
        public const String GrpColumn1SubTitle2 = "Sales $";
        public const String GrpColumn3SubTitle1 = "eScores Completed";
        public const String GrpColumn4SubTitle1 = "Lead Info Generated";

        public const String Rank = "Rank";
        public const String District = "District";
        public const String StoreNumber = "Store Number";
        public const String StoreName = "Store Name";
        public const String Total = "Total";
        public const String HDESalesConsultant = "HDE Sales Consultant";
        public const String Territory = "Territory";
        public const String ProviderName = "Provider Name";

        #endregion --- Column Titles ---

        #region --- Sheet Instructions ---

        public const string SheetInstructions = "Instructions";
        public const string SIHeaderTitle = "eScore Home Depot Performance Report";
        public const string SIHeaderMsg1 = "The duration of eScore Performance is reported in individual worksheets of this report.";
        public const string SIHeaderMsg2 = "Performance is broken out by week, month, and retail year for each business unit.";
        public const string SIWeekly = "Weekly Reporting";
        public const string SIWeeklyMsg1 = "Business Units are ranked based upon overall current week production.";
        public const string SIWeeklyMsg2 = "Weeks are reported Monday through Sunday.";

        public const string SIMonthly = "Monthly Reporting";
        public const string SIMonthlyMsg1 = "Business Units are ranked based upon overall current month production.";
        public const string SIMonthlyMsg2 = "Monthly reports compare equal time lapsed month-over-month.";
        public const string SIMonthlyMsg3 = "For example, if reporting period ended on the 15th, report would indicate prior month performance through the 15th.";

        public const string SIYear = "Retail Year Reporting";
        public const string SIYearMsg1 = "Retail Year runs February 1 - January 31.";
        public const string SIYearMsg2 = "Business Units are ranked based upon overall current retail year production.";
        public const string SIYearMsg3 = "Retail Year reports compare equal time  lapsed year-over-year.";
        public const string SIYearMsg4 = "For example, if reporting period ended on the March 15th, report would compare production between February 1 and March 15 year-over-year.";

        public const string SITerminology = "Reporting Terminology";
        public const string SITerminologyMsg1 = "Conversion % to eScore:   Conversion percentage of all installed measures submitted that result in a completed eScore evaluation";
        public const string SITerminologyMsg2 = "eScores Completed:   A count of all eScore evaluations completed that were generated by the associated entity";
        public const string SITerminologyMsg3 = "Lead Info Generated:   A count of potential leads generated by each completed eScore evaluation. Lead is a sum of all non-10 measures from completed or auto approved rebates.";

        public const string SIDefinitions = "Definitions";
        public const string SIDefinitionTitle1 = "Jobs Reported";
        public const string SIDefinitionTitle2 = "Sales $";
        public const string SIDefinitionTitle3 = "Reported Upgrades";
        public const string SIDefinitionTitle4 = "HDE Sales Consultant";

        //public const string SIWeeklyMsg1 = "Business Units are ranked based upon overall current week production.";
        //public const string SIWeeklyMsg2 = "Weeks are reported Monday through Sunday.";

        #region --- Definitions Sub Headers ---

        public const String SIDefStore = "Store";
        public const String SIDefProviders = "Providers";
        public const String SIDefHDE = "HDE";

        #endregion --- Definitions Sub Headers ---

        public const String SIDefStore4T1Msg1 = "Count of proposals created for the following measures:";
        public const String SIDefStore4T2Msg1 = "Sum of Install Cost for proposals with an Accepted or Submitted status for the following measures:";
        public const String SIDefStore4T3Msg1 = "Sum of Install Quantity for the following measures:";
        public const String SIDefStore4T4Msg1 = "Employees with an asterisk are no longer employed by The Home Depot.";
        #endregion --- Sheet Instructions ---

        #region --- Sheet Instructions ---

        public const string SheetStoresWeek = "Stores by Week";
        public const string SheetStoresMonth = "Stores by Month";
        public const string SheetStoresRetailYear = "Stores by Retail Year";

        #endregion --- Sheet Instructions ---

        #region --- Sheet Instructions ---

        public const string SheetProvidersWeek = "Providers by Week";
        public const string SheetProvidersMonth = "Providers by Month";
        public const string SheetProvidersRetailYear = "Providers by Retail Year";

        #endregion --- Sheet Instructions ---

        #region --- Sheet Instructions ---

        public const string SheetHDEWeek = "HDE by Week";
        public const string SheetHDEMonth = "HDE by Month";
        public const string SheetHDERetailYear = "HDE by Retail Year";

        #endregion --- Sheet Instructions ---
    }

    public class TimeZone
    {
        public const String CST = "CST";
        public const String EST = "EST";
    }

    public class CustomerSummaryRpt
    {
        public static String LPC = "LPC";
        public static String CustomerName = "Customer Name";
        public static String CustomerSiteID = "Customer Site ID";
        public static String CustomerAddress = "Customer Address";
        public const String District = "District";
        public static String HomePhone = "Home Phone";
        public static String CellPhone = "Cell Phone";
        public static String CustomerEmailAddress = "Customer Email Address";
        public static String CustomerRegistrationDate = "Customer Registration Date";
        public static String VisitType = "Visit Type";
        public static String VisitDate = "Visit Date";
        public static String ReferralInfo = "Referral Info";
        public static String FirstVisitAdvisor = "1st Visit Advisor";
        public static String EvaluationFeesPaid = "Evaluation Fees Paid";
        public static String eScoreEvaluationRequestDate = "eScore Evaluation Request Date";
        public static String eScoreEvaluationScheduledDate = "eScore Evaluation Scheduled On Date";
        public static String eScoreEvaluationDate = "eScore Evaluation Date";
        public static String FirstVisitRecommendation1 = "1st Visit Recommendation 1";
        public static String FirstVisitRecommendation2 = "1st Visit Recommendation 2";
        public static String FirstVisitRecommendation3 = "1st Visit Recommendation 3";
        public static String AirSealingScore = "Air Sealing Score";
        public static String AtticInsulationScore = "Attic Insulation Score";
        public static String DuctSystemScore = "Duct System Score";
        public static String LightingScore = "Lighting Score";
        public static String HVACSystemScore = "HVAC System Score";
        public static String AppliancesElectronicsScore = "Appliances/Electronics Score";
        public static String WaterHeatingScore = "Water Heating Score";
        public static String RefrigeratorsScore = "Refrigerators Score";
        public static String WindowsandDoorsScore = "Windows and Doors Score";
        public static String WallInsulationScore = "Wall Insulation Score";
        public static String EvalScore = "Eval Score";
        public static String NumberofReInspections = "Number of Re-Inspections";
        public static String NumberofReScores = "Number of Re-Scores";
        public static String AirSealingScorePTD = "Air Sealing Score (PTD)";
        public static String AtticInsulationScorePTD = "Attic Insulation Score (PTD)";
        public static String DuctSystemScorePTD = "Duct System Score (PTD)";
        public static String LightingScorePTD = "Lighting Score (PTD)";
        public static String HVACSystemScorePTD = "HVAC System Score (PTD)";
        public static String AppliancesElectronicsScorePTD = "Appliances/Electronics Score (PTD)";
        public static String WaterHeatingScorePTD = "Water Heating Score (PTD)";
        public static String RefrigeratorsScorePTD = "Refrigerators Score (PTD)";
        public static String WindowsandDoorsScorePTD = "Windows and Doors Score (PTD)";
        public static String WallInsulationScorePTD = "Wall Insulation Score (PTD)";
        public static String eScorePTD = "eScore (PTD)";
        public static String TotalNumberOfUpgrades = "Total Number Of Upgrades";
        public static String AutoApprovedUpgradesPTD = "Auto Approved Upgrades (PTD)";
        public static String QCNNumbers = "QCN Numbers";
        public static String CustomerSpending = "Customer Spending $";
        public static String RebatesPaid = "Rebates Paid $";
        public static String eScoreReward = "eScore Reward $";
        public static String kWhSavings = "kWh Savings";
        public static String DICFL = "DI CFL";
        public static String DIShowerheads = "DI Showerheads";
        public static String PTDRecommendation1 = "PTD Recommendation 1";
        public static String PTDRecommendation2 = "PTD Recommendation 2";
        public static String PTDRecommendation3 = "PTD Recommendation 3";
        public static String LPCProgramModel = "LPC Program Model";
        public static String ProgramModel = "Program Model";
        public static String AirSealing = "Air Sealing";
        public static String AirSealingQCN = "Air Sealing QCN";
        public static String AtticInsulation = "Attic Insulation";
        public static String AtticInsulationQCN = "Attic Insulation QCN";
        public static String DuctSystem = "Duct System";
        public static String DuctSystemQCN = "Duct System QCN";
        public static String HVACSystem = "HVAC System";
        public static String HVACQCN = "HVAC QCN";
        public static String WaterHeating = "Water Heating";
        public static String WaterHeatingQCN = "Water Heating QCN";
        public static String Refrigerators = "Refrigerators";
        public static String RefrigeratorQCN = "Refrigerator QCN";
        public static String WindowsDoors = "Windows/Doors";
        public static String WindowsDoorsQCN = "Windows/Doors QCN";
        public static String WallInsulation = "Wall Insulation";
        public static String WallInsulationQCN = "Wall Insulation QCN";
        public static String StoreNumber = "Store Number";
        public static String Lighting = "Lighting";
        public static String LightingQCN = "Lighting QCN";
        public static String ApplianceElec = "Appliance/Electronics";
        public static String ApplianceElecQCN = "Appliance/Electronics QCN";
    }

    public class SignersRole
    {
        public static String Customer = "Customer";
        public static String Client = "Client";
        public static String Reviewer = "Reviewer";
        public static String Approver = "Approver";
        public static String Landlord = "Landlord";
        public static String LPC = "LPC";
        public static String TVA = "TVA";
    }
    public class RoleNames
    {
        public static String Administrator = "Administrator";
        public static String OperationsMgr = "Operations Mgr";
        public static String Advisor = "Advisor";
        public static String FSMgr = "FS Mgr";
        public static String FSSupervisor = "FS Supervisor";
        public static String FieldSupervisor = "Field Supervisor";
        public static String LPCManagedAdvisor = "LPC Managed Advisor";
        public static String LPCManagedAdmin = "LPC Managed Admin";
        public static String LPCManagedUser = "LPC Managed User";
        public static String LPCAdmin = "LPC Admin";
        public static String LPCUser = "LPC User";
    }
    public class DeliveryModel
    {
        public static String TVADelivered = "TVA Delivered";
        public static String LPCManaged = "LPC Managed";
        
    }
}
 