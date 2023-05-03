using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocType
{
    [Serializable]
    public class Data
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long LpcId;
        public long ByUserId;

        private SqlDb Db;
        private Common.Log Lg;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.DocType.Data";
            }
        }

        public long DataId;
        public long ParentId;
        public long DocTypeId;
        public int Required = -1;
        public int Encrypt = -1;
        public String DocTypeParamValue;
        public int StatusId = 1;

        public virtual String AddSql { get { return String.Empty; } }
        public virtual String GetAllSql { get { return String.Empty; } }
        #endregion

        #region --- Constructors ---
        public Data()
        {
            ProgramId = Setting.Session.ProgramId;
            LpcId = EDM.Setting.Session.LpcId;
            ByUserId = Setting.Session.UserId;
            ConfigKey = String.Empty;
        }
        public Data(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public Data(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        #endregion --- Constructors ---

        #region --- Public Methods ---
        /// <summary>
        /// Module, DataId, ParentId, DocTypeId, Required, Encrypt are required. 
        /// If Required = 1, DocTypeParamValue is required, else optional. 
        /// If Encrypt = 1, DocTypeParamValue is encrypted using ParentId.
        /// ByUserId is fetched from Session, if unavailable, please specify.
        /// </summary>
        public Boolean Add()
        {
            String logParams = "ParentId:" + ParentId + "|DocTypeId:" + DocTypeId + "|Required:" + Required + "|Encrypt:" + Encrypt
                + "|DocTypeParamValue:" + DocTypeParamValue + "|ByUserId:" + ByUserId;
            try
            {
                if (ParentId <= 0) { Message = "ParentId is required."; return false; }
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return false; }
                if (Required < 0) { Message = "Required is required."; return false; }
                if (Encrypt < 0) { Message = "Encrypt is required."; return false; }
                if (Required == 1 && String.IsNullOrEmpty(DocTypeParamValue)) { Message = "DocTypeParamValue is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                /* Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */
                if (Encrypt == 1 && !String.IsNullOrEmpty(DocTypeParamValue))
                    DocTypeParamValue = EDM.Common.Helper.Encrypt(DocTypeParamValue, ParentId);
                /* end Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */

                Hashtable prms = new Hashtable();
                prms["ParentID"] = ParentId;
                prms["DocTypeID"] = DocTypeId;
                prms["DocTypeParamValue"] = DocTypeParamValue;
                prms["ByUserID"] = ByUserId;

                Db.SetSql(AddSql, prms);
                Lg.Debug("Add", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Lg.Info("Add", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DataId = MsSql.CheckLongDBNull(dr["DataID"]);
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                if (DataId <= 0)
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
        /// Module, ParentId, DataId, Required, Encrypt are required. 
        /// If Required = 1, DocTypeParamValue is required, else optional. 
        /// If Encrypt = 1, DocTypeParamValue is encrypted using ParentId.
        /// ByUserId is fetched from Session, if unavailable, please specify.
        /// StatusId may be specified.
        /// </summary>
        public Boolean Update()
        {
            String logParams = "ParentId:" + ParentId + "|DataId:" + DataId + "|DocTypeParamValue:" + DocTypeParamValue + "|StatusId:" + StatusId + "|ByUserId:" + ByUserId;
            try
            {
                if (ParentId <= 0) { Message = "ParentId is required."; return false; }
                if (DataId <= 0) { Message = "DataId is required."; return false; }
                if (Required < 0) { Message = "Required is required."; return false; }
                if (Encrypt < 0) { Message = "Encrypt is required."; return false; }
                if (Required == 1 && String.IsNullOrEmpty(DocTypeParamValue)) { Message = "DocTypeParamValue is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                /* Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */
                if (Encrypt == 1 && !String.IsNullOrEmpty(DocTypeParamValue))
                    DocTypeParamValue = EDM.Common.Helper.Encrypt(DocTypeParamValue, ParentId);
                /* end Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */

                Hashtable prms = new Hashtable();
                prms["DataID"] = DataId;
                prms["ParentID"] = ParentId;
                prms["DocTypeParamValue"] = DocTypeParamValue;
                if (StatusId > 0) prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql(AddSql, prms);
                Lg.Debug("Update", Db.SqlStmt);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error updating record.";
                    Lg.Info("Update", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                if (MsSql.CheckLongDBNull(dr["DataID"]) <= 0)
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
        /// Module is required. ProgramId is fetched from Session, if unavailable, please specify.
        /// LpcId may be specified (it is retrieved from Session).
        /// encryptMode = 0 (encrypted), 1 (plain-text), 2 - masked. For 1 & 2, ParentId is required.
        /// </summary>
        public DataSet GetAll(int encryptMode = 1)
        {
            String logParams = "ProgramId:" + ProgramId + "|LpcId:" + LpcId + "|ParentId:" + ParentId;
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (LpcId > 0) prms["LPCID"] = LpcId;
                if (ParentId > 0) prms["ParentID"] = ParentId;

                Db.SetSql(GetAllSql, prms);
                Lg.Debug("GetAll", Db.SqlStmt);
                DataSet ds = Db.ExecuteNoTransQuery();

                if (encryptMode == 0) return ds;
                if (ds != null && !MsSql.IsEmpty(ds) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String docTypeParamValue = SqlDb.CheckStringDBNull(dr["DocTypeParamValue"]);

                        /* Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */
                        if (SqlDb.CheckIntDBNull(dr["Encrypt"]) == 1 && !String.IsNullOrEmpty(docTypeParamValue))
                            docTypeParamValue = EDM.Common.Helper.Decrypt(docTypeParamValue, ParentId);
                        /* end Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */

                        if (encryptMode == 2 && !String.IsNullOrEmpty(docTypeParamValue))
                            docTypeParamValue = Common.Helper.LMask(docTypeParamValue, 2);

                        dr["DocTypeParamValue"] = docTypeParamValue;
                    }
                }
                return ds;
            }
            catch (Exception ex) { Message = ex.Message; Lg.Error("GetAll", ex); return null; }
        }
        #endregion --- Public Methods ---
    }
}
