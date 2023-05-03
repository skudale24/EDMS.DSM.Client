using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using VTI.Common;
using System.Collections;

namespace EDM.DocFile
{
    public class ApplicationDocTypeDates
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ADTDId;
        public long ObjectId;
        public long DocTypeId;
        public long ObjectType;
        public long ProgramId;
        public int DocTypeParam = 3;
        public String DocExpiryDate;
        public long ByUserId;
        #endregion

        #region --- Constructors ---
        public ApplicationDocTypeDates() { }
        public ApplicationDocTypeDates(String module) : this() { Module = module; }
        public ApplicationDocTypeDates(String module, long ObjectId) : this(module) { ObjectId = ObjectId; }
        public ApplicationDocTypeDates(String module, long ObjectId, long docTypeId) : this(module, ObjectId) { DocTypeId = docTypeId; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, ApplicationId, DocTypeId, DocExpiryDate, ByUserId are required.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "ObjectId:" + ObjectId + "|DocTypeId:" + DocTypeId + "|DocTypeParam:" + DocTypeParam
                + "|DocExpiryDate:" + DocExpiryDate + "|ProgramId" + ProgramId + "|ObjectType" + ObjectType + "|ByUserId:" + ByUserId;

            try
            {
                Hashtable prms = new Hashtable();
                prms["ObjectID"] = ObjectId;
                prms["DocTypeID"] = DocTypeId;
                prms["ObjectType"] = ObjectType;
                prms["DocTypeParam"] = DocTypeParam;
                prms["DocExpiryDate"] = DocExpiryDate;
                prms["ByUserID"] = ByUserId;
                prms["ProgramID"] = ProgramId;
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_DocTypeDates", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.DocFile.ApplicationDocTypeDates", "Save", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = logParams + "|Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                ADTDId = MsSql.CheckIntDBNull(dr["ADTDID"]);
                Message = MsSql.CheckStringDBNull(dr["Message"]);

                return ADTDId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DocFile.ApplicationDocTypeDates", "Save", ex, logParams);
                return false;
            }
        }
        #endregion
    }
}
