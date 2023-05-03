using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocType
{
    public class Id
    {
        public const long DoestheAttichaveKneewalls = 5021;
        public const long DoesthehomehavekneewallinsulationlessthanR11 = 5022;
        /* Dec 18, 2017 | Nibha Kothari | ES-4336: PGEBGDR: AP: Edit Customer: Documents link */
        public const long EnergyReductionPlan = 16001;
        public const int CompanyProfilePhoto = 15714;
        public const int EPBDocuments = 15715;
    }
    public class Name
    {
        public const String PackageOrSplitSystemNew = "Package or Split System (new)";
        public const String IndoorModelNumber = "Indoor Model Number";
        public const String EvaporatorCoilModelNumber = "Evaporator Coil Model Number";
        public const String OwnerContact = "OwnerContact";
        public const String YearBuilt = "YearBuilt";
        public const String DisabilityProof = "DisabilityProof";
        public const String DisabilityAssistance = "DisabilityAssistance";
        public const String HasDisability = "HasDisability";
        public const String Employer = "Employer";
        public const String IncomeSource = "IncomeSource";
        public const String HasIncome = "HasIncome";
        public const String DeedName = "DeedName";
        public const String MfrdHomeTitle = "MfrdHomeTitle";
        public const String OwnerName = "OwnerName";
        public const String OwnerAddress = "OwnerAddress";
        public const String RentalProperty = "RentalProperty";
        public const String OwnIt = "OwnIt";
        public const String HouseType = "HouseType";
        public const String SSN = "SSN";
        public const String IncomeGrossMonthly = "IncomeGrossMonthly";
        public const String BirthDate = "BirthDate";
    }

    public class DocType
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;
        public String ByUser = String.Empty;
        public String TranDate = String.Empty;

        private SqlDb Db;
        private EDM.Common.Log Lg;
        public String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.DocType.DocType";
            }
        }

        public long Id;
        public long GroupId = 0;
        public String GroupName = String.Empty;
        public String Name = String.Empty;
        public String Description = String.Empty;
        public int Version = 0;
        public int DisplayIn = 0;
        public int System = 0;
        public int Sequence = 1;
        public int Param = 0;
        public int StatusId = 0;
        public String Validation = String.Empty;
        public int Required = 0;
        public int Inventory = 0;
        public long DocTypeID;
        //public long DocTypeGroupID;
        public string DocTypeName;

        public long DocPropertiesID;
        public DateTime ExpiryDate;
        public string ValidityStatus;

        #endregion --- Properties ---

        #region --- Constructors ---
        public DocType() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; ConfigKey = String.Empty; }
        public DocType(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public DocType(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        public DocType(String module, long id) : this(module) { Id = id; }
        #endregion --- Constructors ---

        #region --- Public Methods ---
        /// <summary>
        /// Module, GroupId, Name, Version are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Add()
        {
            String logParams = "GroupId:" + GroupId + "|Name:" + Name + "|Version:" + Version + "|ByUserId:" + ByUser;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (GroupId <= 0) { Message = "GroupId is required."; return false; }
                if (String.IsNullOrEmpty(Name)) { Message = "DocTypeName is required."; return false; }
                if (Version <= 0) { Message = "Version is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["DocTypeGroupID"] = GroupId;
                prms["DocTypeName"] = Name;
                if (!String.IsNullOrEmpty(Description)) prms["DocTypeDesc"] = Description;
                prms["DocTypeVersion"] = Version;
                prms["DisplayIn"] = DisplayIn;
                prms["ByUserID"] = ByUserId;
                prms["StatusID"] = StatusId;

                Db.SetSql("p_AU_DocType", prms);
                Lg.Debug("Add", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding record";
                    Lg.Info("Add", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                Id = SqlDb.CheckLongDBNull(dr["DocTypeID"]);

                if (Id <= 0)
                {
                    Lg.Error("Add", new Exception(Message), logParams);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Add", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module, DocTypeId are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Update()
        {
            String logParams = "Id:" + Id + "|ByUserId:" + ByUser;
            try
            {
                if (Id <= 0) { Message = "DocTypeId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = Id;
                if (GroupId > 0) prms["DocTypeGroupID"] = GroupId;
                if (!String.IsNullOrEmpty(Name)) prms["DocTypeName"] = Name;
                if (!String.IsNullOrEmpty(Description)) prms["DocTypeDesc"] = Description;
                if (Version > 0) prms["DocTypeVersion"] = Version;
                if (DisplayIn > 0) prms["DisplayIn"] = DisplayIn;
                prms["ByUserID"] = ByUserId;
                if (StatusId > 0) prms["StatusID"] = StatusId;

                Db.SetSql("p_AU_DocType", prms);
                Lg.Debug("Update", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding record";
                    Lg.Info("Update", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                if (SqlDb.CheckLongDBNull(dr["DocTypeID"]) <= 0)
                {
                    Lg.Error("Update", new Exception(Message), logParams);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Update", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module is required.
        /// </summary>
        public DataSet GetAll()
        {
            String logParams = "DocTypeId:" + Id;
            try
            {
               
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                Db.SetSql("p_GET_DocTypes", prms);
                Lg.Debug("GetAll", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex);
                return null;
            }
        }

        public DataSet GetAllGPPDocType()
        {
            //long logParams = "DocTypeId:" + Id;
            try
            {
                Hashtable prms = new Hashtable();
                Db.SetSql("p_GET_GPPDocType", prms);
                Lg.Debug("GetAll", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetAll", ex);
                return null;
            }
        }
        /// <summary>
        /// Module is required.
        /// </summary>
        public DataSet GetActive()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["StatusID"] = EDM.Setting.Status.Active;
                Db.SetSql("p_GET_DocTypes", prms);
                Lg.Debug("GetActive", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetActive", ex);
                return null;
            }
        }
        /// <summary>
        /// Module, GroupId are required.
        /// </summary>
        public DataSet GetByGroupId(EDM.Group.TypeId groupId)
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["StatusID"] = EDM.Setting.Status.Active;
                prms["GroupID"] = (long)groupId;
                if(!string.IsNullOrEmpty(DocTypeName)) prms["DocTypeName"] = DocTypeName;

                Db.SetSql("p_GET_DocTypes", prms);
                Lg.Debug("GetByGroupId", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetByGroupId", ex);
                return null;
            }
        }

        /// <summary>
        /// Module, DocTypeId are required.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "DocTypeId:" + Id;
            try
            {
                if (Id <= 0) { Message = "DocTypeId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = Id;

                Db.SetSql("p_GET_DocType", prms);
                Lg.Info("GetById", Db.SqlStmt);
                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "No record found.";
                    Lg.Info("GetById", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                GroupId = SqlDb.CheckLongDBNull(dr["DocTypeGroupID"]);
                GroupName = SqlDb.CheckStringDBNull(dr["DocTypeGroupName"]);
                Name = SqlDb.CheckStringDBNull(dr["DocTypeName"]);
                Description = SqlDb.CheckStringDBNull(dr["DocTypeDesc"]);
                Version = SqlDb.CheckIntDBNull(dr["DocTypeVersion"]);
                DisplayIn = SqlDb.CheckIntDBNull(dr["DisplayIn"]);
                System = SqlDb.CheckIntDBNull(dr["SystemID"]);
                TranDate = SqlDb.CheckStringDBNull(dr["TranDate"]);
                ByUser = SqlDb.CheckStringDBNull(dr["ByUser"]);
                StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetById", ex, logParams);
                return false;
            }
        }
        public DataSet GetDocTypesByObject(long ObjectID,long ObjectType)
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.ObjectID] = ObjectID;
                prms[EDM.Setting.Fields.ObjectType] = ObjectType;
                prms[EDM.Setting.Fields.StatusID] = StatusId;

                Db.SetSql("p_GET_DocTypesByObject", prms);
                Lg.Debug("GetDocTypesByObject", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetDocTypesByObject", ex);
                return null;
            }
        }

        public Boolean AddUpdateDocTypesAndProperties(long ObjectID)
        {
            String logParams = "Id:" + Id + "|ByUserId:" + ByUser;
            try
            {
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                if (ObjectID > 0) prms["ObjectID"] = ObjectID;
                if (DocTypeID > 0) prms["DocTypeID"] = DocTypeID;
                if (ExpiryDate != new DateTime()) prms["ExpiryDate"] = ExpiryDate;
                if (!String.IsNullOrEmpty(ValidityStatus)) prms["ValidityStatus"] = ValidityStatus;               
                prms["ByUserID"] = ByUserId;
                if (StatusId > 0) prms["StatusID"] = StatusId;

                Db.SetSql("P_AU_DocProperties", prms);
                Lg.Debug("AddUpdateDocTypesAndProperties", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding record";
                    Lg.Info("AddUpdateDocTypesAndProperties", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                if (SqlDb.CheckLongDBNull(dr["DocPropertiesID"]) <= 0)
                {
                    Lg.Error("AddUpdateDocTypesAndProperties", new Exception(Message), logParams);
                    return false;
                }
                DocPropertiesID = SqlDb.CheckLongDBNull(dr["DocPropertiesID"]);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("AddUpdateDocTypesAndProperties", ex, logParams);
                return false;
            }
        }
        public Boolean DeleteDocProperties(long ObjectID)
        {
            String logParams = "ObjectID:" + ObjectID + "|DocTypeID:" + DocTypeID;
            try
            {
                Hashtable prms = new Hashtable();
                if (ObjectID > 0) prms["ObjectID"] = ObjectID;
                if (DocTypeID > 0) prms["DocTypeID"] = DocTypeID;

                Db.SetSql("p_D_DocProperties", prms);
                Lg.Debug("DeleteDocProperties", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error deleting record";
                    Lg.Info("DeleteDocProperties", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                if (SqlDb.CheckLongDBNull(dr["DocTypeID"]) <= 0)
                {
                    Lg.Error("DeleteDocProperties", new Exception(Message), logParams);
                    return false;
                }
                DocTypeID = SqlDb.CheckLongDBNull(dr["DocTypeID"]);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("DeleteDocProperties", ex, logParams);
                return false;
            }
        }
        #endregion --- Public Methods ---
    }
}
