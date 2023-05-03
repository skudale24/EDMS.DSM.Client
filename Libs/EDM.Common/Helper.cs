using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VTI.Common;

namespace EDM.Common
{
    public class MethodReturn
    {
        public Boolean Status { get; set; }
        public String Message { get; set; }

        /// <summary>
        /// dllPrms = EDM.Project.Appointment,EDM.Project,SendSchedule where
        /// EDM.Project.Appointment = class name
        /// EDM.Project = dll name
        /// SendSchedule = method name
        /// </summary>
        /// <returns>MethodReturn = Boolean Status, String Message</returns>
        public MethodReturn Invoke(string dllPrms, object[] ctorParams)
        {
            try
            {
                String[] invokeParams = dllPrms.Split(',');

                //Fetch Type
                Type typ = Type.GetType(invokeParams[0] + "," + invokeParams[1]);
                if (typ == null)
                {
                    Status = false;
                    Message = "Error fetching dll type for " + invokeParams[0];
                    return this;
                }

                //Create Object using ctor(module, configKey, programId,ByUserID)
                Object obj = Activator.CreateInstance(typ, ctorParams);
                if (obj == null)
                {
                    Status = false;
                    Message = "Error creating dll instance for " + invokeParams[0] + " using ctor(module, configKey, programId,ByUserID)";
                    return this;
                }

                //Method
                MethodInfo meth = typ.GetMethod(invokeParams[2]);
                if (meth == null)
                {
                    Status = false;
                    Message = "Error fetching dll method " + invokeParams[2];
                    return this;
                }
                //Method Paramters
                ParameterInfo[] prms = meth.GetParameters();
                object[] methPrms = new object[prms.Length];
                for (int idx = 0; idx < prms.Length; idx++)
                {
                    if (invokeParams.Length > 3 + idx)
                        methPrms[idx] = invokeParams[3 + idx];
                    else
                        methPrms[idx] = Type.Missing;
                }
                //Invoke Method
                return (EDM.Common.MethodReturn)meth.Invoke(obj, methPrms);
            }
            catch (Exception ex) { Status = false; Message = ex.Message; return this; }
        }
    }
    public class Helper
    {
        #region --- Populate ---
        public static void Fill(ListControl lc, DataSet ds, String valueField, String textField, Boolean addSelect = false)
        {
            VTI.Common.DataUtils.Fill(lc, ds, valueField, textField);
            if (addSelect) lc.Items.Insert(0, new ListItem("--- Select ---", "0"));
        }
        public static void GetStates4UseCase(ListControl uxState, Roles roleId, AddressType AddrType)
        {
            // Condition to check DesiredUserRole
            if (roleId.Equals(Roles.Advisor) 
                || roleId.Equals(Roles.Customer) 
                || roleId.Equals(Roles.LPCAdmin)
                || roleId.Equals(Roles.LPCManagedAdmin)
                || roleId.Equals(Roles.LPCManagedAdvisor)
                || roleId.Equals(Roles.LPCManagedUser)
                || roleId.Equals(Roles.LPCUser) 
                || roleId.Equals(Roles.LPCGM)
                || roleId.Equals(Roles.EPBAdmin))
            {
                VTI.Common.DataUtils.Fill(uxState, GetAll(1), EDM.Setting.Fields.StateID, EDM.Setting.Fields.StateAbbr);
                uxState.Items.Insert(0, new ListItem("Select", "0"));
            }
           else
            {
                VTI.Common.DataUtils.Fill(uxState, GetAll(0), EDM.Setting.Fields.StateID, EDM.Setting.Fields.StateAbbr);
                uxState.Items.Insert(0, new ListItem("Select", "0"));
            }
            // And/OR combination with AddresType
            // Accordingly fill the States Dropdown box and return
        }
        public static DataSet GetAll(long statusId = 1)
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["StatusID"] = statusId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_States", prms, out SqlforLog);
                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch { return null; }
        }
        public static void Fill(ref RadMenu uxRadMenu, DataSet ds, String keyValue, String keyParent, String keyText, String keyUrl)
        {
            try
            {
                uxRadMenu.DataFieldID = keyValue;
                uxRadMenu.DataFieldParentID = keyParent;
                uxRadMenu.DataValueField = keyValue;
                uxRadMenu.DataTextField = keyText;
                uxRadMenu.DataNavigateUrlField = keyUrl;

                uxRadMenu.DataSource = ds;
                uxRadMenu.DataBind();
            }
            catch (Exception ex) { Common.Log.Info("EDM.Common.Helper", "Fill", "Error:" + ex.Message); }
        }

