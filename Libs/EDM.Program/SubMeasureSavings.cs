using System;
using System.Collections;
using System.Data;
using System.Xml;
using VTI.Common;

namespace EDM.Program
{
    public class SubMeasureSavings
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;

        public long MeasureId;
        public String MeasureName = String.Empty;
        public long SubMeasureId;      
        public String SubMeasureName = String.Empty;
        public int PrimaryHeatingType;
        public String HouseType = String.Empty;
        public String SqFootage = String.Empty;
        public DateTime? CalEffectiveBeginDate;
        public DateTime? CalEffectiveEndDate;
        public double kWh = 0;
        public double kW = 0;
        public double ccf = 0;
        public double Gallons;
        public double CO2;
        public long TreesPlanted;
        public long CarsOffTheRoad;
        public long MeasureLife;
        public int StatusId = 0;
        public int RowNum = 0;

        public String ByUser;
        private SqlDb Db;
        private EDM.Common.Log Lg;
        private String _configKey = String.Empty;
        public string Comments = String.Empty;
        public int IsAddMode = 0;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Common.Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Program";
            }
        }

       
        #endregion

        #region --- Constructors ---
        public SubMeasureSavings()
        {
            ProgramId = EDM.Setting.Session.ProgramId;
            ConfigKey = String.Empty;
            ByUserId = EDM.Setting.Session.UserId;
        }
        public SubMeasureSavings(String module) : this()
        {
            Module = module; ConfigKey = String.Empty;          
        }
        public SubMeasureSavings(String module, String configKey, long programId) : this(module)
        {
            ProgramId = programId;
            ConfigKey = configKey;           
        }
        #endregion

        public DataSet GetEffectiveSavingsDates()
        {
            String logParams = "ProgramID:" + ProgramId + "|MeasureId:" + MeasureId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }
                if (MeasureId <= 0) { Message = "MeasureId is required."; return null; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["MeasureID"] = MeasureId;
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramID"] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_EffectiveSavingsDates", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasureSavings", "GetEffectiveSavingsDates", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.SubMeasureSavings", "GetEffectiveSavingsDates", ex, logParams);
                return null;
            }
        }

        public DataSet GetAllSavingByMeasureAndSubMeasureId()
        {
            String logParams = "ProgramID:" + ProgramId + "|MeasureId:" + MeasureId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return null; }
                if (MeasureId <= 0) { Message = "MeasureId is required."; return null; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["MeasureID"] = MeasureId;
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramID"] = ProgramId;
                prms["IsAddMode"] = IsAddMode;
                
                if (CalEffectiveBeginDate != null) { prms["CalEffectiveBeginDate"] = CalEffectiveBeginDate; }
                if (CalEffectiveEndDate != null) { prms["CalEffectiveEndDate"] = CalEffectiveEndDate; }

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_ProgramSubmeasureSavings", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.SubMeasureSavings", "GetAllSavingByMeasureAndSubMeasureId", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.SubMeasureSavings", "GetAllSavingByMeasureAndSubMeasureId", ex, logParams);
                return null;
            }
        }

        public Boolean GetInitialEffectiveSavingsDates()
        {
            String logParams = "ProgramID:" + ProgramId + "|MeasureId:" + MeasureId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (MeasureId <= 0) { Message = "MeasureId is required."; return false; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["MeasureID"] = MeasureId;
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramID"] = ProgramId;

                Db.SetSql("p_GET_EffectiveSavingsDates", prms);
                Lg.Info("GetById", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = "Record not found";
                    Lg.Info("GetInitialEffectiveSavingsDates", Db.SqlStmt + "|" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];  
                CalEffectiveBeginDate = Convert.IsDBNull(dr[EDM.Setting.Fields.CalEffectiveBeginDate]) ? (DateTime?)null : Convert.ToDateTime(dr[EDM.Setting.Fields.CalEffectiveBeginDate]);
                CalEffectiveEndDate = Convert.IsDBNull(dr[EDM.Setting.Fields.CalEffectiveEndDate]) ? (DateTime?)null : Convert.ToDateTime(dr[EDM.Setting.Fields.CalEffectiveEndDate]);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.SubMeasureSavings", "GetInitialEffectiveSavingsDates", ex, logParams);
                return false;
            }
        }

        public bool UpdateSavingsBulk(DataTable dt_Updated)
        {
            String logParams = "";
            bool retValue = false;
            try
            {
                long XMLCounter = 0;

                XmlDocument xmlDoc = new XmlDocument();

                XmlElement xmlRoot = xmlDoc.CreateElement("Root");
                XmlElement SAVRootElement = xmlDoc.CreateElement("RootElement");

                foreach (DataRow objDataRow in dt_Updated.Rows)
                {
                    XmlElement SAVSource = xmlDoc.CreateElement("SAVSource");

                    XmlNode ID = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.ID, "");
                    ID.InnerText = XMLCounter.ToString();
                    SAVSource.AppendChild(ID);

                    long tblPSSID = MsSql.CheckLongDBNull(objDataRow[EDM.Setting.Fields.PSSID]);
                    XmlNode xmltblPSSID = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.PSSID, "");
                    xmltblPSSID.InnerText = tblPSSID.ToString();
                    SAVSource.AppendChild(xmltblPSSID);

                    long tblProgramID = MsSql.CheckLongDBNull(objDataRow[EDM.Setting.Fields.ProgramID]);
                    XmlNode xmltblProgramID = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.ProgramID, "");
                    xmltblProgramID.InnerText = tblProgramID.ToString();
                    SAVSource.AppendChild(xmltblProgramID);

                    long tblWOMeasureID = MsSql.CheckLongDBNull(objDataRow[EDM.Setting.Fields.MeasureID]);
                    XmlNode xmltblWOMeasureID = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.MeasureID, "");
                    xmltblWOMeasureID.InnerText = tblWOMeasureID.ToString();
                    SAVSource.AppendChild(xmltblWOMeasureID);             

                    long tblWOSubMeasureID = MsSql.CheckLongDBNull(objDataRow[EDM.Setting.Fields.SubMeasureID]);
                    XmlNode xmltblWOSubMeasureID = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.SubMeasureID, "");
                    xmltblWOSubMeasureID.InnerText = tblWOSubMeasureID.ToString();
                    SAVSource.AppendChild(xmltblWOSubMeasureID);

                    String tblCalEffectiveBeginDate = MsSql.CheckStringDBNull(objDataRow[EDM.Setting.Fields.CalEffectiveBeginDate]);
                    XmlNode xmltblCalEffectiveBeginDate = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.CalEffectiveBeginDate, "");
                    xmltblCalEffectiveBeginDate.InnerText = tblCalEffectiveBeginDate.ToString();
                    SAVSource.AppendChild(xmltblCalEffectiveBeginDate);

                    String tblCalEffectiveEndDate = MsSql.CheckStringDBNull(objDataRow[EDM.Setting.Fields.CalEffectiveEndDate]);
                    XmlNode xmltblCalEffectiveEndDate = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.CalEffectiveEndDate, "");
                    xmltblCalEffectiveEndDate.InnerText = tblCalEffectiveEndDate.ToString();
                    SAVSource.AppendChild(xmltblCalEffectiveEndDate);

                    XmlNode xmlkW = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.kW, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.kW]))
                        xmlkW.InnerText = objDataRow[EDM.Setting.Fields.kW].ToString();
                    SAVSource.AppendChild(xmlkW);

                    XmlNode xmlkWhSavings = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.kWhSavings, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.kWhSavings]))
                        xmlkWhSavings.InnerText = objDataRow[EDM.Setting.Fields.kWhSavings].ToString();
                    SAVSource.AppendChild(xmlkWhSavings);

                    XmlNode xmlkWhElectrification = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.kWhElectrification, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.kWhElectrification]))
                        xmlkWhElectrification.InnerText = objDataRow[EDM.Setting.Fields.kWhElectrification].ToString();
                    SAVSource.AppendChild(xmlkWhElectrification);

                    XmlNode xmlTherms = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.Therms, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.Therms]))
                        xmlTherms.InnerText = objDataRow[EDM.Setting.Fields.Therms].ToString();
                    SAVSource.AppendChild(xmlTherms);

                    XmlNode xmlGallons = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.Gallons, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.Gallons]))
                        xmlGallons.InnerText = objDataRow[EDM.Setting.Fields.Gallons].ToString();
                    SAVSource.AppendChild(xmlGallons);

                    XmlNode xmlCO2 = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.CO2, "");
                    if (!Convert.IsDBNull(objDataRow[EDM.Setting.Fields.CO2]))
                        xmlCO2.InnerText = objDataRow[EDM.Setting.Fields.CO2].ToString();
                    SAVSource.AppendChild(xmlCO2);

                    String tblTrees = MsSql.CheckStringDBNull(objDataRow[EDM.Setting.Fields.Trees]);
                    XmlNode xmltblTrees = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.Trees, "");
                    xmltblTrees.InnerText = tblTrees.ToString();
                    SAVSource.AppendChild(xmltblTrees);

                    String tblCarsOffTheRoad = MsSql.CheckStringDBNull(objDataRow[EDM.Setting.Fields.CarsOffTheRoad]);
                    XmlNode xmltblCarsOffTheRoad = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.CarsOffTheRoad, "");
                    xmltblCarsOffTheRoad.InnerText = tblCarsOffTheRoad.ToString();
                    SAVSource.AppendChild(xmltblCarsOffTheRoad);

                    String tblMeasureLife = MsSql.CheckStringDBNull(objDataRow[EDM.Setting.Fields.MeasureLife]);
                    XmlNode xmltblMeasureLife = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.MeasureLife, "");
                    xmltblMeasureLife.InnerText = tblMeasureLife.ToString();
                    SAVSource.AppendChild(xmltblMeasureLife);

                    long tblPheatingType = MsSql.CheckIntDBNull(objDataRow[EDM.Setting.Fields.PrimaryHeatingType]);
                    XmlNode xmltblPheatingType = xmlDoc.CreateNode(XmlNodeType.Element, EDM.Setting.Fields.PrimaryHeatingType, "");
                    xmltblPheatingType.InnerText = tblPheatingType.ToString();
                    SAVSource.AppendChild(xmltblPheatingType);

                    SAVRootElement.AppendChild(SAVSource);
                    XMLCounter++;
                }

                xmlRoot.AppendChild(SAVRootElement);
                xmlDoc.AppendChild(xmlRoot);
                retValue = UpdateInBulk(xmlDoc.OuterXml);
            }
            catch (Exception ex)
            {
                  Common.Log.Error(Module, Module + ":EDM.Program.SubMeasureSavings", "UpdateSavingsBulk", ex, logParams);
            }
            return retValue;
        }

        private Boolean UpdateInBulk(string xmlString)
        {
            String logParams = "|xmlString:" + xmlString + "|ByUserId:" + ByUserId;
            try
            {
                if (xmlString.Length <= 0) { Message = "XmlString is required."; return false; }

                Hashtable prms = new Hashtable();
                prms[EDM.Setting.Fields.XmlString] = xmlString;
                prms[EDM.Setting.Fields.ByUserID] = ByUserId;

                Db.SetSql("p_U_SavingsDetailsInBulk", prms);
                Lg.Debug("UpdateInBulk", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = logParams + "|Error updating record.";
                    Lg.Info("UpdateInBulk", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["tblPSSID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Lg.Error("UpdateInBulk", ex, logParams);
                return false;
            }
        }

        public Boolean ValidateSavingDate()
        {
            String logParams = "ProgramID:" + ProgramId + "|MeasureId:" + MeasureId + "|SubMeasureId:" + SubMeasureId;
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (MeasureId <= 0) { Message = "MeasureId is required."; return false; }
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return false; }
                if (CalEffectiveBeginDate == null) { Message = "CalEffectiveBeginDate is required."; return false; }
                if (CalEffectiveEndDate == null) { Message = "CalEffectiveEndDate is required."; return false; }
               

                Hashtable prms = new Hashtable();
                prms["MeasureID"] = MeasureId;
                prms["SubMeasureID"] = SubMeasureId;
                prms["ProgramID"] = ProgramId;
                prms["CalEffectiveBeginDate"] = CalEffectiveBeginDate;
                prms["CalEffectiveEndDate"] = CalEffectiveEndDate;
                prms["IsAddMode"] = IsAddMode;
                prms["RowNum"] = RowNum;

                Db.SetSql("p_CHK_ValidSavingsDate", prms);
                Lg.Debug("ValidateSavingDate", Db.SqlStmt);

                DataSet ds = Db.ExecuteQuery();
                if (SqlDb.IsEmpty(ds))
                {
                    Message = logParams + "|Error updating record.";
                    Lg.Info("ValidateSavingDate", Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["IsValid"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = logParams + "|Error:" + ex.Message;
                Lg.Error("ValidateSavingDate", ex, logParams);
                return false;
            }
        }
    }
}
