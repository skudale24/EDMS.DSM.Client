using System;
using System.Collections.Generic;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    public class SettingMessage
    {
        #region --- Properties ---
        public String Message;
        public long ProgramId;

        private long? _SettingID;
        public long? SettingID
        {
            get { return _SettingID; }
            set { _SettingID = value; }
        }

        private string _Module;
        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }

        private string _Key;
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _StatusID;
        public string StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }

        private DateTime? _TranDate;
        public DateTime? TranDate
        {
            get { return _TranDate; }
            set { _TranDate = value; }
        }

        private string _StatusName;
        public string StatusName
        {
            get { return _StatusName; }
            set { _StatusName = value; }
        }
        #endregion

        #region --- Constructors ---
        public SettingMessage() { ProgramId = EDM.Setting.Session.ProgramId; }
        public SettingMessage(DataRow dr)
        {
            SettingID = MsSql.CheckLongDBNull(dr["SettingID"]);
            Module = MsSql.CheckStringDBNull(dr["Module"]);
            Key = MsSql.CheckStringDBNull(dr["Key"]);
            Value = MsSql.CheckStringDBNull(dr["Value"]);
        }
        #endregion

        #region --- Public Methods ---
        public List<SettingMessage> Populate()
        {
            try
            {
                EDM.Setting.DB db = new EDM.Setting.DB(Module);

                DataSet ds = db.GetAll();
                if (MsSql.IsEmpty(ds)) return null;

                List<SettingMessage> lst = new List<SettingMessage>();

                if (MsSql.IsEmpty(ds)) return lst;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lst.Add(new SettingMessage(dr));
                }

                return lst;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return null;
            }
        }
        #endregion
    }
}
