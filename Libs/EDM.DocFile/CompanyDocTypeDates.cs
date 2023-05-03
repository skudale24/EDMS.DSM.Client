using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.DocFile
{
    public class CompanyDocTypeDates
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long CDTDId;
        public long CompanyId;
        public long ProgramId;
        public long DocTypeId;
        public int DocTypeParam = 3;
        public String DocTypeParamValue;
        public long ByUserId;
        #endregion

        #region --- Constructors ---
        public CompanyDocTypeDates() { }
        public CompanyDocTypeDates(String module) : this() { Module = module; }
        public CompanyDocTypeDates(String module, long companyId) : this(module) { CompanyId = companyId; }
        public CompanyDocTypeDates(String module, long companyId, long docTypeId) : this(module, companyId) { DocTypeId = docTypeId; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, CompanyId, DocTypeId, DocTypeParamValue, ByUserId are required.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "CompanyId:" + CompanyId + "|DocTypeId:" + DocTypeId + "|DocTypeParam:" + DocTypeParam
                + "|DocTypeParamValue:" + DocTypeParamValue + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                prms["CompanyID"] = CompanyId;
                prms["DocTypeID"] = DocTypeId;
                prms["DocTypeParam"] = DocTypeParam;
                prms["DocTypeParamValue"] = DocTypeParamValue;
                prms["ByUserID"] = ByUserId;
                if(ProgramId > 0) prms["ProgramID"] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_CompanyDocTypeDates", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile.CompanyDocTypeDates", "Save", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                CDTDId = MsSql.CheckIntDBNull(dr["CDTDID"]);
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                return CDTDId > 0 ? true : false;
            }
            catch (Exception ex) 
            { 
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.CompanyDocTypeDates", "Save", ex, logParams);
                return false; 
            }
        }
        #endregion
    }
}
