using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Program
{
    public class SubMeasure
    {
        public const int AtticInsulation = 1;
        public const int KneeWallInsulation = 11;
        public const int StaticVentilation = 12;
        public const int DIYInstallation = 101;

        public const String AtticInsulationName = "Attic Insulation";
        public const String KneeWallInsulationName = "Knee Wall Insulation";
        public const String StaticVentilationName = "Static Ventilation";
}

    public class SubMeasures
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;

        public long MeasureId;
        public String MeasureName = String.Empty;
        public long SubMeasureId;
        public long ProgramSMIID;
        public String SubMeasureName = String.Empty;
        public double kWh = 0;
        public double kW = 0;
        public int SelfRebate = 0;
        public double SRUnitPerc = 0;
        public double SRUnitAmt = 0;
        public double SRHomeAmt = 0;
        public int ContractorRebate = 0;
        public double CRUnitPerc = 0;
        public double CRUnitAmt = 0;
        public double CRHomeAmt = 0;
        public int LimitId = 0;
        public int FinAvail = 0;
        public int StatusId = 0;
        public String TranDate;
        public String ByUser;
        public int SubMeasureType = 0;
        public int CalculateSavings = 0;
        public String ModifiedDate;
        #endregion

        #region --- Constructors ---
        public SubMeasures() { ProgramId = Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public SubMeasures(String module) : this() { Module = module; }
        public SubMeasures(String module, long subMeasureId) : this(module) { SubMeasureId = subMeasureId; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module is required. ProgramId is fetched from Session, if not available please specify.
        /// MeasureId, ProgramIncentiveId may be specified.
        /// </summary>
        public DataSet GetAll(long programIncentiveId = 0)
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (MeasureId > 0) prms["MeasureID"] = MeasureId;
                if (programIncentiveId > 0) prms["ProgramIncentiveID"] = programIncentiveId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SubMeasures", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasures", "GetAll", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.SubMeasures", "GetAll", ex); return null; }
        }
        public DataSet GetAll4HD(long programIncentiveId = 0)
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                if (MeasureId > 0) prms["MeasureID"] = MeasureId;
                if (programIncentiveId > 0) prms["ProgramIncentiveID"] = programIncentiveId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SubMeasures4HD", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasures", "GetAll4HD", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.SubMeasures", "GetAll", ex); return null; }
        }

        /// <summary>
        /// Module, SubMeasureId are required. ProgramId, ByUserId are fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean GetById()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramSMIID"] = ProgramSMIID;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SubMeasure", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasures", "GetById", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.Program.SubMeasures", "GetById", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                MeasureId = MsSql.CheckLongDBNull(dr["MeasureID"]);
                MeasureName = MsSql.CheckStringDBNull(dr["MeasureName"]);
                SubMeasureId = MsSql.CheckLongDBNull(dr["SubMeasureID"]);
                SubMeasureName = MsSql.CheckStringDBNull(dr["SubMeasureName"]);
                kWh = MsSql.CheckDblDBNull(dr["kWh"]);
                kW = MsSql.CheckDblDBNull(dr["kW"]);
                TranDate = MsSql.CheckStringDBNull(dr["TranDate"]);
                ByUser = MsSql.CheckStringDBNull(dr["ByUser"]);
                StatusId = MsSql.CheckIntDBNull(dr["StatusID"]);
                SelfRebate = MsSql.CheckIntDBNull(dr["SelfRebate"]);
                SRUnitPerc = MsSql.CheckDblDBNull(dr["SRUnitPerc"]);
                SRUnitAmt = MsSql.CheckDblDBNull(dr["SRUnitAmt"]);
                SRHomeAmt = MsSql.CheckDblDBNull(dr["SRHomeAmt"]);
                ContractorRebate = MsSql.CheckIntDBNull(dr["ContractorRebate"]);
                CRUnitPerc = MsSql.CheckDblDBNull(dr["CRUnitPerc"]);
                CRUnitAmt = MsSql.CheckDblDBNull(dr["CRUnitAmt"]);
                CRHomeAmt = MsSql.CheckDblDBNull(dr["CRHomeAmt"]);
                LimitId = MsSql.CheckIntDBNull(dr["LimitID"]);
                FinAvail = MsSql.CheckIntDBNull(dr["FinanceAvail"]);
                SubMeasureType = MsSql.CheckIntDBNull(dr["SubMeasureType"]);
                CalculateSavings = MsSql.CheckIntDBNull(dr["CalculateSavings"]);
                ModifiedDate = MsSql.CheckStringDBNull(dr["ModifiedDate"]);
                return true;
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.SubMeasures", "GetById", ex); return false; }
        }

        /// <summary>
        /// Module, SubMeasureId are required. ProgramId, ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Save()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramID"] = ProgramId;
                prms["kWh"] = kWh;
                prms["kW"] = kW;
                prms["SelfRebate"] = SelfRebate;
                prms["SRUnitPerc"] = SRUnitPerc;
                prms["SRUnitAmt"] = SRUnitAmt;
                prms["SRHomeAmt"] = SRHomeAmt;
                prms["ContractorRebate"] = ContractorRebate;
                prms["CRUnitPerc"] = CRUnitPerc;
                prms["CRUnitAmt"] = CRUnitAmt;
                prms["CRHomeAmt"] = CRHomeAmt;
                prms["LimitID"] = LimitId;
                prms["FinanceAvail"] = FinAvail;
                prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;
                prms["ProgramSMIID"] = ProgramSMIID;
                prms["SubMeasureType"] = SubMeasureType;
                prms["CalculateSavings"] = CalculateSavings;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_SubMeasure", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasures", "Save", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error saving record.";
                    Common.Log.Info(Module + ":EDM.Program.SubMeasures", "Save", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["SubMeasureID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.SubMeasures", "Save", ex);
                return false;
            }
        }

        /// <summary>
        /// Module, MeasureId are required. ProgramId is fetched from Session, if unavailable, please specify.
        /// </summary>
        public DataSet GetAllByMeasureId()
        {
            String logParams = "ProgramID:" + ProgramId + "|MeasureId:" + MeasureId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }
                if (MeasureId <= 0) { Message = "MeasureId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["MeasureID"] = MeasureId;
                if (StatusId > 0) prms["StatusID"] = StatusId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SubMeasuresByMeasure", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasures", "GetAllByMeasureId", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.SubMeasures", "GetAllByMeasureId", ex, logParams);
                return null;
            }
        }

        public void ShowAtticSMs(ref Boolean showAttic, ref Boolean showKnee, ref Boolean showStatic, String[] upgrades)
        {
            try
            {
                MeasureId = Measure.AtticInsulation;
                StatusId = 1;
                DataSet ds = GetAllByMeasureId();

                Boolean atticOk = false, kneeOk = false, staticOk = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (MsSql.CheckLongDBNull(dr["SubMeasureID"]))
                    {
                        case SubMeasure.AtticInsulation:
                            atticOk = true;
                            break;
                        case SubMeasure.KneeWallInsulation:
                            kneeOk = true;
                            break;
                        case SubMeasure.StaticVentilation:
                            staticOk = true;
                            break;
                    }
                }
                if (!atticOk) showAttic = false;
                if (!kneeOk) showKnee = false;
                if (!staticOk) showStatic = false;

                foreach (String upgrade in upgrades)
                {
                    String[] vals = upgrade.Split('|');
                    switch (int.Parse(vals[2]))
                    {
                        case SubMeasure.AtticInsulation:
                            showAttic = false;
                            break;
                        case SubMeasure.KneeWallInsulation:
                            showKnee = false;
                            break;
                        case SubMeasure.StaticVentilation:
                            showStatic = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, "EDM.Program.SubMeasures", "ShowAtticSMs", ex);
                return;
            }
        }
        #endregion
    }
}
