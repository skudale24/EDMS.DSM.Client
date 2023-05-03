using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.Common
{
    public class History
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long LpcId;
        public long ByUserId;

        private SqlDb Db;
        private EDM.Common.Log Lg;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Application.History";
            }
        }
        public long HistoryId;
        public string HistoryData;
        public int HistoryType;
        public long ObjectId;
        public long HistoryKey;
        public string ObjectType;
        public long DocObjectType;
        public long DocTypeId;
        public string CreatedByType;
        #endregion --- Properties ---

        #region --- Constructors ---
        public History()
        {
            ProgramId = EDM.Setting.Session.ProgramId;
            ByUserId = EDM.Setting.Session.UserId;
            LpcId = EDM.Setting.Session.LpcId;
            ConfigKey = String.Empty;
        }
        public History(String module) : this() { Module = module; ConfigKey = String.Empty; }
        public History(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        #endregion --- Constructors ---

        #region --- Public Methods ---
        public DataSet GetHistory()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ObjectID] = ObjectId;
                prms[EDM.Setting.Fields.ObjectType] = ObjectType;
                if (HistoryType > 0) prms[EDM.Setting.Fields.HistoryType] = HistoryType;  
                if (HistoryKey > 0) prms["HistoryKey"] = HistoryKey;   
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                if (DocObjectType > 0) prms["DocObjectType"] = DocObjectType;
                if (DocTypeId > 0) prms[EDM.Setting.Fields.DocTypeID] = DocTypeId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_History", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.WorkOrder.History", "GetWOHistory", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }

            catch (Exception ex) { Common.Log.Error(Module, Module + ":EDM.WorkOrder.History", "GetWOHistory", ex); return null; }
        }
        public Boolean Add()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required"; return false; }
                if (ObjectId <= 0) { Message = "ObjectId is required"; return false; }
                if (string.IsNullOrEmpty(ObjectType)) { Message = "ObjectType is required"; return false; }
                if (string.IsNullOrEmpty(HistoryData)) { Message = "HistoryData is required"; return false; }
                if (HistoryType <= 0) { Message = "HistoryType is required"; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required"; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.ProgramID] = ProgramId;
                prms[EDM.Setting.Fields.ObjectID] = ObjectId;
                prms[EDM.Setting.Fields.HistoryData] = HistoryData;
                prms[EDM.Setting.Fields.HistoryType] = HistoryType;
                prms[EDM.Setting.Fields.ObjectType] = ObjectType;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;
                prms["CreatedByType"] = CreatedByType;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_WOHistory", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.WorkOrder.History", "Add", SqlforLog);
                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error saving WorkOrder";
                    Common.Log.Info(Module + ":EDM.WorkOrder.History", "Add", Message);
                    return false;
                }
                HistoryId = Convert.ToInt64(ds.Tables[0].Rows[0]["HistoryId"]);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.WorkOrder.History", "Add", ex, "WorkOrder [" + ByUserId + "] UserId ");
                return false;
            }
        }
        #endregion --- Public Methods ---
    }
}