        public static void Highlight(ref RadMenu uxRadMenu, HttpRequest req)
        {
            try
            {
                RadMenuItem rmi = uxRadMenu.FindItemByUrl(req.Url.PathAndQuery);
                if (rmi == null)
                {
                    try
                    {
                        String dir = Path.GetDirectoryName(req.Path);
                        if (dir.Length > 1)
                        {
                            dir = dir.Substring(1).Replace("Activity", "Activitie");
                            if (dir.IndexOf("\\") > 0) dir = dir.Substring(0, dir.IndexOf("\\"));
                            if (dir.Length > 0) rmi = uxRadMenu.FindItemByText(dir);
                            if (rmi == null) rmi = uxRadMenu.FindItemByText(dir + "s");
                        }
                    }
                    catch { }
                }
                if (rmi != null)
                    rmi.HighlightPath();
            }
            catch { }
        }
        #endregion

        #region --- Text ---
        public static String LMask(String input, int howManyPlain = 4)
        {
            try
            {
                int noToRepl = input.Length - howManyPlain;
                /* Feb 09, 2017 | Nibha Kothari | ES-2802: UV:14990 EFT Notifications: Last three Digits of Account & Routing# are Recurring */
                return input.Replace(input.Substring(0, noToRepl), new String('X', noToRepl));
            }
            catch { return String.Empty; }
        }
        public static String RMask(String input, int howManyPlain = 4)
        {
            try
            {
                int noToRepl = input.Length - howManyPlain;
                /* Feb 09, 2017 | Nibha Kothari | ES-2802: UV:14990 EFT Notifications: Last three Digits of Account & Routing# are Recurring */
                return input.Replace(input.Substring(howManyPlain), new String('X', noToRepl));
            }
            catch { return String.Empty; }
        }
        public static String Mask(String input, int howManyPlain = 4)
        {
            try
            {
                /* Feb 09, 2017 | Nibha Kothari | ES-2802: UV:14990 EFT Notifications: Last three Digits of Account & Routing# are Recurring */
                int noToRepl = input.Length - (howManyPlain * 2);
                return input.Replace(input.Substring(howManyPlain, noToRepl), new String('X', noToRepl));
            }
            catch { return String.Empty; }
        }

