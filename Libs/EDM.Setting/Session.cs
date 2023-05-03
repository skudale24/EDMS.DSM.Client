using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using VTI.Common;

namespace EDM.Setting
{
    public class Session
    {
        public static long ProgramId
        {
            get
            {
                long programId = 0;
                String temp = String.Empty;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        //Session
                        if (HttpContext.Current?.Session != null)
                            try { temp = String.Empty + HttpContext.Current.Session["ProgramID"]; } catch { }
                        //Querystring
                        if (temp.Length <= 0) { try { temp = String.Empty + HttpContext.Current.Request["ProgramID"]; } catch { } }
                    }
                    //Config File
                    if (temp.Length <= 0) { try { temp = String.Empty + ConfigurationManager.AppSettings["ProgramID"]; } catch { } }

                    try { if (temp.Length > 0) programId = long.Parse(temp); } catch { }

                    try { if (HttpContext.Current?.Session != null && programId > 0) HttpContext.Current.Session["ProgramID"] = programId; } catch { }
                }
                catch { }
                return programId;
            }
        }
        public static long UserId
        {
            get
            {
                long userId = 0;
                try
                {
                    if (HttpContext.Current?.Session != null)
                    {

                        String temp = String.Empty + HttpContext.Current.Session["UserID"];
                        if (temp.Length > 0)
                            long.TryParse(temp, out userId);
                    }
                }
                catch { }
                return userId;
            }
        }
        public static String UserName
        {
            get
            {
                String userName = String.Empty;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        userName = String.Empty + HttpContext.Current.Session["FullName"];
                    }
                }
                catch { }
                return userName;
            }
        }
        public static long RoleId
        {
            get
            {
                long roleId = 0;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        String temp = String.Empty + HttpContext.Current.Session["RoleID"];
                        if (temp.Length > 0)
                            long.TryParse(temp, out roleId);
                    }
                }
                catch { }
                return roleId;
            }
        }
        public static String ProgramName
        {
            get
            {
                string programName = String.Empty;
                String temp = String.Empty;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        //Session
                        try { temp = String.Empty + HttpContext.Current.Session["ProgramName"]; } catch { }
                        //Querystring
                        if (temp.Length <= 0) { try { temp = String.Empty + HttpContext.Current.Request["ProgramName"]; } catch { } }
                    }
                    //Config File
                    if (temp.Length <= 0) { try { temp = String.Empty + ConfigurationManager.AppSettings["ProgramName"]; } catch { } }

                    try { if (temp.Length > 0) programName = temp; } catch { }

                    try { if (HttpContext.Current != null && programName != null) HttpContext.Current.Session["ProgramName"] = programName; } catch { }
                }
                catch { }
                return programName;
            }
        }
        public static long LpcId
        {
            get
            {
                long lpcId = 0;
                String temp = String.Empty;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        //Session
                        try { temp = String.Empty + HttpContext.Current.Session["LPCID"]; } catch { }
                        //Querystring
                        if (String.IsNullOrEmpty(temp)) { try { temp = String.Empty + HttpContext.Current.Request["LPCID"]; } catch { } }
                    }

                    try { if (!String.IsNullOrEmpty(temp)) lpcId = long.Parse(temp); } catch { }

                    try { if (HttpContext.Current != null && lpcId > 0) HttpContext.Current.Session["LPCID"] = lpcId; } catch { }
                }
                catch { }
                return lpcId;
            }
        }
    }
}
