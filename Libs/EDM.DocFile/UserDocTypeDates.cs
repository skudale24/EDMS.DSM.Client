using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class UserDocTypeDates
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long UDTDId;
        public long UserId;
        public long DocTypeId;
        public int DocTypeParam = 3;
        public String DocTypeParamValue;
        public long ByUserId;
        public long HistoryKey = 0;
        public String ObjectType;
        public String HistoryData;
        public long ProgramId;
        #endregion

        #region --- Constructors ---
        public UserDocTypeDates() { }
        public UserDocTypeDates(String module) : this() { Module = module; }
        public UserDocTypeDates(String module, long userId) : this(module) { UserId = userId; }
        public UserDocTypeDates(String module, long userId, long docTypeId) : this(module, userId) { DocTypeId = docTypeId; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, UserId, DocTypeId, DocTypeParamValue, ByUserId are required.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "UserId:" + UserId + "|DocTypeId:" + DocTypeId + "|DocTypeParam:" + DocTypeParam
                + "|DocTypeParamValue:" + DocTypeParamValue + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                prms["UserID"] = UserId;
                prms["DocTypeID"] = DocTypeId;
                prms["DocTypeParam"] = DocTypeParam;
                prms["DocTypeParamValue"] = DocTypeParamValue;
                prms["ByUserID"] = ByUserId;
                prms[EDM.Setting.Fields.ObjectType] = ObjectType;
                prms[EDM.Setting.Fields.HistoryData] = HistoryData;
                prms[EDM.Setting.Fields.HistoryKey] = HistoryKey;
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_UserDocTypeDates", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile.UserDocTypeDates", "Save", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                UDTDId = MsSql.CheckIntDBNull(dr["UDTDID"]);
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                return UDTDId > 0 ? true : false;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.UserDocTypeDates", "Save", ex, logParams);
                return false; 
            }
        }
        #endregion
    }
}
