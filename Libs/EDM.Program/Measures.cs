using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Program
{
    public class Measure
    {
        public const int AirSealing = 1;
        public const int AtticInsulation = 2;
        public const int DuctSystem = 3;
        public const int Lighting = 4;
        public const int HVACSystem = 5;
        public const int AppliancesElectronics = 6;
        public const int WaterHeating = 7;
        public const int Refrigerators = 8;
        public const int WindowsAndDoors = 9;
        public const int WallInsulation = 10;
        public const int DirectH2O = 11;
        public const int HealthAndSafety = 12;
        public const int RepairLI = 101;
        public const int WAPMeasureID = 1001;

        public class Result
        {
            public const String P1 = "P1";
            public const String P2 = "P2";
            public const String F3 = "F3";
            public const String F4 = "F4";

            public static Boolean IsFail(String result)
            {
                return result == P2 || result == F3 || result == F4 ? true : false;
            }
        }
    }

    public class Measures
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId = 1;
        #endregion

        #region --- Constructors ---
        public Measures() { ProgramId = Setting.Session.ProgramId; }
        public Measures(String module) : this() { Module = module; }
        #endregion

        #region --- Public Methods ---
        public virtual DataSet GetAll()
        {
            try
            {
                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_Measures", prms, out SqlforLog);
                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.Message; return null; }
        }
        #endregion
    }

    public class MeasureDataType
    {
        public const int Eval = 1;
        public const int Insp = 2;
    }
}
