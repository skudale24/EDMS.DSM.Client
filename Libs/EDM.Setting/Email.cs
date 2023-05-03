using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    public class Email
    {
        #region --- Constants ---
        public enum Type
        {
            AdvSchedAppt = 101,
            AdvSurvey = 102,
            CustSchedAppt = 103,
            CustEvalReq = 104,
            ContraSchedAppt = 105,
            ContraExpDocs = 106,
            ContraSurvey = 107,
            InspPastDue = 108,
            ContraFailInsp = 109,
            AdvCanAppt = 110,
            CusteScoreComplete = 111,
            PwdForgot = 112,
            CustAcctCreated = 113,
            ContraPassInsp = 114,
            ContraPassVirtual = 1040,
            CustReschedAppt = 115,
            VacationAutoResched = 116,
            CustInspReq1 = 117,
            ContraIncompletePI = 118,
            SubContraApproved = 119,
            ISMOrderAddStock = 120,
            ISMOrderAddStockLPC = 121,
            ContraApproved = 122,
            ContraPaymentNoti = 123,
            PwdReset = 124,
            ContraCustInvite = 125,
            ContraDetails = 126,
            ContraContactCust = 127,
            ContraEnrolConfirm = 128,
            ContraCanAppt = 129,
            VacationSchedule = 130,
            VacationCustCanAppt = 131,
            ContraExpDocs7 = 132,
            ContraExpDocs15 = 133,
            ContraExpDocs30 = 134,
            ContraFailInsp4Images = 135,
            LPCPayoutCheck = 136,
            LPCBankInfoChange = 137,
            VMFUploadLoanFile = 139,
            QCNAccountInfoUpdate = 140,
            AutoApprovedReport = 141,
            ConfirmResetPassword = 142,
            AdvisorSchedule = 143,
            CustomerFollowUp = 144,
            ProjectDurationReschedule = 145,
            ContraSchedApptLPC = 146,
            ContractorSchedule = 147,
            HDSchedule = 148,
            HDScheduleLPC = 149,
            HDInspPass = 150,
            HDInspVirtual = 1041,
            HDInspFail = 151,
            CustInspPass = 152,
            CustInspVirtual = 234,
            CustInspFail = 153,
            CustomerIncompletePI = 154,
            HDIncompletePI = 155,
            ContraCanApptLPC = 156,
            HDCanAppt = 157,
            PwdForgotVMF = 158,
            PwdForgotCust = 159,
            PwdForgotContra = 160,
            PwdForgotHD = 161,
            PwdResetVMF = 162,
            PwdResetCust = 163,
            PwdResetHD = 164,
            PwdResetContra = 165,
            AdvSchedApptLPC = 166,
            CustReschedApptLPC = 167,
            CustSchedApptLPC = 168,
            CustInspReq2 = 169,
            ISMOrderAddStockYOW = 170,
            ISMOrderAddStockLPCYOW = 171,
            CustPasswordReset = 172,
            CustReportReminder = 173,
            LPCPayout = 174,
            InspPastDueLPC = 175,
            CustAcctCreatedTmpPwd = 176,
            CustAcctCreatedTmpPwdKiosk = 177,
            CustAcctCreatedSelfe = 178,
            HDAppointmentScheduled = 179,
            InventoryAlert = 180,
            CustomerReferral = 181,
            UtilityDataImportFail = 182,
            UtilityDataImportSuccess = 183,
            CustSchedAppt1 = 184,
            CustSchedAppt3 = 185,
            InspNotes = 186,
            CustEvalReqLPC = 187,
            CustAcctCreatedLPC = 188,
            AutoApprovedReportLPC = 189,
            AdvSurveyLPC = 190,
            ContraSurveyLPC = 191,
            CusteScoreCompleteLPC = 192,
            CustReportReminderLPC = 193,
            CustomerFollowUpLPC = 194,
            ContraDetailsLPC = 195,
            PayoutBatchChange = 196,
            SFTPUploadFail = 197,
            SFTPUploadSuccess = 198,
            SymphonyEventEnd = 199, /* Dec 12, 2017 | Nibha Kothari | ES-4101: PGEBGDR: EDMS.Service: Event-ending Email to Customer */
            CustAggWelcome = 200,   /* Dec 14, 2017 | Nibha Kothari | ES-4311: PGEBGDR: EDMS.Service: Customer welcome email */
            FileListNotFound = 201, /* Jan 31, 2018 | Swapnil Bhave | DSM-17: Merge feature/AWS S3 Research into current EDM-DSM / Develop branch */
            CampaignContractorContacted = 202, /* June 06, 2018 | Swapnil Bhave | DSM-433 Create an API to support HVAC email campaign */
            WaterHeaterCampaignContractorContacted = 203, /* July 27 2018|Swapnil Bhave|DSM-489: WaterHeater Campaign */
            WindowsAndDoorsCampaignContractorContacted = 204, /* Aug 11 2018|Swapnil Bhave|DSM-518: WindowsAndDoors Campaign */
            BasicEmailTemplate = 999,
            CustomEmail = 1001,
            CustAcctNotifyContactCenter = 205, /*August 28 2018 - Customer registration info sent to Contact Center*/
            ReliefFund = 206,
            HUPHealthSurvey = 207,
            GCAppApprovalInstallationMailToCust = 208,
            GCAppApprovalInstallationMailToQCN = 209,
            GCAppIncompleteMailToQCN = 210,
            GCAppFinalReviewMailToCust = 211,
            GCAppFinalApprovalToCust = 212,
            GCAppFinalApprovalToQCN = 213,
            GCAppFinalStatusToCust = 214,
            GCAppFinalStatusToQCN = 215,
            GCAppReadyForReviewStatusToQCN = 216,
            GCAppSubmitToCust = 217,
            GCAppFeeProcessedToCust = 218,
            GCAppDraftToCust = 219,
            GCAppSubmitToCQCN = 220,
            GCAppSubmitToCLPC = 221,
            GCAppReadyForReviewToCLPC = 222,
            GCAppSubmitConfirmToQcn = 223,
            GCLPCAcctCreated = 224,
            GCAppRejectedToCust = 225,
            GCAppSignAndSubmitoCust = 226,
            GCAppIAAReviewToLPC = 227,
            GCReadyForSignatureToLPC = 228,
            CustSchedApptVirtual=229,
            CustReschedApptVirtual=230,
            AdvCanApptVirtual=231,
            CustReschedApptLPCVirtual=232,
            SelfSchedActReqVirtual=233,
            DPPCRESSRegistration = 235,
            DPPCustRegCommercialNoGC = 236,
            DPPFinalTurnkeyLPCManNoGC = 237,
            DPPApplSubmitCommOrNoGC = 238,
            DPPApplSubmitLPCManGCAndDPP = 239,
            DPPResiCustRegLPCMan = 240,
            DPPApplSubmitTKGCAndDPP = 241,
            DPPCustRegTKResidential = 242,
            DPPLPCManTKCRPeerReview = 243,
            DPPContractUpdateTKLPCManW9 = 244,
            DPPLPCManTKTVAPeerReview2 = 245,
            DPPLPCManTKTVASign = 246,
            DPPIncompleteStatus = 247,
            DPPPeerReviewComplete = 248,
            CustInspCorrection = 249,
            PandaDocDPPContract = 249,
            PandaDocW9 = 250,
            ConInstallDateReq = 1046,
            QuickQuoteCompleted = 1047,
            CustInspectionReportComplete = 261,
            CustInspectionReportCompleteLPC = 262,
            DocuSignW9 = 263,
            DocuSignDPPContract = 264,
            GCAppCustomerAgreement = 265,
            GCAppAgreementReviewToLPC = 266,
            HUPOnlineApplication=267,
            HUPLandlordApplicationRequest=269,
            HUPOnlineApplicationRenter = 270,
            UserExpDocs = 1055,
            UserExpDocs7 = 1056,
            UserExpDocs15 = 1057,
            UserExpDocs30 = 1058,
            DailyRecapEmail = 1059,
            HUPOnlineApplicationCreatedCP = 271,
            HUPOnlineApplicationReceivedCP = 272,
            HUPOnlineApplicationNotEligible = 273,
            HUPOnlineApplicationNotEligibleLPC = 290,
            HUPOnlineApplicationIncomplete = 274,
            HUPOnlineApplicationIncompleteLPC = 291,
            HUPOnlineApplicationApproved = 275,
            HUPOnlineApplicationApprovedLPC = 292,
            HUPOnlineApplicationReferral = 276,
            HUPOnlineApplicationReferralLPC = 289,
            CustInspSchedApptVirtual = 277,
            CustInspSchedAppt = 278,
            InspectionAppointmentReminder = 279,
            ApplicationEligibleForUpgrades = 280,
            ApplicationEligibleForUpgradesLPC = 294,
            EvaluationAppointmentReminder = 281,
            EvaluationAppointmentReminderLPC = 293,
            HUPInstallationScheduled = 282,
            HUPInstallationScheduledLPC = 295,
            HUPInstallationReminder = 283,
            HUPInstallationReminderLPC = 296,
            AppointmentFollowup = 284,
            AppointmentFollowupLPC = 297,
            WinterTips = 285,
            SummerTips = 286,
            ConEvalPhotoNotify = 287,
            ContributionRequestFrom = 288
        }
        public enum ScopeType
        {
            Template = 0,
            System = 1,
            Custom = 2
        }
        #endregion

        #region --- Properties ---
        public String Module = String.Empty;
        public String Message = String.Empty;
        //public String ConfigKey = String.Empty;
        public SqlDb Db;
        public long ProgramId;
        public long ByUserId = 0;

        public long ObjectId = 0;
        public String ObjectType = String.Empty;
        public String Subject = String.Empty;
        public String ToEmail = String.Empty;
        public String CcEmail = String.Empty;
        public String BccEmail = String.Empty;
        public String Body = String.Empty;
        public int StatusId = 0;
        public long EmailType = 0;
        public long EmailCategory = 0;
        public String Notes = String.Empty;
        public int Scope = Convert.ToInt32(ScopeType.System);
        public long ModifiedByID = 0;
        public String ModifiedDate = String.Empty;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
            }
        }
        #endregion

        #region --- Constructors ---
        public Email() { ProgramId = Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; Init(); }
        public Email(String module) : this() { Module = module; }
        public Email(String configKey, long programId) : this() { ProgramId = programId; Init(configKey); }
        public Email(String module, EDM.Setting.Email.Type objectId) : this() { Module = module; ObjectId = (int)objectId; }

        public Email(String module, String configKey, EDM.Setting.Email.Type objectId) : this(module)
        {
            Module = module; 
            ConfigKey = configKey;
            ObjectId = (int)objectId;
        }

        #endregion

        #region --- Private Methods ---
        public void Init(String configKey = "") 
        { 
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
        }
        #endregion --- Private Methods ---

        public Boolean GetById()
        {
            try
            {
                if (ObjectId <= 0) { Message = "ObjectId is required."; return false; }
                if (!GetById(ObjectId)) return false;

                if (StatusId != 1)
                    Body = String.Empty;

                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }
        public Boolean GetById(Type objectId)
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ObjectID"] = (int)objectId;
                prms["ProgramID"] = ProgramId;
                return Get4Id(prms);
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }
        public Boolean GetById(long objectId)
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ObjectID"] = objectId;
                prms["ProgramID"] = ProgramId;
                return Get4Id(prms);
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }
        protected Boolean Get4Id(Hashtable prms)
        {
            try
            {
                Db.SetSql("p_GET_EmailSetting", prms);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) { Message = "Error retrieving details from DB"; return false; }

                DataRow dr = ds.Tables[0].Rows[0];

                //ProgramId = SqlDb.CheckLongDBNull(dr["ProgramId"]);
                ObjectId = SqlDb.CheckLongDBNull(dr["ObjectID"]);
                ObjectType = SqlDb.CheckStringDBNull(dr["ObjectType"]);
                Subject = SqlDb.CheckStringDBNull(dr["Subject"]);
                ToEmail = SqlDb.CheckStringDBNull(dr["ToEmail"]);
                CcEmail = SqlDb.CheckStringDBNull(dr["CcEmail"]);
                BccEmail = SqlDb.CheckStringDBNull(dr["BccEmail"]);
                Body = SqlDb.CheckStringDBNull(dr["Body"]);
                StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);

                EmailType = SqlDb.CheckLongDBNull(dr["EmailType"]);
                EmailCategory = SqlDb.CheckLongDBNull(dr["EmailCategory"]);
                Notes = SqlDb.CheckStringDBNull(dr["Notes"]);
                Scope = SqlDb.CheckIntDBNull(dr["Scope"]);
                ModifiedByID = SqlDb.CheckLongDBNull(dr["ModifiedByID"]);
                ModifiedDate = SqlDb.CheckStringDBNull(dr["ModifiedDate"]);

                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean GetByName()
        {
            try
            {
                if (String.IsNullOrEmpty(ObjectType)) { Message = "ObjectType is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ObjectType"] = ObjectType;
                prms["ProgramID"] = ProgramId;

                Boolean retVal = Get4Id(prms);
                if (!retVal) return false;

                if (StatusId != 1)
                    Body = String.Empty;

                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                prms["Scope"] = Scope;

                Db.SetSql("p_GET_EmailSettings", prms);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) { Message = "Error retrieving details from DB"; return null; }

                return ds;
            }
            catch (Exception ex) { Message = ex.ToString(); return null; }
        }

        public Boolean Save()
        {
            try
            {
                // if (ObjectId <= 0) { Message = "ObjectId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                prms["ObjectID"] = ObjectId;
                prms["ObjectType"] = ObjectType;
                prms["Subject"] = Subject;
                prms["ToEmail"] = ToEmail;
                prms["CcEmail"] = CcEmail;
                prms["BccEmail"] = BccEmail;
                prms["Body"] = Body;
                if (StatusId > 0) prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                prms["EmailType"] = EmailType;
                prms["EmailCategory"] = EmailCategory;
                prms["Notes"] = Notes;
                prms["Scope"] = Scope;

                Db.SetSql("p_AU_EmailSetting", prms);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) { Message = "Error saving details to DB"; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                return SqlDb.CheckLongDBNull(dr["ObjectID"]) <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public string ReplaceGeneralSubstitutionTags(string body, long extProgramId = 0, long lpcId = 0)
        {
            try
            {
                long GC_ProgramID = 8, localProgramId = 1, HUP_ProgramID = 2;
                localProgramId = extProgramId > 0 ? extProgramId : ProgramId > 0 ? ProgramId : 1;

                string ImageUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl, localProgramId, configKey: ConfigKey);
                string apUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.APUrl, localProgramId, configKey: ConfigKey);
                string cpUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.CPUrl, localProgramId, configKey: ConfigKey);
                string qcnUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.QCNUrl, localProgramId, configKey: ConfigKey);
                string landingUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.LandingUrl, localProgramId, configKey: ConfigKey);
                string selfAuditURL = EDM.Setting.DB.GetByName(EDM.Setting.Key.SelfAuditURL, localProgramId, configKey: ConfigKey);

                string EmailImagesUrl = ImageUrl + "/CDN/EmailImages";
                
                if (lpcId > 0)
                {
                    string LogoIMG = ImageUrl + "/CDN/TVA_LPCLogo/logo_" + lpcId + ".png";
                    string GCLogoIMG = String.Empty;
                    if ((localProgramId & GC_ProgramID) == localProgramId || (localProgramId & GC_ProgramID) == GC_ProgramID)
                    {
                        LogoIMG = ImageUrl + "/CDN/LPCLogo/logo_" + lpcId + ".png";
                        GCLogoIMG = ImageUrl + "/CDN/LPCLogo/logo_" + lpcId + ".png";
                        LogoIMG = "<img alt=\"\" class=\"max-wt\" style=\"float:right;border-width:0px;border-style:solid;\" src=\"" + LogoIMG + "\" >";
                    }
                    if ((localProgramId & HUP_ProgramID) == localProgramId || (localProgramId & HUP_ProgramID) == HUP_ProgramID)
                    {
                        LogoIMG = ImageUrl + "/CDN/TVA_HUP_LPCLogo/HUP_logo_" + lpcId + ".png";
                    }
                    body = body.Replace("%%LPCLOGO%%", LogoIMG);
                    body = body.Replace("%%GCLPCLOGO%%", GCLogoIMG);
                }
                string TVAEmailImagesUrl = EmailImagesUrl + "/EnergyRightLogo.png";
                if ((localProgramId & GC_ProgramID) == localProgramId || (localProgramId & GC_ProgramID) == GC_ProgramID)
                {
                    TVAEmailImagesUrl = EmailImagesUrl + "/TVA_Green_logo.png.png";
                }
                if ((localProgramId & HUP_ProgramID) == localProgramId || (localProgramId & HUP_ProgramID) == HUP_ProgramID)
                {
                    TVAEmailImagesUrl = EmailImagesUrl + "/TVA_EnergyRight_HomeUplift_Final.png";
                }

                body = body.Replace("%%TVALOGO%%", TVAEmailImagesUrl);
                body = body.Replace("%%EMAILIMAGESURL%%", EmailImagesUrl);
                body = body.Replace("%%YEAR%%", DateTime.Today.Year.ToString());
                body = body.Replace("%%APURL%%", apUrl);
                body = body.Replace("%%CPURL%%", cpUrl);
                body = body.Replace("%%QCNURL%%", qcnUrl);
                body = body.Replace("%%SELFAUDITURL%%", selfAuditURL);
                body = body.Replace("%%LANDINGURL%%", landingUrl);

                // Following block needs to remove once we updated all the email template with about tags.
                body = body.Replace("%%DOMAIN%%", cpUrl);                
                body = body.Replace("%%SELF_AUDIT%%", selfAuditURL);
                body = body.Replace("%%CUSTOMER_URL%%", cpUrl);
                body = body.Replace("%%RESCH_URL%%", cpUrl);
                body = body.Replace("%%CONTRACTOR_URL%%", qcnUrl);
                body = body.Replace("%%HELP_URL%%", cpUrl);
                body = body.Replace("%%ADMINDOMAIN%%", apUrl);
                body = body.Replace("%%CURRENTYEAR%%", DateTime.Now.Year.ToString());                
                body = body.Replace("%%LOGINURL%%", qcnUrl);
                // end block


                return body;
            }
            catch (Exception ex) { Message = ex.ToString(); return body; }
        }
    }
}
