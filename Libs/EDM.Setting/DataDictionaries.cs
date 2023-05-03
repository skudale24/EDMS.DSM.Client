using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    public class DataDictionaries
    {
        public class ScopeType
        {
            public const String Grid = "Grid";
            public const String Page = "Page";
        }
        public class Key
        {
            public const String FinancingSetting = "FinancingSetting";
            public const String HouseType = "HouseType";
            /* Dec 07, 2017 | Nibha Kothari | ES-4012: (SUP-805) KCPL: Customer Enrollment: External Utility File: Rate Code to LPC Mapping */
            public const String AddNewCustomer = "AddNewCustomer";
        }

        //ES-944
        #region --- Properties ---
        public String Message;

        public String Module;
        public long ProgramId = 0;
        public long DDId;
        public long LpcId;
        public String TitleKey;
        public String TitleValue;
        public int TitleDisplay;
        public int ExportExcelDisplay;
        public int ImportExcelDisplay;
        public String Extra;
        public String Scope;
        #endregion

        #region --- Constructors ---
        public DataDictionaries() { ProgramId = Session.ProgramId; }
        /* Aug 09, 2017 | Nibha Kothari | ES-3629: Admin: Contractor : Configure Financing Section */
            public DataDictionaries(String module):this() { Module = module; }
        public DataDictionaries(String module, long ddId) : this(module) { DDId = ddId; }
        /* Aug 09, 2017 | Nibha Kothari | ES-3629: Admin: Contractor : Configure Financing Section */
        #endregion

        #region --- Public Methods ---
        public DataSet GetAll()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DataDictionaries", prms, out SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch { return null; }
        }

        /// <summary>
        /// Module, TitleKey are required. ProgramId is fetched from Session, if unavailable, please specify.
        /// Aug 09, 2017 | Nibha Kothari | ES-3629: Admin: Contractor : Configure Financing Section
        /// </summary>
        public Boolean GetByKey()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (String.IsNullOrEmpty(TitleKey)) { Message = "TitleKey is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["TitleKey"] = TitleKey;
                if (!String.IsNullOrEmpty(Module)) prms["Module"] = Module;
                if (!String.IsNullOrEmpty(Scope)) prms["Scope"] = Scope;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DataDictionary", prms, out SqlforLog);
                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Record not found.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DDId = MsSql.CheckLongDBNull(dr["DDID"]);
                LpcId = MsSql.CheckLongDBNull(dr["LPCID"]);
                TitleKey = MsSql.CheckStringDBNull(dr["TitleKey"]);
                TitleValue = MsSql.CheckStringDBNull(dr["TitleValue"]);
                TitleDisplay = MsSql.CheckIntDBNull(dr["TitleDisplay"]);
                ExportExcelDisplay = MsSql.CheckIntDBNull(dr["ExportExcelDisplay"]);
                ImportExcelDisplay = MsSql.CheckIntDBNull(dr["ImportExcelDisplay"]);
                Module = MsSql.CheckStringDBNull(dr["Module"]);
                Extra = MsSql.CheckStringDBNull(dr["Extra"]);
                Scope = MsSql.CheckStringDBNull(dr["Scope"]);

                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}
