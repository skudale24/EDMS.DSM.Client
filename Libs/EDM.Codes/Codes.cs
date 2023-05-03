using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using VTI.Common;

namespace EDM.Codes
{
    /// <summary>
    /// List of groupings of values used in the EDMS system. Maps to the Codes table.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc { }

    /// <summary>
    /// List of groupings of values used in the EDMS system. Maps to the Codes table.
    /// </summary>
    public class Codes
    {
        #region --- Properties ---
        /// <summary>
        /// AP, CP, WS, UT, etc.
        /// </summary>
        public String Module;
        /// <summary>
        /// Error message
        /// </summary>
        public String Message;
        /// <summary>
        /// Program identifier. 1 - eScore, 5 - KCPL, etc.
        /// </summary>
        public long ProgramId;
        /// <summary>
        /// LPC identifier, in case Codes need to be defined per LPC. This will override program-level codes.
        /// </summary>
        public long LpcId;

        private SqlDb Db;
        private Common.Log Lg;
        private String _configKey = String.Empty;
        /// <summary>
        /// Database Connection String Key specified in config file.
        /// </summary>
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Codes.Codes";
            }
        }

        /// <summary>
        /// Code identifier.
        /// </summary>
        public long CodeId;
        /// <summary>
        /// Code name, used to fetch grouping. All items within a group will have the same code name.
        /// </summary>
        public String CodeName;
        /// <summary>
        /// Generic description, only used for display purposes.
        /// </summary>
        public String CodeDesc;
        /// <summary>
        /// Value identifier.
        /// </summary>
        public int CodeValue = 0;
        /// <summary>
        /// Value's display string.
        /// </summary>
        public String CodeText;
        /// <summary>
        /// Value identifier, in case it is String and not int.
        /// </summary>
        public String CodeValueText = String.Empty;
        /// <summary>
        /// Sequence in which values are to be displayed.
        /// </summary>
        public int CodeSeq = 1;
        /// <summary>
        /// Used to group distinct modules in the system, will be used in the implementation configurator UI when it is implemented properly.
        /// </summary>
        public String GroupName = String.Empty;
        /// <summary>
        /// Record status. 1 - Active, 2 - Archived.
        /// </summary>
        public int StatusId;
        /// <summary>
        /// User adding or modifying a record.
        /// </summary>
        public long ByUserId;
        /// <summary>
        /// Different ID for  particular measure
        /// </summary>
        public long MeasureID;


        #endregion

        #region --- Constructors ---
        /// <summary>
        /// Default empty constructor. Fetch and store ProgramId and ByUserId from Session to be used by methods in same context.
        /// </summary>
        public Codes() { ProgramId = Setting.Session.ProgramId; ByUserId = Setting.Session.UserId; LpcId = EDM.Setting.Session.LpcId; ConfigKey = String.Empty; }
        /// <summary>
        /// Calls empty constructor above. Store Module for logging purposes.
        /// </summary>
        /// <param name="module">AP, CP, UT, WS, etc.</param>
        public Codes(String module) : this() { Module = module; ConfigKey = String.Empty; }
        /// <summary>
        /// Constructor with config db connection string key and ProgramId.
        /// </summary>
        public Codes(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        /// <summary>
        /// Call module constructor above. Store CodeId for get or update functionality.
        /// </summary>
        /// <param name="module">AP, CP, UT, WS, etc.</param>
        /// <param name="id">Primary key.</param>
        public Codes(String module, long id) : this(module) { CodeId = id; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Add a new code to the database.
        ///     ProgramId and ByUserId are required and fetched from Session in the constructor. If not available, please specify.
        ///     Module is required for logging purposes.
        ///     CodeName is required to uniquely identify the grouping.
        ///     CodeText is required for the text to be displayed in this group.
        ///     
        ///     CodeValue is optional and if not specified, it defaults to the next running number in this group 
        ///         identified by the CodeName.
        ///     CodeSeq is optional and defaults to 1.
        ///     LpcId is optional and may be speficied if this group is to be stored at the LPC level and not Program level. Make
        ///         sure the same group exists at program level before doing this because otherwise other LPCs will not be able to 
        ///         retrieve this group.
        ///     CodeDesc is optional and is a generic description.
        ///     CodeValueText is optional and may be specified in case a non-numeric CodeValue is required.
        ///     GroupName is optional and will be used by the Configurator tool for set-up purposes.
        ///     StatusId defaults to 0.
        /// </summary>
        /// <returns>Boolean. true in case of success. false in case of error or exception. Message will contain error.</returns>
        public Boolean Add()
        {
            String logParams = "ProgramID:" + ProgramId + "|LPCID:" + LpcId + "|CodeName:" + CodeName + "|CodeDesc:" + CodeDesc
                + "|CodeValue:" + CodeValue + "|CodeText:" + CodeText + "|CodeValueText:" + CodeValueText + "|CodeSeq:" + CodeSeq
                + "|GroupName:" + GroupName + "|StatusID:" + StatusId + "|ByUserID:" + ByUserId;

            try
            {
                if (String.IsNullOrEmpty(CodeName)) { Message = "CodeName is required."; return false; }
                if (String.IsNullOrEmpty(CodeText)) { Message = "CodeText is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["CodeName"] = CodeName;
                prms["CodeDesc"] = CodeDesc;
                prms["CodeValue"] = CodeValue;
                prms["CodeText"] = CodeText;
                prms["CodeValueText"] = CodeValueText;
                prms["CodeSeq"] = CodeSeq;
                prms["GroupName"] = GroupName;
                prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_AU_Code", prms);
                Lg.Info("Add", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error saving record.";
                    Lg.Info("Add", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                CodeId = SqlDb.CheckLongDBNull(dr["CodeID"]);
                if (CodeValue <= 0) CodeValue = SqlDb.CheckIntDBNull(dr["CodeValue"]);
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return CodeId <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Add", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Update existing code in database.
        ///     ProgramId and ByUserId are required and fetched from Session. If not available please specify.
        ///     Module is required for logging purposes.
        ///     CodeId is required to uniquely identify record to be updated.
        ///     
        ///     CodeDesc is optional but if not specified will default to empty string.
        ///     LPCID, CodeName, CodeValue, CodeText, CodeSeq, StatusId are optional and will be updated only 
        ///         if specified.
        /// </summary>
        /// <returns>Boolean. true in case of success. false in case of error or exception. Message will contain error.</returns>
        public Boolean Update()
        {
            String logParams = "ProgramID:" + ProgramId + "|LPCID:" + LpcId + "|CodeName:" + CodeName + "|CodeDesc:" + CodeDesc
                + "|CodeValue:" + CodeValue + "|CodeText:" + CodeText + "|CodeValueText:" + CodeValueText + "|CodeSeq:" + CodeSeq
                + "|GroupName:" + GroupName + "|StatusID:" + StatusId + "|ByUserID:" + ByUserId;

            try
            {
                if (CodeId <= 0) { Message = "CodeId is required"; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["CodeID"] = CodeId;
                prms["CodeDesc"] = CodeDesc;
                prms["CodeValueText"] = CodeValueText;
                prms["GroupName"] = GroupName;
                prms["ByUserID"] = ByUserId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                if (!String.IsNullOrEmpty(CodeName)) prms["CodeName"] = CodeName;
                if (CodeValue > 0) prms["CodeValue"] = CodeValue;
                if (!String.IsNullOrEmpty(CodeText)) prms["CodeText"] = CodeText;
                if (CodeSeq > 0) prms["CodeSeq"] = CodeSeq;
                if (StatusId > 0) prms["StatusID"] = StatusId;

                Db.SetSql("p_AU_Code", prms);
                Lg.Info("Update", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error saving record.";
                    Lg.Info("Update", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return SqlDb.CheckLongDBNull(dr["CodeID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Update", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Module, CodeId are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Delete()
        {
            String logParams = "CodeID:" + CodeId + "|ByUserID:" + ByUserId;

            try
            {
                if (CodeId <= 0) { Message = "CodeId is required"; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required"; return false; }

                Hashtable prms = new Hashtable();
                prms["CodeID"] = CodeId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_D_Code", prms);
                Lg.Info("Delete", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error deleting record.";
                    Lg.Info("Delete", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);

                return SqlDb.CheckLongDBNull(dr["CodeID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("Delete", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Fetch all codes from database.
        ///     Module is required for logging purposes.
        ///     ProgramId is fetched from Session, if unavailable, please specify.
        ///     GroupName, LpcId may be specified for filtering records.
        /// </summary>
        /// <returns>DataSet. All/filtered codes from database in case of success. null in case of error/exception. Message contains error.
        ///     Records contain ProgramID, ProgramName, LPCID, LPCName, CodeID, CodeName, CodeValue, CodeText, CodeValueText, 
        ///         CodeSeq, GroupName, CodeDesc (max 50 chars), GroupName, TranDateRaw, TranDate (formatted), ByUserID, ByUser, 
        ///         StatusID, StatusName columns.
        /// </returns>
        public DataSet GetAll()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (String.IsNullOrEmpty(GroupName)) prms["GroupName"] = GroupName;

                Db.SetSql("p_GET_Codes", prms);
                Lg.Info("GetAll", Db.SqlStmt);

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
        /// Fetch details for specific record identified by CodeId.
        ///     Module is required for logging purposes.
        ///     CodeId is required to uniquely identify the record.
        /// </summary>
        /// <returns>Boolean. true in case of success. false in case of error or exception. Message will contain error.
        ///     LpcId, CodeName, CodeDesc, CodeValue, CodeText, CodeValueText, CodeSeq, GroupName, StatusId, ByUserId
        ///         will populated.
        /// </returns>
        public Boolean GetById()
        {
            String logParams = "CodeId:" + CodeId;
            try
            {
                if (CodeId <= 0) { Message = "CodeId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["CodeID"] = CodeId;

                Db.SetSql("p_GET_Code", prms);
                Lg.Info("GetById", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Lg.Info("GetById", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                LpcId = SqlDb.CheckLongDBNull(dr["LPCID"]);
                CodeName = SqlDb.CheckStringDBNull(dr["CodeName"]);
                CodeDesc = SqlDb.CheckStringDBNull(dr["CodeDesc"]);
                CodeValue = SqlDb.CheckIntDBNull(dr["CodeValue"]);
                CodeText = SqlDb.CheckStringDBNull(dr["CodeText"]);
                CodeValueText = SqlDb.CheckStringDBNull(dr["CodeValueText"]);
                CodeSeq = SqlDb.CheckIntDBNull(dr["CodeSeq"]);
                GroupName = SqlDb.CheckStringDBNull(dr["GroupName"]);
                StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);
                ByUserId = SqlDb.CheckLongDBNull(dr["ByUserID"]);

                return true;
            }
            catch (Exception ex)
            {
                Lg.Error("GetById", ex, logParams);
                return false;
            }
        }

        /// <summary>
        /// Fetch set of records grouped by CodeName for StatusID = 1 (Active).
        ///     Module is required for logging purposes.
        ///     ProgramId is required to uniquely identify the program.
        ///     CodeName is required to uniquely identify the group.
        ///     
        ///     LpcId is optional and may be specified if the group has been over-ridden for a specific Lpc.
        ///         If not available, records will be fetched for program level.
        /// </summary>
        /// <returns>DataSet. Set of records identified by ProgramId, CodeName, [LpcId] in case of success.
        /// null in case of error/exception. Message will contain error.
        ///     Records contain ProgramID, LPCID, CodeID (CodeValue), CodeName, CodeValue (CodeText), 
        ///     CodeValueText, CodeSeq, GroupName, StatusID, TranDate, ByUserID columns.
        /// </returns>
        public DataSet GetByName()
        {
            String logParams = "ProgramID:" + ProgramId + "|LPCID:" + LpcId + "|CodeName:" + CodeName;
            try
            {
                if (String.IsNullOrEmpty(CodeName)) { Message = "CodeName is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["CodeName"] = CodeName;

                Db.SetSql("p_GET_CodesByName", prms);
                Lg.Info("GetByName", Db.SqlStmt);

                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Lg.Error("GetByName", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Fetch set of records grouped by CodeName for StatusID = 1 (Active).
        ///     Module is required for logging purposes.
        ///     ProgramId is required to uniquely identify the program.
        ///     CodeName is required to uniquely identify the group.
        ///     
        ///     LpcId is optional and may be specified if the group has been over-ridden for a specific Lpc.
        ///         If not available, records will be fetched for program level.
        /// </summary>
        /// <returns>DataSet. Set of records identified by ProgramId, CodeName, [LpcId] in case of success.
        /// null in case of error/exception. Message will contain error.
        ///     Records contain ProgramID, LPCID, CodeID (CodeValue), CodeName, CodeValue (CodeText), 
        ///     CodeValueText, CodeSeq, GroupName, StatusID, TranDate, ByUserID columns.
        /// </returns>
        /*public DataSet GetCodesByName()
        {
            String logParams = "|CodeName:" + CodeName;
            try
            {
                if (String.IsNullOrEmpty(CodeName)) { Message = "CodeName is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = (long)EDM.Common.Programs.GPP;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["CodeName"] = CodeName;

                Db.SetSql("p_GET_CodesByName", prms);
                Lg.Info("GetCodesByName", Db.SqlStmt);

                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Lg.Error("GetCodesByName", ex, logParams);
                return null;
            }
        }*/
                              
        /// <summary>
        /// Module, CodeName, CodeText is required. ProgramId is fetched from Session, if not available, please specify. 
        /// LpcId may be specified for LPC-specific settings.
        /// </summary>
        public DataSet GetBySubTypeText()
        {
            String logParams = "ProgramID:" + ProgramId + "|LPCID:" + LpcId + "|CodeName:" + CodeName + "|CodeText:" + CodeText;
            try
            {
                if (String.IsNullOrEmpty(CodeName)) { Message = "CodeName is required."; return null; }
                if (String.IsNullOrEmpty(CodeText)) { Message = "CodeText is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["CodeName"] = CodeName;
                prms["CodeText"] = CodeText;

                Db.SetSql("p_GET_CodesBySubTypeText", prms);
                Lg.Info("GetBySubTypeText", Db.SqlStmt);

                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Lg.Error("GetBySubTypeText", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// Same as GetByName but fetch CodeValueText instead of CodeValue.
        ///     Module is required for logging purposes.
        ///     CodeName is required to uniquely identify group of records to be fetched.
        ///     
        ///     LpcId is optional and may be specified if group is overridden for LPC.
        /// </summary>
        /// <returns>DataSet. Set of records identified by ProgramId, CodeName, [LpcId] in case of success.
        /// null in case of error/exception. Message will contain error.
        ///     Records contain ProgramID, LPCID, CodeID (CodeValueText), CodeName, CodeValue (CodeText), CodeValueText, 
        ///         CodeSeq, GroupName, StatusID, TranDate, ByUserID, CodeDesc columns.</returns>
        public DataSet GetByValueText()
        {
            String logParams = "ProgramID:" + ProgramId + "|LPCID:" + LpcId + "|CodeName:" + CodeName;
            try
            {
                if (String.IsNullOrEmpty(CodeName)) { Message = "CodeName is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                prms["CodeName"] = CodeName;

                Db.SetSql("p_GET_CodesByValueText", prms);
                Lg.Info("GetByName", Db.SqlStmt);

                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetByValueText", ex, logParams);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String GetCodesByText(String Recommneds)
        {
            String logParams = "Recommneds:" + Recommneds;
            String Recommend = String.Empty;
            string tag1 = "•";
            try
            {
                Hashtable prms = new Hashtable();
                prms["CodeText"] = Recommneds;

                Db.SetSql("p_GET_CodesByText", prms);
                Lg.Info("GetCodesByText", Db.SqlStmt);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) return Recommend;

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (string.IsNullOrEmpty(Recommend))
                        Recommend = " " + tag1 + " " + item["CodeDesc"].ToString();
                    else
                        Recommend = Recommend + "\r\n" + " " + tag1 + " " + item["CodeDesc"].ToString();
                }

                return Recommend;
            }
            catch (Exception ex)
            {
                Lg.Error("GetCodesByText", ex, logParams);
                return Recommend;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String GetCodes4HNS(String Recommneds)
        {
            String logParams = "Recommneds:" + Recommneds;
            String Recommend = String.Empty;
            try
            {
                Hashtable prms = new Hashtable();
                prms["CodeText"] = Recommneds;

                Db.SetSql("p_GET_CodesByText", prms);
                Lg.Info("GetCodes4HNS", Db.SqlStmt);

                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) return Recommend;

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Recommend = item["CodeDesc"].ToString();
                }

                return Recommend;
            }
            catch (Exception ex)
            {
                Lg.Error("GetCodes4HNS", ex, logParams);
                return Recommend;
            }
        }

        /// <summary>
        /// To Get ProjectTypes on Customer snapshot
        /// </summary>
        /// <returns></returns>
        public DataSet GetProjectTypes()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;

                Db.SetSql("p_GET_ProjectTypes", prms);
                Lg.Info("GetProjectTypes", Db.SqlStmt);

                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Lg.Error("GetProjectTypes", ex);
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// CodeName used to fetch sets of Codes.
    /// </summary>
    public class Name
    {
        //--- General ---
        /// <summary>
        /// StatusType
        /// </summary>
        public static String StatusType = "StatusType";
        /// <summary>
        /// CompanyStatus
        /// </summary>
        public static String CompanyStatus = "CompanyStatus";
        /// <summary>
        /// LimitType
        /// </summary>
        public static String LimitType = "LimitType";
        /// <summary>
        /// PasswordChangeReason
        /// </summary>
        public static String PasswordChangeReason = "PasswordChangeReason";
        /// <summary>
        /// AccountType
        /// </summary>
        public static String AccountType = "AccountType";
        /// <summary>
        /// VacationType
        /// </summary>
        public static String VacationType = "VacationType";
        /// <summary>
        /// DocumentStatus
        /// </summary>
        public static String DocumentStatus = "DocumentStatus";
        /// <summary>
        /// SubstitutionTag
        /// </summary>
        public static String SubstitutionTag = "SubstitutionTag";
        /// <summary>
        /// StoreVisitType
        /// </summary>
        public static String StoreVisitType = "StoreVisitType";
        /// <summary>
        /// DoNotContact
        /// </summary>
        public static String DoNotContact = "DoNotContact";
        /// <summary>
        /// ProjectType
        /// </summary>
        public static String ProjectType = "ProjectType";
        //--- CallLogs ---
        /// <summary>
        /// CallType
        /// </summary>
        public static String CallType = "CallType";
        /// <summary>
        /// CallDisposition
        /// </summary>
        public static String CallDisposition = "CallDisposition";
        //--- ServiceAddressGI ---
        /// <summary>
        /// HouseType
        /// </summary>
        public static String HouseType = "HouseType";
        /// <summary>
        /// Foundation
        /// </summary>
        public static String Foundation = "Foundation";
        public static String HUPFoundation = "HUPFoundation";
        /// <summary>
        /// Orientation
        /// </summary>
        public static String Orientation = "Orientation";
        /// <summary>
        /// FootLadder
        /// </summary>
        public static String FootLadder = "FootLadder";
        /// <summary>
        /// PrimaryHeating
        /// </summary>
        public static String PrimaryHeating = "PrimaryHeating";
        /// <summary>
        /// PrimaryHeatingSubType
        /// </summary>
        public static String PrimaryHeatingSubType = "PrimaryHeatingSubType";
        /// <summary>
        /// PrimaryWaterHeating
        /// </summary>
        public static String PrimaryWaterHeating = "PrimaryWaterHeating";
        /// <summary>
        /// ProgrammableThermostat
        /// </summary>
        public static String ProgrammableThermostat = "ProgrammableThermostat";
        /// <summary>
        /// BuySellingStatus
        /// </summary>
        public static String BuySellingStatus = "BuySellingStatus";
        /// <summary>
        /// HearAboutProgram 
        /// </summary>
        public static String HearAboutProgram = "HearAboutProgram";
        /// <summary>
        /// HearAboutProgram 
        /// </summary>
        public static String Language = "Language";
        /// <summary>
        /// Code
        /// </summary>
        public static String Code = "Code";
        /// <summary>
        /// StructureType 
        /// </summary>
        public static String StructureType = "StructureType";
        /* Mar 15, 2016 | Nibha Kothari | ES-879: New HOAddressGI Page */
        /// <summary>
        /// HouseOwn
        /// </summary>
        public static String HouseOwn = "HouseOwn";
        /// <summary>
        /// HouseRental 
        /// </summary>
        public static String HouseRental = "HouseRental";
        /// <summary>
        /// HaveInternet 
        /// </summary>
        public static String HaveInternet = "HaveInternet";
        /// <summary>
        /// WiFiInternet
        /// </summary>
        public static String WiFiInternet = "WiFiInternet";
        /// <summary>
        /// HVACZoned
        /// </summary>
        public static String HVACZoned = "HVACZoned";
        /// <summary>
        /// SecondaryHeating
        /// </summary>
        public static String SecondaryHeating = "SecondaryHeating";
        /// <summary>
        /// EVPV
        /// </summary>
        public static String EVPV = "EVPV";
        /* END Mar 15, 2016 | Nibha Kothari | ES-879: New HOAddressGI Page */
        //--- end ServiceAddressGI ---
        /* Apr 25, 2016| Nibha Kothari | ES-1214: Implement Rebate Status Reasons from DB */
        /// <summary>
        /// PIReason
        /// </summary>
        public static String PIReason = "PIReason";
        /* end Apr 25, 2016| Nibha Kothari | ES-1214: Implement Rebate Status Reasons from DB */
        //--- Payout ---
        /// <summary>
        /// PayoutCustomer 
        /// </summary>
        public static String PayoutCustomer = "PayoutCustomer";
        /* Apr 29, 2016 | Nibha Kothari | ES-1215: Implement KCPL Payouts */
        /// <summary>
        /// PayoutType 
        /// </summary>
        public static String PayoutType = "PayoutType";
        /* Apr 29, 2016 | Nibha Kothari | ES-1215: Implement KCPL Payouts */
        /* July 13, 2016 | Pratik Chothe | ES-1742: Create Inventory-- Product sub-menu page */
        /// <summary>
        /// Manufracture 
        /// </summary>
        public static String Manufracture = "Manufacturer";
        /// <summary>
        /// Category 
        /// </summary>
        public static String Category = "Category";
        /* July 13, 2016 |Pratik Chothe | ES-1742: Create Inventory-- Product sub-menu page */
        /// <summary>
        /// CAdvisorSurvey 
        /// </summary>
        public static String CAdvisorSurvey = "CAdvisorSurvey";
        /// <summary>
        /// CQCNSurvey 
        /// </summary>
        public static String CQCNSurvey = "CQCNSurvey";
        /// <summary>
        /// WinSvcSuffix
        /// </summary>
        public const String WinSvcSuffix = "WinSvcSuffix";
        /// <summary>
        /// ApplicantRelationship 
        /// </summary>
        public const String ApplicantRelationship = "ApplicantRelationship ";
        /// <summary>
        /// USCitizen 
        /// </summary>
        public const String USCitizen = "USCitizen";
        /// <summary>
        /// LandlordConsent 
        /// </summary>
        public const String LandlordConsent = "LandlordConsent";
        /// <summary>
        /// PriorWAP 
        /// </summary>
        public const String PriorWAP = "PriorWAP";
        /// <summary>
        /// CSAGIOwnIt 
        /// </summary>
        public const String CSAGIOwnIt = "CSAGIOwnIt";
        /// <summary>
        /// HomeType 
        /// </summary>
        public const String CSAGIHouseType = "CSAGIHouseType";
        /// <summary>
        /// Rental property
        /// </summary>
        public const String CSAGIRentalProperty = "CSAGIRentalProperty";
        /// <summary>
        /// Family Type
        /// </summary>
        public const String CSAGIFamilyType = "CSAGIFamilyType";
        /// <summary>
        /// Building Exterior
        /// </summary>
        public const String CSAGIBuildingExterior = "CSAGIBuildingExterior";
        /// <summary>
        /// oundation
        /// </summary>
        public const String CSAGIFoundation = "CSAGIFoundation";
        /// <summary>
        /// Roof Condition
        /// </summary>
        public const String CSAGIRoofCondition = "CSAGIRoofCondition";
        /// <summary>
        ///Primary Heating
        /// </summary>
        public const String CSAGIPrimaryHeating = "CSAGIPrimaryHeating";
        /// <summary>
        /// Mold Or Moisture
        /// </summary>
        public const String CSAGIMoldOrMoisture = "CSAGIMoldOrMoisture";
        /// <summary>
        /// Utility Account Verification
        /// </summary>
        public const String UtilityAccVerification = "UtilityAccVerification";
        /// <summary>
        /// Bill Type -- kW / kWh
        /// </summary>
        public const String BillUnit = "BillUnit";
        /// <summary>
        /// SmallBusinessType for Contractor
        /// </summary>
        public const String SmallBusinessType = "SmallBusinessType";
        public const String Actions = "Actions";
        /// <summary>
        /// CreditCardProcessGateway for Evaluation Payment
        /// </summary>
        public const String CreditCardProcessGateway = "CCProcessGateway";
        public const String CompanyRecordType = "CompanyRecordType";
        public const String BadgeType = "BadgeType";

        public const String BadgeStatus = "BadgeStatus";

        public const String IsFinanced = "Financed";

        public const String FinancedBy = "FinancedBy";
        public const String FundingStatus = "FundingStatus";

        public const String MaritalStatus = "MaritalStatus";
        public const String Gender = "Gender";
        public const String Race = "Race";
        public const String VeteranStatus = "VeteranStatus";
        /// <summary>
        /// CRRProjectType -- Capacity Decrease, Capacity Increase..
        /// </summary>
        public const string CRRProjectType = "CRRProjectType";

        /// <summary>
        /// BillingOption -- Distributor Billing Option, TVA-Vendor Billing Option
        /// </summary>
        public const string BillingOption = "BillingOption";

        /// <summary>
        /// ParticipantSubType -- School, University, Other
        /// </summary>
        public const string ParticipantSubType = "ParticipantSubType";

        /// <summary>
        /// ParticipantRate -- Residential/GSA1 Commercial, Other Commercial, Residential/GSA1 Commercial (>10 kW Systems)
        /// </summary>
        public const string ParticipantRate = "ParticipantRate";

        /// <summary>
        /// District -- Southeast, Kentucky..
        /// </summary>
        public const string District = "Region";

        /// <summary>
        /// PartTypes -- Res,Com
        /// </summary>
        public const string ParticipantType = "PartTypes";

        /// <summary>
        /// EnergyType -- Solar, Biomass...
        /// </summary>
        public const string EnergyType = "EnergyType";

        /// <summary>
        /// SubEnergy -- Sm Solar, lg Solar...
        /// </summary>
        public const string SubEnergy = "SubEnergy";

        /// <summary>
        /// SubEnergy -- exp, GPP, old...
        /// </summary>
        public const string InitCon = "InitCon";

        /// <summary>
        /// SubEnergy -- exp, GPP, old...
        /// </summary>
        public const string CurCon = "CurCon";

        /// <summary>
        /// EnergyType -- Canceled, Executed...
        /// </summary>
        public const string ContractStatus = "ContractStatus";

        /// <summary>
        /// EnergyType -- Non Interval, Interval...
        /// </summary>
        public const string MeterType = "MeterType";

        /// <summary>
        /// DocumentType -- Select type Of Doc...
        /// </summary>
        public const string DocumentType = "DocumentType";

        /// <summary>
        /// EnergyType -- Non Interval, Interval...
        /// </summary>
        public const string MeteringConnection = "MeteringConnection";

        /// <summary>
        /// Rate -- Retail
        /// </summary>
        public const string Rate = "Rate";
        public const string FemaleHeadedHousehold = "FemaleHeadedHousehold";
        public const string HispanicOrigin = "HispanicOrigin";
        public const string FPL = "FPL";

        public const string SourceOfFund = "SourceOfFund";
        public const string OnBoardNonProfit= "OnBoardNonProfit";
        /* April 14, 2023 |Roshni Suvagiya | DMI-82: Application - changing status not eligiblity and incomplete */
        public const string NoticeOfIneligibility = "NoticeOfIneligibility";
        public const string IncompleteApplication = "IncompleteApplication";
        public const string ContributionFormRequest = "ContributionFormRequest";
        /* April 14, 2023 |Roshni Suvagiya | DMI-82: Application - changing status not eligiblity and incomplete */
        /// <summary>
        /// AppointmentType -- Virtual, In Person
        /// </summary>
        public const string AppointmentType = "AppointmentType";
        public const string SchedulingType = "SchedulingType";
        public const string ProjectSchedulingType = "ProjectSchedulingType";
        public const string Duration = "Duration";

        public static String HUPCQCNSurvey = "HUPCQCNSurvey";
        public static String HUPCHealthSurvey = "HUPCHealthSurvey";
        public static String HUPHealthSurveyOptionForQus = "HUPHealthSurveyOptionForQus";

        /// <summary>
        /// HUPAdvisorSurvey 
        /// </summary>
        public static String HUPAdvisorSurvey = "HUPAdvisorSurvey";
        public static String AccessType = "AccessType";
        /// <summary>
        /// LPCAccess yes or no 
        /// </summary>
        public static String LPCAccess = "LPCAccess";

        /// <summary>
        /// GCAdvisorSurvey 
        /// </summary>
        public static String GCAdvisorSurvey = "GCAdvisorSurvey";
        public static String GCQCNSurvey = "GCQCNSurvey";

        /// <summary>
        /// DPPContract 
        /// </summary>
        public static String LPCIACompleted = "LPCIACompleted";
        public static String FacilityType = "FacilityType";
        public static String GenerationType = "GenerationType";

        /// <summary>
        /// CustomerAddressType -- rresidential, Business
        /// </summary>
        public const string CustomerAddressType = "CustomerAddressType";

        public static String SubMeasureType = "SubMeasureType";
        public static String PrimaryHeatingTypeSA = "PrimaryHeatingTypeSA";
        public static String HouseTypeSA = "HouseTypeSA";
        public static String SqFootageSA = "SqFootageSA";

        public static String ActionRecommended = "ActionRecommended";

        public static String ProgramDelivery = "ProgramDelivery";
        public static String DeliveryModel = "DeliveryModel";
        public static String ApplicationStatus = "ApplicationStatus";
        public static String RecruitmentStrategy = "RecruitmentStrategy";
        public static String ContributionType = "ContributionType";
        public static String ApplicationContact = "ApplicationContact";
        public static String AMI = "AMI";
        public static String QualificationPreference = "QualificationPreference";
        public static String DespositionLeads = "DespositionLeads";
    }

    /// <summary>
    /// Some specific CodeValue/CodeTexts required in the system for conditions.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Global StatusID values: 1 - Active, 2 - Disabled, 3 - Archived (not being used currently).
        /// </summary>
        public enum Status : long
        {
            /// <summary>
            /// 1 - Active
            /// </summary>
            Active = 1,
            /// <summary>
            /// 2 - Disabled
            /// </summary>
            Disabled = 2,
            /// <summary>
            /// 3 - Archived (not used currently)
            /// </summary>
            Archived = 3
        }

        public enum HistoryType : int
        {
            /// <summary>
            /// 1 - Status change
            /// </summary>
            Statuschange = 1,
            /// <summary>
            /// 2 - Field change
            /// </summary>
            Fieldchange = 2,
            /// <summary>
            /// 3 - Doc change
            /// </summary>
            Docchange = 3,
            /// <summary>
            /// 4 - Measure change
            /// </summary>
            Measurechange = 4,
            /// <summary>
            /// 5 - Action
            /// </summary>
            Action = 5
        }

        public enum HealthNSafety : int
        {
            /// <summary>
            /// 1 - Flue Clearance
            /// </summary>
            FlueClearance = 1,
            /// <summary>
            /// 2 - Flue Disconnect
            /// </summary>
            FlueDisconnect = 2,
            /// <summary>
            /// 3 - Bulk Moisture
            /// </summary>
            BulkMoisture = 3,
            /// <summary>
            /// 4 - Structural
            /// </summary>
            Structural = 4,
            /// <summary>
            /// 5 - Vapor Barrier
            /// </summary>
            VaporBarrier = 5,
                /// <summary>
                /// 6 - CO Monitor
                /// </summary>
            COMonitor = 6
        }

        /// <summary>
        /// Fetch enum description for a particular enum value string. If not available return the enum value string itself.
        /// </summary>
        /// <param name="value">Enum value string to be looked-up.</param>
        /// <returns>String. Enum description or value if description not available.</returns>
        public static String GetEnumDesc(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return attrs[0].Description;
            }
            catch { }
            return value.ToString();
        }
    }
}