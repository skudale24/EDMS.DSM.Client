using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Group
{
    public enum TypeId
    {
        District = 11,
        ApplicantPersonal = 11100,
        ApplicantIncome = 11200,
        ApplicantBenefits = 11300,
        ApplicationDocs = 12000,
        CustomerSAGI = 10000,
        EnergyReductionPlan = 16000,
        Agency = 15000,
        AuditorUser = 15001,
        AgencyUser = 15004,
        ContractorEnrollment = 4,
        ContractorInvoice = 15005,
        AgencyInvoice = 15006,
        AuditBulkFiles = 16000,
        WOAdditionalDocument = 18000,
        ApplicationAdditionalDocument = 19000,       
        WorkOrder = 50,
        ApplicationSF = 10,
        Inspection = 60,
        PIP = 17000,
        MembersDoc = 12500,
        GPPContracts = 20000,
        GPPDocTypeId = 15547,
        MembersDoc_HomeOwnersIdentification=12501,
        MembersDoc_ProofOfIncome=12502,
        MembersDoc_ProofOfOwnership=12503,
        ReliefFund = 51,
        ReliefFundNonProfitCerts = 52,
        GCApplicationDocs = 30005,
        //GCApplication_MembersDoc = 30006,
        GCApplicationDoc_AdditionalInformation = 30006,
        GCApplicationDoc_FinalPDF = 30008,
        CustomerSiteDocs = 25088,
        DPPContractIADocumentObjectType = 30009,
        ContractDocumentsObjectType = 30040,
        SurveyDocs = 30010,
        ProfilePictureDocs = 30012,
        EPBDocs = 30013,
        QuickQuoteDocs = 30014,
        QuickQuoteMeasure1 = 30015,
        QuickQuoteMeasure2 = 30016,
        QuickQuoteMeasure3 = 30017,
        QuickQuoteMeasure5 = 30018,
        QuickQuoteMeasure7 = 30019,
        QuickQuoteMeasure9 = 30020,
        DeliveryPreferenceDocs = 30021,
        ContributionForm = 30041
    }

    /// <summary>
    /// Maps to database Groups table.
    /// </summary>
    public class Group
    {
        #region --- Properties ---
        public String Module;
        public String Message;

        public long ProgramId;
        public long ByUserId;
        public String ByUser;
        public String TranDate;

        public long Id;
        public String Name = String.Empty;
        public String Description = String.Empty;
        public int Type = 0;
        public int Sequence = 1;
        public int System = 0;
        public int StatusId = 1;
        #endregion

        #region --- Constructors ---
        public Group() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public Group(String module) : this() { Module = module; }
        public Group(String module, long id) : this(module) { Id = id; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module, Name, Type, Sequence, System, StatusId are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public virtual Boolean Add()
        {
            String logParams = String.Empty;
            String SqlforLogParam = string.Empty;
            try
            {
                if (Name.Length <= 0) { Message = "GroupName is required."; return false; }
                if (Type <= 0) { Message = "GroupType is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["GroupName"] = Common.Helper.Capitalize(Name);
                if (Description.Length > 0) prms["GroupDesc"] = Description;
                prms["GroupType"] = Type;
                prms["GroupSeq"] = Sequence;
                prms["System"] = System;
                prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

               
                logParams = MsSql.GetSqlStmt(String.Empty, prms, out SqlforLogParam);

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Group", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "Add", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record";
                    Common.Log.Info(Module + ":EDM.Group.Group", "Add", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Id = MsSql.CheckLongDBNull(dr["GroupID"]);
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                return Id <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "Add", ex, SqlforLogParam);
                return false;
            }
        }

        /// <summary>
        /// Module, Id are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Update()
        {
            String logParams = String.Empty;
            String SqlforLogParam = string.Empty;
            try
            {
                if (Id <= 0) { Message = "GroupId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["GroupID"] = Id;
                if (Name.Length > 0) prms["GroupName"] = Common.Helper.Capitalize(Name);
                if (Description.Length > 0) prms["GroupDesc"] = Description;
                if (Type > 0) prms["GroupType"] = Type;
                if (Sequence > 0) prms["GroupSeq"] = Sequence;
                if (System >= 0) prms["System"] = System;
                if (StatusId > 0) prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                logParams = MsSql.GetSqlStmt(String.Empty, prms, out SqlforLogParam);

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Group", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "Update", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error updating record.";
                    Common.Log.Info(Module + ":EDM.Group.Group", "Update", SqlforLog + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                return MsSql.CheckLongDBNull(dr["GroupID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "Update", ex, SqlforLogParam);
                return false;
            }
        }

        /// <summary>
        /// Module is required.
        /// </summary>
        public virtual DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (Name.Length > 0) prms["GroupName"] = Name;
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Groups", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "GetAll", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "GetAll", ex);
                return null;
            }
        }
        /// <summary>
        /// Module, Type are required.
        /// </summary>
        protected DataSet GetByType()
        {
            try
            {
                if (Type <= 0) { Message = "Type is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["GroupType"] = Type;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Groups", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "GetByType", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "GetByType", ex);
                return null;
            }
        }
        /// <summary>
        /// Module is required.
        /// </summary>
        protected DataSet GetActiveByType()
        {
            try
            {
                if (Type <= 0) { Message = "Type is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["GroupType"] = Type;
                prms["StatusID"] = (int)EDM.Codes.Value.Status.Active;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Groups", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "GetActiveByType", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "GetActiveByType", ex);
                return null;
            }
        }

        /// <summary>
        /// Module, Id are required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "GroupId:" + Id;
            try
            {
                if (Id <= 0) { Message = "GroupId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["GroupID"] = Id;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Group", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Group.Group", "GetById", SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.Group.Group", "GetById", sql + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                Name = MsSql.CheckStringDBNull(dr["GroupName"]);
                Description = MsSql.CheckStringDBNull(dr["GroupDesc"]);
                Type = MsSql.CheckIntDBNull(dr["GroupType"]);
                Sequence = MsSql.CheckIntDBNull(dr["GroupSeq"]);
                System = MsSql.CheckIntDBNull(dr["SystemID"]);
                StatusId = MsSql.CheckIntDBNull(dr["StatusID"]);
                ByUser = MsSql.CheckStringDBNull(dr["ByUser"]);
                TranDate = MsSql.CheckStringDBNull(dr["TranDate"]);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Group.Group", "GetById", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}