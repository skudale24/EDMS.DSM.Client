using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Program
{
    public class Program
    {
        #region --- Properties ---
        public String Module;
        public String Message;

        private String _configKey = String.Empty;
        private SqlDb Db;
        private EDM.Common.Log Lg;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = _configKey + ":" + Module + ":EDM.Program.Program";
            }
        }

        public long ProgramId = 0;
        public String ProgramName = String.Empty;
        public String ProgramDesc = String.Empty;
        public long LpcId = 0;
        public String StartDate = String.Empty;
        public String EndDate = String.Empty;
        public String CallScript = String.Empty;
        public String Domain = String.Empty;
        public int StatusId;
        public long ByUserId = 0;
        public String ByUser;
        public String TranDate;
        public long CustomerSAID = 0;
        public long CompanyID = 0;
        public long ServiceAddressID = 0;
        #endregion

        #region --- Constructors ---
        public Program() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; ConfigKey = String.Empty; }
        public Program(long id) : this() { ProgramId = id; ConfigKey = String.Empty; }
        public Program(String configKey) : this() { ConfigKey = configKey; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// LpcId may be specified.
        /// </summary>
        public DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (LpcId > 0) prms["LPCID"] = LpcId;

                Db.SetSql("p_GET_Programs", prms);
                Lg.Debug("GetAll", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetAll", ex); return null; }
        }

        public DataSet GetDistinctPrograms()
        {
            try
            {
                Hashtable prms = new Hashtable();

                Db.SetSql("p_GET_AllPrograms", prms);
                Lg.Debug("p_GET_DistinctPrograms", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("p_GET_DistinctPrograms", ex); return null; }
        }
        public DataSet Get4Enrollment()
        {
            try
            {
                Hashtable prms = new Hashtable();

                Db.SetSql("p_GET_Programs4Enrollment", prms);
                Lg.Debug("Get4Enrollment", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Get4Enrollment", ex); return null; }
        }

        public DataSet GetAllPrograms4Role_4User(long roleID=0,long userID=0)
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (roleID > 0) prms["RoleID"] = roleID;
                if (userID > 0) prms["UserID"] = userID;

                Db.SetSql("p_GET_AllPrograms", prms);
                Lg.Debug("GetAllPrograms4Role_4User", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetAllPrograms4Role_4User", ex); return null; }
        }

        public DataSet GetSelectedPrograms()
        {
            try
            {
                Hashtable prms = new Hashtable();

                Db.SetSql("p_GET_SelectedPrograms", prms);
                Lg.Debug("GetSelectedPrograms", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetSelectedPrograms", ex); return null; }
        }

        public DataSet GetSelectedProgramsToAddCustomer()
        {
            try
            {
                Hashtable prms = new Hashtable();

                Db.SetSql("p_GET_SelectedProgramsToAddCustomer", prms);
                Lg.Debug("GetSelectedProgramsTOAddCustomer", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetSelectedProgramsTOAddCustomer", ex); return null; }
        }

        /// <summary>
        /// ProgramId, CallScript, ByUserId are required.
        /// </summary>
        public Boolean Update()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["CallScript"] = CallScript;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_AU_Program", prms);
                Lg.Debug("Update", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error saving record";
                    Lg.Info("Update", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                return SqlDb.CheckLongDBNull(dr["ProgramID"]) <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Update", ex); return false; }
        }

        /// <summary>
        /// Domain is required.
        /// </summary>
        public Boolean GetByDomain()
        {
            try
            {
                if (Domain.Length <= 0) { Message = "Domain is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["Url"] = Domain;

                Db.SetSql("p_GET_Program4Domain", prms);
                Lg.Debug("GetByDomain", Db.SqlStmt);
                DataSet ds = Db.ExecuteNoTransQuery();

                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error fetching details.";
                    Lg.Info("GetByDomain", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                ProgramId = SqlDb.CheckLongDBNull(dr["ProgramID"]);
                ProgramName = SqlDb.CheckStringDBNull(dr["ProgramName"]);

                return true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetByDomain", ex); return false; }
        }

        /// <summary>
        /// ProgramId is required.
        /// </summary>
        public Boolean GetById()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;

                Db.SetSql("p_GET_Program", prms);
                Lg.Info("GetById", Db.SqlStmt);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error fetching details.";
                    Lg.Info("GetById", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                ProgramName = SqlDb.CheckStringDBNull(dr["ProgramName"]);
                ProgramDesc = SqlDb.CheckStringDBNull(dr["ProgramDesc"]);
                CallScript = SqlDb.CheckStringDBNull(dr["CallScript"]);
                StartDate = SqlDb.CheckStringDBNull(dr["StartDate"]);
                EndDate = SqlDb.CheckStringDBNull(dr["EndDate"]);
                StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);
                ByUserId = SqlDb.CheckLongDBNull(dr["ByUserID"]);
                ByUser = SqlDb.CheckStringDBNull(dr["ByUser"]);
                TranDate = SqlDb.CheckStringDBNull(dr["TranDate"]);

                return true;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetById", ex); return false; }
        }


        /// <summary>
        /// CustomerSAID may be specified.
        /// </summary>
        public DataSet GetByCustomerSAID()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (CustomerSAID > 0) prms["CustomerSAID"] = CustomerSAID;            
                Db.SetSql("p_GET_ProgramsByCustomerSAID", prms);
                Lg.Debug("GetByCustomerSAID", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetByCustomerSAID", ex); return null; }
        }

        public DataSet Get4AllowedQCNAndCustomer()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (ServiceAddressID <= 0) { Message = "ServiceAddressID is required."; return null; }
                if (CompanyID <= 0) { Message = "CompanyID is required."; return null; }

                prms["ServiceAddressID"] = ServiceAddressID;
                prms["CompanyID"] = CompanyID;
                Db.SetSql("p_GET_Programs4AllowedQCNAndCustomer", prms);
                Lg.Debug("Get4AllowedQCNAndCustomer", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Get4AllowedQCNAndCustomer", ex); return null; }
        }
        public DataSet Get4AllowedHDAndCustomer()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (ServiceAddressID <= 0) { Message = "ServiceAddressID is required."; return null; }
                if (CompanyID <= 0) { Message = "CompanyID is required."; return null; }

                prms["ServiceAddressID"] = ServiceAddressID;
                prms["CompanyID"] = CompanyID;
                Db.SetSql("p_GET_Programs4AllowedHDAndCustomer", prms);
                Lg.Debug("Get4AllowedHDAndCustomer", Db.SqlStmt);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("Get4AllowedHDAndCustomer", ex); return null; }
        }
        #endregion
    }   

}
