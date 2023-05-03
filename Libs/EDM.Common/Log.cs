using System;
using System.Collections;
using System.Data;
using System.Web;
using VTI.Common;

namespace EDM.Common
{
    public class Log
    {
        #region --- Properties ---
        public String ModuleName;
        public String Message;
        public String ConfigKey;
        #endregion --- Properties ---

        #region --- Constructors ---
        public Log() { }
        public Log(String configKey) : this() { ConfigKey = configKey; }
        #endregion --- Constructors ---

        #region --- Private Methods ---
        private static String GetCurrUserId()
        {
            String val = String.Empty;
            try
            {
                if (HttpContext.Current?.Session != null)
                {
                    val = HttpContext.Current.Session["UserID"]?.ToString();
                    if (String.IsNullOrWhiteSpace(val)) val =SqlDb.CheckStringDBNull(HttpContext.Current.Session["CustomerLoginID"]).ToString();
                }
            }
            catch { }
            return val;
        }
        private static String GetCurrUserName()
        {
            String val = String.Empty;
            try
            {
                if (HttpContext.Current!= null && HttpContext.Current.Session != null)
                    val = SqlDb.CheckStringDBNull("" + HttpContext.Current.Session["FullName"]).ToString();
            }
            catch { }
            return val;
        }
        private static String GetCurrUrl()
        {
            String val = String.Empty;
            try
            {
                if (HttpContext.Current != null)
                    val = HttpContext.Current.Request.RawUrl;
            }
            catch { }
            return val;
        }
        private static String GetCurrPhyPath()
        {
            String val = String.Empty;
            try
            {
                if (HttpContext.Current != null)
                    val = HttpContext.Current.Request.PhysicalPath.ToString();
            }
            catch { }
            return val;
        }
        #endregion

