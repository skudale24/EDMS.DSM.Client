using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    /* Jan 17, 2017 | Nibha Kothari | ES-1874: Measure Based Email Notification */
    public class EmailExtra
    {
        #region --- Properties ---
        public String Module = String.Empty;
        public String Message = String.Empty;
        public String ConfigKey = String.Empty;
        public long ProgramId;
        public long ByUserId = 0;

        public SqlDb Db;

        public long ESEId = 0;
        public long ESObjectId = 0;
        public long ObjectId = 0;
        public String ObjectType = String.Empty;
        public String Body = String.Empty;
        public int StatusId = 0;
        public long ModifiedById = 0;
        public String ModifiedDate = String.Empty;
        public String SubMeasureName = String.Empty;
        #endregion

        #region --- Constructors ---
        public EmailExtra() { ProgramId = Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; Init(); }
        public EmailExtra(String module) : this() { this.Module = module; }
        public EmailExtra(String module, String configKey) : this(module) { Init(configKey); }
        public EmailExtra(String module, String configKey, long programId) : this(module, configKey) { ProgramId = programId; }
        public EmailExtra(String module, long esObjectId) : this(module) { ESObjectId = esObjectId; }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
        }
        #endregion --- Private Methods ---

        public Boolean GetById(Hashtable prms)
        {
            try
            {
                Db.SetSql("p_GET_EmailSettingExtra", prms);
                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) { Message = "Error retrieving details from DB"; return false; }

                DataRow dr = ds.Tables[0].Rows[0];

                ESObjectId = SqlDb.CheckLongDBNull(dr["ESObjectID"]);
                ObjectId = SqlDb.CheckLongDBNull(dr["ObjectID"]);
                SubMeasureName = SqlDb.CheckStringDBNull(dr["SubMeasureName"]);
                ObjectType = SqlDb.CheckStringDBNull(dr["ObjectType"]);
                Body = SqlDb.CheckStringDBNull(dr["Body"]);
                StatusId = SqlDb.CheckIntDBNull(dr["StatusID"]);
                ModifiedById = SqlDb.CheckLongDBNull(dr["ModifiedByID"]);
                ModifiedDate = SqlDb.CheckStringDBNull(dr["ModifiedDate"]);

                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        /// <summary>
        /// Module, ESEId are required.
        /// </summary>
        public Boolean GetById()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ESEId <= 0) { Message = "ESEId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["ESEID"] = ESEId;

                return GetById(prms);
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        /// <summary>
        /// Module, ESObjectId, ObjectType (SubMeasure,etc.), objectIds (SubMeasureID1,SubMeasureID2,etc.) are required.
        /// </summary>
        public Boolean GetByESId(String objectIds)
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (ESObjectId <= 0) { Message = "ESObjectId is required."; return false; }
                if (String.IsNullOrEmpty(ObjectType)) { Message = "ObjectType is required."; return false; }
                if (String.IsNullOrEmpty(objectIds)) { Message = "objectIds is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["ESObjectID"] = ESObjectId;
                prms["ObjectType"] = ObjectType;
                prms["ObjectIDs"] = objectIds;

                Boolean retVal = GetById(prms);
                if (!retVal) return false;

                if (StatusId != 1) Body = String.Empty;

                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                if (ProgramId > 0) prms["ProgramID"] = ProgramId;
                if (ObjectId > 0) prms["ObjectID"] = ObjectId;

                Db.SetSql("p_GET_EmailSettingExtras", prms);
                return Db.ExecuteNoTransQuery();
            }
            catch (Exception ex) { Message = ex.ToString(); return null; }
        }

        public Boolean Save()
        {
            try
            {
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                if (ESEId > 0) prms["ESEID"] = ESEId;
                prms["ProgramID"] = ProgramId;
                if (ESObjectId > 0) prms["ESObjectID"] = ESObjectId;
                if (!String.IsNullOrEmpty(ObjectType)) prms["ObjectType"] = ObjectType;
                if (ObjectId > 0) prms["ObjectID"] = ObjectId;
                if (!String.IsNullOrEmpty(Body)) prms["Body"] = Body;
                if (StatusId > 0) prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                Db.SetSql("p_AU_EmailSettingExtra", prms);
                DataSet ds = Db.ExecuteNoTransQuery();
                if (SqlDb.IsEmpty(ds)) { Message = "Error saving record."; return false; }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = SqlDb.CheckStringDBNull(dr["Message"]);
                ESEId = SqlDb.CheckLongDBNull(dr["ESEID"]);

                return ESEId <= 0 ? false : true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean ProcessBody(Hashtable substitutions, ref String body)
        {
            try
            {
                foreach (DictionaryEntry de in substitutions)
                {
                    ObjectType = (String)de.Key;
                    GetByESId((String)de.Value);
                    body = body.Replace("%%ESExtra" + ObjectType + "%%", Body);
                }
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
    }
    /* end Jan 17, 2017 | Nibha Kothari | ES-1874: Measure Based Email Notification */
}
