using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Email
{
    public class Log
    {
        #region --- Properties ---
        public String ConfigKey = String.Empty;
        public int StatusId = -1;
        public String Message = String.Empty;
        public long EmailLogId;
        public DateTime? CreatedDate;

        public SqlDb Db;
        #endregion --- Properties ---

        #region --- Constructors ---
        public Log() { Init(); }
        public Log(String configKey) : this() { Init(configKey); }
        #endregion --- Constructors ---

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
        }
        #endregion --- Private Methods ---

        public Boolean save(String module, Setting.Email.Type objectType, long objectId, String toEmail, String subject, String body, String attach = "")
        {
            try
            {
                //if (objectType <= 0 || objectId <= 0 || toEmail.Length <= 0) { Message = "Input parameters are missing"; return false; }

                Hashtable prms = new Hashtable();
                prms["ObjectType"] = (int)objectType;
                prms["ObjectID"] = objectId;
                prms["EmailTo"] = toEmail;
                prms["Notes"] = objectType.ToString();
                prms["EmailSubject"] = subject;
                prms["EmailBody"] = body;
                prms["EmailAttach"] = attach;
                prms["Source"] = module;

                Db.SetSql("p_A_EmailLog", prms);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds)) { Message = Db.SqlStmt + "|Error adding record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                long id = SqlDb.CheckLongDBNull(dr["EmailLogID"]);
                if (id > 0)
                {
                    StatusId = 1;
                    Message = "Success";
                    return true;
                }
                else
                {
                    StatusId = 0;
                    return false;
                }
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean Update()
        {
            try
            {
                if (EmailLogId <= 0) { Message = "EmailLogId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["EmailLogID"] = EmailLogId;
                if (CreatedDate != null) prms["CreatedDate"] = CreatedDate;

                Db.SetSql("p_U_EmailLog", prms);
                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message =Db.SqlStmt + "|Error updating record.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                EmailLogId = SqlDb.CheckLongDBNull(dr["EmailLogID"]);

                return EmailLogId > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
    }
}
