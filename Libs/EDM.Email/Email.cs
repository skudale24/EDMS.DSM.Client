using System;
using System.Collections;
using System.Data;
using System.Web;
using VTI.Common;

namespace EDM.Email
{
    public class Email
    {
        #region --- Constants ---
        public String StatusRequest = "Request";
        public String StatusSent = "Sent";
        #endregion

        #region --- Properties ---
        public String Message = String.Empty;

        public long EmailId = 0;
        public String Origin = String.Empty;
        public String Module = String.Empty;
        public String Method = String.Empty;
        public String FromEmail = String.Empty;
        public String ToEmail = String.Empty;
        public String Subject = String.Empty;
        public String Body = String.Empty;
        public String Attachments = String.Empty;
        public String Status = String.Empty;
        #endregion

        #region --- Constructors ---
        public Email() { }
        public Email(String origin, String module, String method, String fromEmail, String toEmail, String subject, String body, String attachments)
        {
            Origin = origin;
            Module = module;
            Method = method;
            FromEmail = fromEmail;
            ToEmail = toEmail;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }
        #endregion

        #region --- Public Methods ---
        public Boolean Add()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["Origin"] = Origin;
                prms["Module"] = Module;
                prms["Method"] = Method;
                prms["FromEmail"] = FromEmail;
                prms["ToEmail"] = ToEmail;
                prms["Subject"] = Subject;
                prms["Body"] = Body;
                prms["Attachments"] = Attachments;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Email", prms, out SqlforLog);
                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = "Error executing " + SqlforLog; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                EmailId = MsSql.CheckLongDBNull(dr["EmailID"]);
                return EmailId > 0 ? true : false;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean Update()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["EmailID"] = EmailId;
                prms["Status"] = Status;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_Email", prms, out SqlforLog);
                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds)) { Message = "Error executing " + SqlforLog; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["EmailID"]) > 0 ? true : false;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public DataSet GetUnsent()
        {
            try
            {
                Hashtable prms = new Hashtable();

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Emails", prms, out SqlforLog);
                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.ToString(); return null; }
        }
        #endregion
    }
}