        public static String Capitalize(String input)
        {
            char[] array = input.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                    array[0] = char.ToUpper(array[0]);
            }
            // Scan through the letters, checking for spaces.
            // Uppercase the lowercase letters following spaces.
            // Lowercase the letters if not following a space.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                        array[i] = char.ToUpper(array[i]);
                }
                else
                {
                    if (char.IsUpper(array[i]))
                        array[i] = char.ToLower(array[i]);
                }
            }
            return new String(array);
        }

        /* Sep 01, 2017 | Nibha Kothari | ES-3766: CPS - Account Type Information in the System */
        public static String Camel2Phrase(String input)
        {
            char[] array = input.ToCharArray();
            String output = array[0].ToString();
            // Scan through the letters, checking for uppercase letters. Add space before the uppercase letters.
            for (int i = 1; i < array.Length; i++)
            {
                if (char.IsUpper(array[i]) && char.IsLower(array[i - 1]))
                    output += " ";

                output += array[i].ToString();
            }
            return output;
        }
        /* end Sep 01, 2017 | Nibha Kothari | ES-3766: CPS - Account Type Information in the System */
        /* Nov 28, 2017 | Nibha Kothari | ES-4255: PGEEBGDR: enbala - Symphony: Mapping */
        public static String AbbreviateCamel(String input)
        {
            char[] array = input.ToCharArray();
            String output = String.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                if (char.IsUpper(array[i]))
                    output += array[i].ToString();
            }
            return output;
        }
        /* end Nov 28, 2017 | Nibha Kothari | ES-4255: PGEEBGDR: enbala - Symphony: Mapping */

        public static String FormatWebsite(String website)
        {
            if (website.Length > 0)
            {
                if (website.StartsWith("http://") || website.StartsWith("https://"))
                    return website;

                website = website.StartsWith("www.") ? "http://" + website : "http://www." + website;
            }

            return website;
        }

        public static String GetDateWOTime(String date)
        {
            return date.Length > 0 ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(date)) : String.Empty;
        }

        public static String FormatCurrency(String amount)
        {
            return amount.Length > 0 ? "$" + String.Format(new System.Globalization.CultureInfo("en-US"), "{0:C}", amount) : String.Empty;
        }

        public static String GetTimeWODate(String date)
        {
            return date.Length > 0 ? String.Format("{0:HH:mm}", Convert.ToDateTime(date)) : String.Empty;
        }
        #endregion

        #region --- Path ---
        public static String RelPath2Phy(String phyUploadPath, String relVirtualLocation)
        {
            if (!relVirtualLocation.StartsWith("/")) relVirtualLocation = "/" + relVirtualLocation;
            return phyUploadPath + relVirtualLocation.Replace("/", "\\");
        }
        #endregion

        #region --- Date ---
        public static String GetCurrentTZAbbr()
        {
            /*
            String currTZ = TimeZone.CurrentTimeZone.StandardName;
            String[] temp = currTZ.Split(' ');
            currTZ = String.Empty;
            for (int idx = 0; idx < temp.Length; idx++)
                currTZ += temp[idx].Substring(0, 1);
            return currTZ;
            */
            return "CST";
        }
        public static DateTime ConvertToEST(DateTime input)
        {
            /*
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTime(input, est);
            */
            return input.AddHours(1);
        }
        public static String ConvertToEST(DateTime input, String sep, String open, String close, String format)
        {
            /* Oct 06, 2017 | Nibha Kothari | ES-3949: Get EDMS legacy scheduling system working */
            String displayEst = EDM.Setting.DB.GetByName(EDM.Setting.Key.DisplayEST);
            if (displayEst == "1")
                return ConvertToEST(input).ToString(format) + " EST" + sep + open + input.ToString(format) + " " + GetCurrentTZAbbr() + close;
            else
                return input.ToString(format) + " " + GetCurrentTZAbbr();
            /* end Oct 06, 2017 | Nibha Kothari | ES-3949: Get EDMS legacy scheduling system working */
        }
        public static DateTime ConvertToCST(DateTime input, String sourceTZ)
        {
            if (sourceTZ == "CST") return input;
            return TimeZoneInfo.ConvertTime(input, GetTZInfo(sourceTZ), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public static TimeZoneInfo GetTZInfo(String timeZone)
        {
            switch (timeZone)
            {
                case "EST":
                    timeZone = "Eastern Standard Time";
                    break;
                case "PST":
                    timeZone = "Pacific Standard Time";
                    break;
                case "MST":
                    timeZone = "Mountain Standard Time";
                    break;
            }
            return TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        }

        /// <summary>
        /// This method is used to get Date and Time from DateTimeString
        /// </summary>
        /// <param name="scheduleDateTime"></param>
        /// <returns></returns>
        public static Hashtable GetFormattedDateTime(String scheduleDateTime)
        {
            Hashtable prms = new Hashtable();
            prms["Date"] = String.Empty;
            prms["Time"] = String.Empty;

            DateTime auditDateTime;
            if (DateTime.TryParse(scheduleDateTime, out auditDateTime))
            {
                prms["Date"] = auditDateTime.ToString("dddd, MMMM dd, yyyy");
                prms["Time"] = auditDateTime.ToString("hh:mm tt");
            }

            return prms;
        }
        public static Hashtable GetFormattedAuditDateTimeTZ(String scheduleDateTime)
        {
            Hashtable prms = new Hashtable();
            prms["AuditDate"] = String.Empty;
            prms["AuditTime"] = String.Empty;

            DateTime auditDateTime;
            if (DateTime.TryParse(scheduleDateTime, out auditDateTime))
            {
                prms["AuditDate"] = auditDateTime.ToString("dddd, MMMM dd, yyyy");
                prms["AuditTime"] = Common.Helper.ConvertToEST(auditDateTime, " / ", String.Empty, String.Empty, "hh:mm tt");
            }

            return prms;
        }
        public static Hashtable GetFormattedAuditDateTime(String scheduleDateTime)
        {
            Hashtable prms = new Hashtable();
            prms["AuditDate"] = String.Empty;
            prms["AuditTime"] = String.Empty;

            DateTime auditDateTime;
            if (DateTime.TryParse(scheduleDateTime, out auditDateTime))
            {
                prms["AuditDate"] = auditDateTime.ToString("dddd, MMMM dd, yyyy");
                prms["AuditTime"] = auditDateTime.ToString("hh:mm tt");
            }

            return prms;
        }

        public static String GetMMddyyyFormattedString(String inputString)
        {
            try
            {
                DateTime proposedInserviceDate; String proposedInserviceDateStr = String.Empty;
                if (!String.IsNullOrEmpty(inputString))
                {
                    proposedInserviceDate = Convert.ToDateTime(inputString);
                    proposedInserviceDateStr = proposedInserviceDate.ToString("MM/dd/yyyy");
                }
                return proposedInserviceDateStr;
            }
            catch { return String.Empty; }
        }

        #endregion

        #region --- Web ---
        /// <summary>
        /// Method can be GET or POST. 
        /// RequestData will be required in case of POST. 
        /// In case of GET the data will be part of the url as the querystring.
        /// If external server requires authentication please specify UserName and Password.
        /// </summary>
        public static Hashtable ExecJSONRequest(String module, String url, String method
    , String userName = "", String password = "", String requestData = "", Dictionary<string, object> HeaderParams = null
    , String ContentType = "")
        {
            /* May 25, 2017 | Nibha Kothari | ES-3234: NEST Bulk Status Update - SSL TLS Fix */
            String logParams = "module:" + module + "|url:" + url + "|method:" + method + "|userName:" + userName + "|password:" + password
                + "|requestData:" + requestData;

            Hashtable retVals = new Hashtable();
            retVals["status"] = "0";

            try
            {
                /* May 25, 2017 | Nibha Kothari | ES-3234: NEST Bulk Status Update - SSL TLS Fix */
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                WebRequest request = HttpWebRequest.Create(url);
                if (!String.IsNullOrWhiteSpace(ContentType))
                    request.ContentType = ContentType;
                else
                    request.ContentType = "application/json";
                request.Method = method;
                if (!String.IsNullOrEmpty(userName)) request.Credentials = new NetworkCredential(userName, password);
                if (HeaderParams != null && HeaderParams.Count > 0)
                {
                    foreach (var item in HeaderParams)
                    {
                        var headerValue = item.Value == null ? string.Empty : item.Value.ToString();
                        request.Headers.Add(item.Key, headerValue);
                    }
                }

                if (!String.IsNullOrEmpty(requestData))
                {
                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(requestData);
                        writer.Flush();
                        writer.Close();
                    }
                }

                try
                {
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    retVals["status"] = "1";
                    retVals["response"] = reader.ReadToEnd();
                    foreach (string key in response.Headers.AllKeys)
                    {
                        if (key.ToLower() == "session-key")
                            retVals["sessionKey"] = response.Headers[key];

                        if (key.ToLower() == "x-workflow-id")
                            retVals["workflowId"] = response.Headers[key];

                        if (key.ToLower() == "x-total-count")
                            retVals["totalCount"] = response.Headers[key];
                    }
                    Log.Info(module, module + ":EDM.Common.Helper", "ExecJSONRequest", logParams + "| Response:" + retVals["response"]);
                }
                catch (WebException wex)
                {
                    using (var stream = wex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        retVals["response"] = reader.ReadToEnd();
                        if (String.IsNullOrWhiteSpace(retVals["response"].ToString()))
                            retVals["response"] = wex.Message;
                        Log.Error(module, module + ":EDM.Common.Helper", "ExecJSONRequest", wex, logParams + "|" + retVals["response"]);
                    }
                }
            }
            catch (Exception ex)
            {
                /* May 25, 2017 | Nibha Kothari | ES-3234: NEST Bulk Status Update - SSL TLS Fix */
                Log.Error(module, module + ":EDM.Common.Helper", "ExecJSONRequest", ex, logParams);
                retVals["response"] = ex.Message;
            }

            return retVals;
        }

        public static Boolean ValidateEmail(String emailAddress)
        {
            Regex re = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return re.IsMatch(emailAddress) ? true : false;
        }
        #endregion

        #region --- Security ---
        public static String GenerateUniqueEmail()
        {
            String output = null;
            try
            {
                output = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                output = output.Replace("=", String.Empty);
                output = output.Replace("+", String.Empty);
                output = output.Replace("/", String.Empty);
                return output + "@noemail.com";
            }
            catch { }
            return output;
        }
        public static String GenerateRandomPwd()
        {
            String output = null;
            try
            {
                System.Random Random = new Random();
                String chars = "abcdefghijklmnopqrstuvwxyz".ToUpper();
                String numbers = "0123456789";
                output = new String(chars.Select(c => chars[Random.Next(chars.Length)]).Take(4).ToArray());
                output += new string(numbers.Select(c => numbers[Random.Next(numbers.Length)]).Take(4).ToArray());
            }
            catch { }
            return output;
        }

        /* Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */
        public static String Decrypt(String input, long key) { return Decrypt(input, key.ToString()); }
        public static String Decrypt(String input, String key)
        {
            if (!String.IsNullOrEmpty(input) && input.StartsWith("EAAAAHNkZnM4N2FkZmE5MWFzMn"))
                input = VTI.Common.Crypto.decrypt(input, key);
            return input;
        }
        public static String Encrypt(String input, long key) { return Encrypt(input, key.ToString()); }
        public static String Encrypt(String input, String key)
        {
            if (!String.IsNullOrEmpty(input) && !input.StartsWith("EAAAAHNkZnM4N2FkZmE5MWFzMn"))
                input = VTI.Common.Crypto.encrypt(input, key);
            return input;
        }
        /* end Dec 08, 2017 | Nibha Kothari | ES-4291: Plain-text Password Decrypt Exception */

        static string EncryptionKey = "MAKV2SPBNI99212";

        public static string DecryptURLSafe(string cipherText)
        {
            return Decrypt(DecodeFromBase64UrlEncoder(cipherText), EncryptionKey);
        }
        public static string EncryptURLSafe(string clearText)
        {
            return EncodeToBase64UrlEncoder(Encrypt(clearText, EncryptionKey));
        }
        public static String DecodeFromBase64UrlEncoder(String encodedData)
        {
            return Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Decode(encodedData);
        }
        public static String EncodeToBase64UrlEncoder(String plainData)
        {
            return Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(plainData);
        }
        public static string CustomNavigateURL(string url)
        {
            string strPath = url.Substring(0, url.IndexOf("?"));
            string strQueryString = url.Substring(url.IndexOf("?") + 1);

            return strPath + "?ENC=" + EncryptURLSafe(strQueryString);
        }
        public static void DecryptAndSetQueryString(HttpApplication app)
        {
            if (HttpContext.Current.Request.QueryString["ENC"] != null)
            {
                string strDecryptedText = EDM.Common.Helper.DecryptURLSafe(HttpContext.Current.Request.QueryString["ENC"].ToString());
                app.Context.RewritePath(HttpContext.Current.Request.Path + "?" + strDecryptedText);
            }
        }
        #endregion

        /// <summary>
        /// This function is used to get the query string value except CallLog and AutoStart parameter used for Call Log 
        /// </summary>
        /// <param name="baseQueryString">Request.Url.Query</param>
        /// <returns>New Query String</returns>
        public static String GetQSWithoutCallLogQS(String baseQueryString)
        {
            String newQueryString = String.Empty;
            try
            {
                String[] arrQueryString = baseQueryString.Split('&');

                foreach (String item in arrQueryString)
                {
                    if (!item.ToLower().Contains(EDM.Setting.Fields.CallLog.ToLower()) && !item.ToLower().Contains(EDM.Setting.Fields.AutoStart.ToLower()))
                    {
                        if (String.IsNullOrEmpty(newQueryString))
                        {
                            newQueryString = item;
                        }
                        else
                        {
                            newQueryString = newQueryString + "&" + item;
                        }
                    }
                }
                return newQueryString;
            }
            catch (Exception ex)
            {
                Common.Log.Info("EDM.Common.Helper", "GetQSWithoutCallLogQS", "Error:" + ex.Message);
                return null;
            }
        }

        #region --- Database ---
        public static Boolean BulkCopy2Sql(String configKey, ref DataTable dt, EDM.Common.Log lg)
        {
            String logParams = String.Empty;
            try
            {
                if (dt == null) { lg.Info("BulkCopy2Sql", "dt is null."); return false; }

                logParams = "TableName:" + dt.TableName;
                if (dt.Rows.Count <= 0) { lg.Info("BulkCopy2Sql", logParams + " has 0 rows, not doing anything."); return true; }

                String dbConnStr = ConfigurationManager.AppSettings[configKey + "ConnString"];
                if (String.IsNullOrEmpty(dbConnStr)) { lg.Info("BulkCopy2Sql", logParams + "|dbConnStr is required."); return false; }

                lg.Info("BulkCopy2Sql", logParams);

                using (SqlConnection dbConn = new SqlConnection(dbConnStr))
                {
                    dbConn.Open();

                    using (SqlBulkCopy sbc = new SqlBulkCopy(dbConn))
                    {
                        sbc.BulkCopyTimeout = 60 * 60;
                        sbc.DestinationTableName = dt.TableName;

                        foreach (DataColumn dc in dt.Columns)
                            sbc.ColumnMappings.Add(dc.ToString(), dc.ToString());

                        lg.Info("BulkCopy2Sql", logParams + "|Going to bulk copy " + dt.Rows.Count + " rows.");
                        sbc.WriteToServer(dt);
                        lg.Info("BulkCopy2Sql", logParams + "|Bulk copied " + dt.Rows.Count + " rows.");

                        sbc.Close();
                    }

                    dbConn.Dispose();
                    dbConn.Close();
                }

                return true;
            }
            catch (Exception ex) { lg.Error("BulkCopy2Sql", ex, logParams); return false; }
            finally
            {
                dt.Clear();
                dt.Dispose();
                dt = null;
            }
        }

        //public static string GetLPCLogoHtml(long lpcId, String ProgramID = "")
        //{
        //    try
        //    {
        //        String ImageUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl);
        //        switch (ProgramID)
        //        {
        //            case "8":
        //                ImageUrl = ImageUrl + "/CDN/LPCLogo/logo_" + lpcId + ".png";
        //                break;
        //            default:
        //                ImageUrl = ImageUrl + "/CDN/TVA_LPCLogo/logo_" + lpcId + ".png";      //Ganesh Jamdurkar| DSM-4693 Remove UtilityLogo and take logo from CDN/TVA_LPCLogo/ 8 oct 2021 GJ
        //                break;
        //        }
        //        return "<img alt=\"\" class=\"max-wt\" style=\"float:right;border-width:0px;border-style:solid;\" src=\"" + ImageUrl + "\" >";  //ImageUrl + "\" height=\"50px\"
        //        //Added Internal Css Class for Single Image display in the eMail img url Ganesh Jamdurkar| DSM-4693 class=\"max-wt\"
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Info("EDM.Common.Helper", "GetQSWithoutCallLogQS", "Error:" + ex.Message);
        //        return string.Empty;
        //    }
        //}
        //public static string GetLPCLogo(long lpcId)
        //{
        //    try
        //    {
        //        String ImageUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl);
        //        ImageUrl = ImageUrl + "/CDN/TVA_LPCLogo/logo_" + lpcId + ".png";
        //        return ImageUrl;
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log.Info("EDM.Common.Helper", "GetLPCLogo", "Error:" + ex.Message);
        //        return string.Empty;
        //    }
        //}

        public static string GetEmailImagesUrl()
        {
            try
            {
                String ImageUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl);
                ImageUrl = ImageUrl + "/CDN/EmailImages";
                return ImageUrl;
            }
            catch (Exception ex)
            {
                Common.Log.Info("EDM.Common.Helper", "GetEmailImagesUrl", "Error:" + ex.Message);
                return string.Empty;
            }
        }
        #endregion --- Database ---
    }
    public class LeftMenuInfo
    {
        public String Name { get; set; }
        public int Sequence { get; set; }        
        public bool IsDisplay { get; set; }
        public String SetUrl { get; set; }
    }
    public enum Programs : long
    {
        /// <summary>
        /// 1 - eScore
        /// </summary>
        eScore = 1,
        /// <summary>
        /// 2 - Limited Income
        /// </summary>
        LimitedIncome = 2,
        /// <summary>
        /// 3 - eScore + LI
        /// </summary>
        eScoreWithLI = 3,
        /// <summary>
        /// 4 - DemandResponse
        /// </summary>
        DemandResponse = 4,
        /// <summary>
        /// 5 - eScore + DR
        /// </summary>
        eScoreWithDR = 5,
        /// <summary>
        /// 6 - HUP + DR
        /// </summary>
        HUPWithDR = 6,
        /// <summary>
        /// 7 - eScore + HUP + DR
        /// </summary>
        eScoreWithHUPandDR = 7,
        /// < summary >
        /// 8 - GPP
        /// </ summary >
        GPP = 8,
        /// < summary >
        /// 9 - eScore + GPP
        /// </ summary >
        eScoreWithGPP = 9,
        /// < summary >
        /// 9 - HUP + GPP
        /// </ summary >
        HUPWithGPP = 10,
        /// <summary>
        /// 7 - eScore + HUP + DR
        /// </summary>
        eScoreWithHUPandGPP = 11,
        /// < summary >
        /// 16 - DPP
        /// </ summary >
        DPP = 16,
        /// < summary >
        /// 19 - DPP
        /// </ summary >
        eScoreWithHUPandDPP = 19,
        /// < summary >
        /// 24 - DPP + GPP
        /// </ summary >
        DPPWithGPP = 24,
        /// < summary >
        /// 26 - DPP + GPP +HUP
        /// </ summary >
        HUPWithGPPandDPP = 26,
        /// < summary >
        /// 27 - eScore+ DPP + GPP +HUP
        /// </ summary >
        eScoreWithHUPGPPandDPP = 27,
        /// < summary >
        /// 255 - All
        /// </ summary >
        All = 255
    }

    public enum SurveyCompletionMethod : int
    {
        /// <summary>
        /// 1 - Email
        /// </summary>
        Email = 1,
        /// <summary>
        /// 2 - SMS
        /// </summary>
        SMS = 2,
        /// <summary>
        /// 3 - Web
        /// </summary>
        Web = 3,
        /// <summary>
        /// 4 - Mobile
        /// </summary>
        Mobile = 4,
    }

    public enum Roles : long
    {
        /// <summary>
        /// 1 - Admin
        /// </summary>
        Administrator = 1,
        /// <summary>
        /// 4 - Contractor
        /// </summary>
        Contractor = 4,
        /// <summary>
        /// 8 - TVA
        /// </summary>
        TVA = 8,
        /// <summary>
        /// 22 - LPC Admin
        /// </summary>
        LPCAdmin = 22,
        /// <summary>
        /// 23 - LPC Managed Advisor
        /// </summary>
        LPCManagedAdvisor = 23,
        /// <summary>
        /// 24 - LPC Managed User
        /// </summary>
        LPCManagedUser = 24,
        /// <summary>
        /// 25 - LPC Managed Admin
        /// </summary>
        LPCManagedAdmin = 25,
        /// <summary>
        /// 29 - LPC GM
        /// </summary>
        LPCGM = 29,
        /// <summary>
        /// 2 - Advisor
        /// </summary>
        Advisor = 2,
        /// <summary>
        /// 2 - Advisor
        /// </summary>
        LPCUser = 3,
        Customer = 999,
        ContractorAdmin = 21,
        QCNAdmin = 13,
        EPBAdmin = 1002
    }

    public enum ProjectType : long
    {
        /// <summary>
        /// 4 - In Progress Inspection
        /// </summary>
        InProgressInspection = 4,
        /// <summary>
        /// 13 - GeneralAppointment
        /// </summary>
        GeneralAppointment = 13,
        /// <summary>
        /// 1 - Evaluation
        /// </summary>
        Evaluation = 1,
        Inspection = 2,
        EvalInsp = 3,
    }
    public enum ProjectDuration:long
    {
        DurationEval = 90,
        DurationInsp = 90,
        DurationEvalInsp = 90,
        DurationInspHVAC1 =	120,
        DurationInspHVACGT1=	180,
        DurationReInsp = 30,
        DurationHVACTuneUp = 90,
        MaximumAppointmentDuration = 240,
        CSGFileCheckApp_Service_GetFileDuration = 0,
        CSGFileCheckApp_Report_GetFileDuration = 4,
        DurationInProgressInsp = 30,
        DurationGNRL = 30,
    }
    public enum AddressType : int
    {       
        ServiceAddress = 1,
        ParticipantMailingAddress = 2,       
        OwnerOfSystem = 3,
        ProjectContact = 4,
        DPPProducerMailingAddress = 5,
        RelocationAddress = 6,
        PhysicalAddress=7,
        MailingAddress=8
    }

    public enum CustomerAddressType : int
    {
        Residential = 1,
        Business = 2
    }

    public enum ProgramModel : int
    {
        Turnkey = 1,
        LPCManaged = 2
    }

    public enum PeerReviewType : int
    {
        CR = 1,
        TVA = 2
    }

    public enum CommonStatus : int
    {
        Active = 1,
        InActive = 0
    }
    public enum QualificationPreference : int
    {
        None = 1,
        AMI = 2,
        FPL = 3,
        Blend = 4
    }
}
