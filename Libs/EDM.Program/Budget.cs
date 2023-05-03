using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Program
{
    public class Budget
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;
        #endregion

        #region --- Constructors ---
        public Budget() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public Budget(String module) : this() { Module = module; }
        #endregion

        #region --- Public Methods ---
        public DataSet GetAvailability()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_ProgramBudgetAvailability", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.Budget", "GetAvailability", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Program.Budget", "GetAvailability", ex, logParams);
                return null;
            }
        }
        #endregion
    }
}
