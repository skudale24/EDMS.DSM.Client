using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Common
{
    public class DBErrorHistory
    {
        #region --- Members ---
        public String Module;
        public String Message;
        #endregion

        #region --- Constructors ---
        public DBErrorHistory() { }
        public DBErrorHistory(String module) { Module = module; }
        #endregion

        #region --- Methods ---
        public DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DBErrorHistory", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Common.DBErrorHistory", "GetAll", SqlforLog);

                return MsSql.ExecuteNoTransQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Common.DBErrorHistory", "GetAll", ex);
                return null;
            }
        }
        #endregion
    }
}
