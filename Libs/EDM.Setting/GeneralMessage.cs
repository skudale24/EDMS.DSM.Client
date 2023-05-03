using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    public class GeneralMessage
    {
        #region --- Constants ---
        public const String Error_SystemError = "Error_SystemError";
        public const String Error_NotAbleToPerformOperation = "Error_NotAbleToPerformOperation";
        public const String Message_AddRecordSuccessfully = "Message_AddRecordSuccessfully";
        public const String Message_UpdateRecordSuccessfully = "Message_UpdateRecordSuccessfully";
        public const String Validation_UserIsNotAuthenticated = "Validation_UserIsNotAuthenticated";
        public const String Validation_InvalidUserOrPassword = "Validation_InvalidUserOrPassword";
        public const String Format_DateTimeFormat = "Format_DateTimeFormat";
        public const String Check_ForExistUserAndServiceAddress = "Check_ForExistUserAndServiceAddress";
        public const String Error_File_Not_Found = "Error_File_Not_Found";
        public const String Configuration_AllowedExtenstion = "Configuration_AllowedExtenstion";
        public const String Request_Time_Out = "Request_Time_Out";
        public const String ExportExcelFormat_DateTimeFormat = "ExportExcelFormat_DateTimeFormat";
        public const String ExportExcelFormat_DateFormat = "ExportExcelFormat_DateFormat";
        #endregion

        #region --- Properties ---
        private int? _GeneralMessageId;
        public int? GeneralMessageId
        {
            get { return _GeneralMessageId; }
            set { _GeneralMessageId = value; }
        }

        private string _GeneralMessageKey;
        public string GeneralMessageKey
        {
            get { return _GeneralMessageKey; }
            set { _GeneralMessageKey = value; }
        }

        private string _GeneralMessageValue;
        public string GeneralMessageValue
        {
            get { return _GeneralMessageValue; }
            set { _GeneralMessageValue = value; }
        }

        private int? _LanguageId;
        public int? LanguageId
        {
            get { return _LanguageId; }
            set { _LanguageId = value; }
        }
        #endregion

        #region --- Public Methods ---
        public List<GeneralMessageSearch> Search()
        {
            Hashtable prms = new Hashtable();
            String SqlforLog = string.Empty;
            String sql = MsSql.GetSqlStmt("p_GET_GeneralMessagesSearch", prms, out SqlforLog);

            DataSet ds = MsSql.ExecuteNoTransQuery(sql);
            if (MsSql.IsEmpty(ds)) return null;

            List<GeneralMessageSearch> lst = new List<GeneralMessageSearch>();
            if (MsSql.IsEmpty(ds)) return lst;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GeneralMessageSearch gms = new GeneralMessageSearch();
                gms.GeneralMessageId = MsSql.CheckIntDBNull(dr["GeneralMessageId"]);
                gms.GeneralMessageKey = MsSql.CheckStringDBNull(dr["GeneralMessageKey"]);
                gms.GeneralMessageValue = MsSql.CheckStringDBNull(dr["GeneralMessageValue"]);

                lst.Add(gms);
            }

            return lst;
        }

        public static String GetByKey(List<GeneralMessageSearch> gmLst, String key)
        {
            if (gmLst == null || gmLst.Count <= 0) return String.Empty;

            GeneralMessageSearch gm = gmLst.Find(delegate (GeneralMessageSearch o) { return o.GeneralMessageKey == key; });
            return gm != null ? gm.GeneralMessageValue : String.Empty;
        }
        #endregion
    }

    public class GeneralMessageSearch : GeneralMessage
    {
        private int? _PageIndex;
        public int? PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        private int? _PageSize;
        public int? PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        private int? _TotalCount;
        public int? TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; }
        }

        private string _SearchMethod;
        public string SearchMethod
        {
            get { return _SearchMethod; }
            set { _SearchMethod = value; }
        }

        private string _SearchName;
        public string SearchName
        {
            get { return _SearchName; }
            set { _SearchName = value; }
        }

        private string _OrderBy;
        public string OrderBy
        {
            get { return _OrderBy; }
            set { _OrderBy = value; }
        }

        private string _OrderSequence;
        public string OrderSequence
        {
            get { return _OrderSequence; }
            set { _OrderSequence = value; }
        }

        private string _GeneralMessageIds;
        public string GeneralMessageIds
        {
            get { return _GeneralMessageIds; }
            set { _GeneralMessageIds = value; }
        }

        private string _NotInGeneralMessageIds;
        public string NotInGeneralMessageIds
        {
            get { return _NotInGeneralMessageIds; }
            set { _NotInGeneralMessageIds = value; }
        }

        private string _LanguageName;
        public string LanguageName
        {
            get { return _LanguageName; }
            set { _LanguageName = value; }
        }

        private string _Action;
        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        private string _ExactGeneralMessageKey;
        public string ExactGeneralMessageKey
        {
            get { return _ExactGeneralMessageKey; }
            set { _ExactGeneralMessageKey = value; }
        }
    }
}