        #region --- Public Static Methods ---
        /// <summary>
        /// logFileName is the suffix to be appended between Config->LogFilePath and current date: 
        /// (<LogFilePath>_logFileName_<current date>.log)
        /// </summary>
        public static void Debug(String moduleName, String methodName, String info, String logFileName = "")
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Debug, moduleName, methodName, info, logFileName);
            }
            catch { }
        }

        /// <summary>
        /// logFileName is the suffix to be appended between Config->LogFilePath and current date: 
        /// (<LogFilePath>_logFileName_<current date>.log)
        /// </summary>
        public static void Info(String moduleName, String methodName, String info, String logFileName = "")
        {
            try
            {
                string extra = "|EXTRA: @UserId=" + GetCurrUserId() + ",@Username=" + GetCurrUserName() + ",@NavUrl=" + GetCurrUrl();
                VTI.Common.Log.Write(VTI.Common.Log.Level.Info, moduleName, methodName, info + extra, logFileName);
            }
            catch { }
        }

        /// <summary>
        /// logFileName is the suffix to be appended between Config->LogFilePath and current date: 
        /// (<LogFilePath>_logFileName_<current date>.log)
        /// </summary>
        public static void Error(String module, String moduleName, String methodName, String msg
            , String extra = "", Boolean sendEmail = true, String logFileName = "")
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Error, moduleName, methodName
                    , "ERROR:" + msg + "|EXTRA:" + extra + ":" + GetCurrUserId() + ":" + GetCurrUrl(), logFileName);

                if (!sendEmail) return;

                String message = "<table border='1'>"
                    + "<tr><td>Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td>Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td>Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>";

                if (extra.Length > 0) message += "<tr><td>Extra:</td><td>" + extra + "</td></tr>";

                Email.MailMessage mm = new Email.MailMessage();
                mm.Send(new Setting.DB(module).GetByKey(Setting.Key.FromEmail)
                    , Setting.DB.GetByName(Setting.Key.AdminEmail)
                    , moduleName + ":" + methodName + ":ERROR"
                    , message);
            }
            catch { }
        }
        /// <summary>
        /// logFileName is the suffix to be appended between Config->LogFilePath and current date: 
        /// (<LogFilePath>_logFileName_<current date>.log)
        /// </summary>
        public static void Error(String module, String moduleName, String methodName, Exception ex
            , String extra = "", Boolean sendEmail = true, String logFileName = "")
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Error, moduleName, methodName
                    , "ERROR:" + ex.Message + "|EXTRA:" + extra + ":" + GetCurrUserId() + ":" + GetCurrUrl(), logFileName);

                if (!sendEmail) return;

                String message = "<table border='1'>"
                    + "<tr><td>Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td>Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td>Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>"
                    + "<tr><td>Source:</td><td>" + ex.Source + "</td></tr>"
                    + "<tr><td>Page Name:</td><td>" + ex.TargetSite.DeclaringType.FullName + "</td></tr>"
                    + "<tr><td>Method Name:</td><td>" + ex.TargetSite + "</td></tr>"
                    + "<tr><td>Exception Details:</td><td>" + ex.Message + "</td></tr>"
                    + "<tr><td>Extra Information:</td><td>" + ex.StackTrace + "</td></tr>"
                    + "<tr><td>Exception:</td><td>" + ex.ToString() + "</td></tr>";

                if (extra.Length > 0) message += "<tr><td>Extra:</td><td>" + extra + "</td></tr>";

                int idx = 2;
                String inExDets = String.Empty;
                while (ex.InnerException != null)
                {
                    inExDets += "<br />" + idx + "<br />" + ex.InnerException.ToString();
                    idx++;
                    ex = ex.InnerException;
                }

                message += "<tr><td>Inner Exception Details:</td><td>" + inExDets + "</td></tr>"
                     + "</table>";

                Email.MailMessage mm = new Email.MailMessage();
                mm.Send(new Setting.DB(module).GetByKey(Setting.Key.FromEmail)
                    , Setting.DB.GetByName(Setting.Key.AdminEmail)
                    , moduleName + ":" + methodName + ":ERROR"
                    , message);
            }
            catch { }
        }

        public static void Exception(String dbConnStr, String moduleName, String methodName, Exception ex
            , String extra = "", Boolean sendEmail = true, String logFileName = "")
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Error, moduleName, methodName
                    , "ERROR:" + ex.Message + "|EXTRA:" + extra + ":" + GetCurrUserId() + ":" + GetCurrUrl(), logFileName);

                if (!sendEmail) return;

                String message = "<table border='1'>"
                    + "<tr><td>Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td>Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td>Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>"
                    + "<tr><td>Source:</td><td>" + ex.Source + "</td></tr>"
                    + "<tr><td>Page Name:</td><td>" + ex.TargetSite.DeclaringType.FullName + "</td></tr>"
                    + "<tr><td>Method Name:</td><td>" + ex.TargetSite + "</td></tr>"
                    + "<tr><td>Exception Details:</td><td>" + ex.Message + "</td></tr>"
                    + "<tr><td>Extra Information:</td><td>" + ex.StackTrace + "</td></tr>"
                    + "<tr><td>Exception:</td><td>" + ex.ToString() + "</td></tr>";

                if (extra.Length > 0) message += "<tr><td>Extra:</td><td>" + extra + "</td></tr>";

                int idx = 2;
                String inExDets = String.Empty;
                while (ex.InnerException != null)
                {
                    inExDets += "<br />" + idx + "<br />" + ex.InnerException.ToString();
                    idx++;
                    ex = ex.InnerException;
                }

                message += "<tr><td>Inner Exception Details:</td><td>" + inExDets + "</td></tr>"
                     + "</table>";

                EDM.Setting.DB stg = new Setting.DB(dbConnStr, 0);

                Email.MailMessage mm = new Email.MailMessage(dbConnStr, 0);

                mm.Send(stg.GetByKey(Setting.Key.FromEmail), stg.GetByKey(Setting.Key.AdminEmail), moduleName + ":" + methodName + ":ERROR", message);
            }
            catch { }
        }

        public static String GetExceptionMessage(Exception ex)
        {
            String message = string.Empty;
            try
            {
                message = "<b>System Exception</b> : <br/>";               
                message += "<table border='1'>"
                    + "<tr><td width="+"20%"+">Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Source:</td><td>" + ex.Source + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Page Name:</td><td>" + ex.TargetSite.DeclaringType.FullName + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Method Name:</td><td>" + ex.TargetSite + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Exception Details:</td><td>" + ex.Message + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Extra Information:</td><td>" + ex.StackTrace + "</td></tr>"
                    + "<tr><td width=" + "20%" + ">Exception:</td><td>" + ex.ToString() + "</td></tr>";                

                int idx = 2;
                String inExDets = String.Empty;
                while (ex.InnerException != null)
                {
                    inExDets += "<br />" + idx + "<br />" + ex.InnerException.ToString();
                    idx++;
                    ex = ex.InnerException;
                }

                message += "<tr><td width=" + "20%" + ">Inner Exception Details:</td><td>" + inExDets + "</td></tr>"
                     + "</table>";
            }
            catch { }

            return message;
        }
        #endregion

        #region --- Public Methods ---
        public void Debug(String methodName, String info)
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Debug, ModuleName, methodName, info, ConfigKey);
            }
            catch { }
        }

        public void Info(String methodName, String info)
        {
            try
            {
              VTI.Common.Log.Write(VTI.Common.Log.Level.Info, ModuleName, methodName, info, ConfigKey);
            }
            catch { }
        }

        public void Error(String methodName, String info, String extra = "", Boolean sendEmail = true)
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Error, ModuleName, methodName
                    , "ERROR:" + info + "|EXTRA:" + extra + ":" + GetCurrUserId() + ":" + GetCurrUrl(), ConfigKey);

                if (!sendEmail) return;

                String message = "<table border='1'>"
                    + "<tr><td>Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td>Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td>Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>";

                if (extra.Length > 0) message += "<tr><td>Extra:</td><td>" + extra + "</td></tr>";

                EDM.Setting.DB stg = new Setting.DB(ConfigKey, 0);

                Email.MailMessage mm = new Email.MailMessage(ConfigKey, 0);
                mm.Send(stg.GetByKey(Setting.Key.FromEmail), stg.GetByKey(Setting.Key.AdminEmail)
                    , ModuleName + ":" + methodName + ":ERROR", message);
            }
            catch { }
        }
        /// <summary>
        /// logFileName is the suffix to be appended between Config->LogFilePath and current date: 
        /// (<LogFilePath>_logFileName_<current date>.log)
        /// </summary>
        public void Error(String methodName, Exception ex, String extra = "", Boolean sendEmail = true)
        {
            try
            {
                VTI.Common.Log.Write(VTI.Common.Log.Level.Error, ModuleName, methodName
                    , "ERROR:" + ex.Message + "|EXTRA:" + extra + ":" + GetCurrUserId() + ":" + GetCurrUrl(), ConfigKey);

                if (!sendEmail) return;

                String message = "<table border='1'>"
                    + "<tr><td>Username:</td><td>" + GetCurrUserId() + ":" + GetCurrUserName() + "</td></tr>"
                    + "<tr><td>Error Path:</td><td>" + GetCurrPhyPath() + "</td></tr>"
                    + "<tr><td>Raw Url:</td><td>" + GetCurrUrl() + "</td></tr>"
                    + "<tr><td>Source:</td><td>" + ex.Source + "</td></tr>"
                    + "<tr><td>Page Name:</td><td>" + ex.TargetSite.DeclaringType.FullName + "</td></tr>"
                    + "<tr><td>Method Name:</td><td>" + ex.TargetSite + "</td></tr>"
                    + "<tr><td>Exception Details:</td><td>" + ex.Message + "</td></tr>"
                    + "<tr><td>Extra Information:</td><td>" + ex.StackTrace + "</td></tr>"
                    + "<tr><td>Exception:</td><td>" + ex.ToString() + "</td></tr>";

                if (extra.Length > 0) message += "<tr><td>Extra:</td><td>" + extra + "</td></tr>";

                int idx = 2;
                String inExDets = String.Empty;
                while (ex.InnerException != null)
                {
                    inExDets += "<br />" + idx + "<br />" + ex.InnerException.ToString();
                    idx++;
                    ex = ex.InnerException;
                }

                message += "<tr><td>Inner Exception Details:</td><td>" + inExDets + "</td></tr>"
                     + "</table>";

                EDM.Setting.DB stg = new Setting.DB(ConfigKey, 0);

                Email.MailMessage mm = new Email.MailMessage(ConfigKey, 0);
                mm.Send(stg.GetByKey(Setting.Key.FromEmail), stg.GetByKey(Setting.Key.AdminEmail)
                    , ModuleName + ":" + methodName + ":ERROR", message);
            }
            catch { }
        }

        /// <summary>
        /// Module is required. SP featches Setting->%CleanDays and deletes logs older than that from corresponding tables.
        /// </summary>
        public Boolean Delete()
        {
            try
            {
                Hashtable prms = new Hashtable();

                SqlDb db = new SqlDb(ConfigKey);
                db.SetSql("p_D_LogClean", prms);
                DataSet ds = db.ExecuteQuery();

                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Error deleting records.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Count"]) + " records deleted.";
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

      
        #endregion --- Public Methods ---
    }
}
