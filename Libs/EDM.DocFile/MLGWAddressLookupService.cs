using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VTI.Common;   

namespace EDM.DocFile
{
    public enum StatusType : int
    {
        NoBill = -6,
        EmptyCustomer = -5,
        MultiCustomer = -4,
        NoCustomer = -3,
        InvalidXML = -2,
        EmptyData = -1,
        Success = 0,
        MLGWError = 1
    }
    public class MLGWAddressLookupService
    {
        #region --- Members ---
        public String ConfigKey = String.Empty;
        public long ProgramId;
        public String Module;
        public long ByUserId;

        public SqlDb Db;
        public EDM.Common.Log Lg;
        #endregion

        #region --- Constructors ---
        public MLGWAddressLookupService() { Init(); }
        public MLGWAddressLookupService(String configKey, long programId) { ProgramId = programId; Init(configKey); }
        public MLGWAddressLookupService(String module) : this() { Module = module; }
        public MLGWAddressLookupService(String module, String configKey, long programId) : this(module) { ProgramId = programId; Init(configKey); }
        public MLGWAddressLookupService(String module, String configKey, long programId, long byUserId) : this(module, configKey, programId) { }
        #endregion

        #region --- Public Methods ---
        public EDM.Common.MethodReturn ProcessMLGWAddressLookupService()
        {
            EDM.Common.MethodReturn mr = new EDM.Common.MethodReturn();
            try
            {
                string Message = string.Empty;    
                //Hashtable prms = new Hashtable();
                //Db.SetSql("p_UT_GET_ServiceAddress4Lookup", prms);
                //Lg.Info("ProcessMLGWAddressLookupService", Db.SqlStmt);
                //DataSet ds = Db.ExecuteNoTransQuery();
                //if (SqlDb.IsEmpty(ds))
                //{
                //    Message = "No service addresses to lookup.";
                //    Lg.Info("ProcessMLGWAddressLookupService", Message);                                            
                //}
                //else
                //{  
                //    String saId, acNo, addr;
                //    String errs = String.Empty;
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        saId = SqlDb.CheckStringDBNull(dr["ServiceAddressID"]);
                //        acNo = SqlDb.CheckStringDBNull(dr["AccountNumber"]);
                //        addr = SqlDb.CheckStringDBNull(dr["Address1"]);

                //        String logParams = "ServiceAddressId:" + saId + "|AccountNumber:" + acNo + "|Address1:" + addr;

                //        if (String.IsNullOrEmpty(acNo) && String.IsNullOrEmpty(addr))
                //        {
                //            Lg.Info("ProcessMLGWAddressLookupService", logParams + "|Both blank, not processing.");
                //            continue;
                //        }

                //        Lg.Info("ProcessMLGWAddressLookupService", logParams + "|Going to process...");

                //        String text = String.Empty;
                //        String ret = ExecMLGWLookUp(Lg, acNo, addr, ref text);
                //        if (!String.IsNullOrEmpty(ret))
                //        {
                //            if (!String.IsNullOrEmpty(errs)) errs += ",";
                //            errs += saId + ":" + ret;
                //            SaveMLGWLookupLog(Db, Lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.MLGWError, ret);
                //        }
                //        else
                //        {
                //            ProcessXML(Db, Lg, saId, acNo, addr, text);
                //        }
                //    }
                   
                //    if (!String.IsNullOrEmpty(errs))
                //    {
                //        Message += "Error processing " + errs;
                //        Lg.Info("ProcessMLGWAddressLookupService", Message);
                //    }
                //    else
                //    {
                //        Message = SqlDb.GetRowCount(ds) + " records processed.";                        
                //    }
                //}                
                mr.Message = Message;
                mr.Status = true;
                return mr;
            }
            catch (Exception ex)
            {
                mr.Status = false;
                mr.Message = ex.Message;
                Lg.Error("ProcessMLGWAddressLookupService", new Exception(ex.Message));
                return mr;
            }
        }
        #endregion

        #region --- Private Methods ---
        private void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);
            Lg = new EDM.Common.Log(ConfigKey);
            Lg.ModuleName = configKey + ".Service.MLGWAddressLookupService";
        }
     
        private static String ExecMLGWLookUp(EDM.Common.Log lg, String accountNumber, String address, ref String output)
        {
            String logParams = "accountNumber:" + accountNumber + "|address:" + address;
            try
            {
                MLGWAddrLookup.MLGWSoapClient obj = new MLGWAddrLookup.MLGWSoapClient("MLGWSoap");
                output = obj.lookUpAddress(accountNumber, address);
                if (String.IsNullOrEmpty(output)) return logParams + "|No response from MLGW Service";
                return String.Empty;
            }
            catch (Exception ex)
            {
                lg.Error("ExecMLGWLookUp", ex, logParams);
                return logParams + "|" + ex.Message;
            }
        }

        private static void ProcessXML(SqlDb db, EDM.Common.Log lg, String saId, String acNo, String addr, String text)
        {
            String logParams = "saId:" + saId + "|acNo:" + acNo + "|addr:" + addr + "|text:" + text;
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.EmptyData, "MLGW returned empty data");
                    lg.Error("ProcessXML", new Exception("Utility lookup returned no data"), logParams);
                    AddCustomerBilling(db, lg, Convert.ToInt64(saId));
                    return;
                }
                if (!text.StartsWith("<"))
                {
                    SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.InvalidXML
                        , "MLGW returned invalid XML [" + text + "]");
                    lg.Error("ProcessXML", new Exception("Utility lookup returned error"), logParams);
                    AddCustomerBilling(db, lg, Convert.ToInt64(saId));
                    return;
                }

                XDocument xml = XDocument.Load(new StreamReader(ConvertStringToStream(lg, text)));
                if (xml.Descendants("Customers").Descendants("Customer").Count() <= 0)
                {
                    SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.NoCustomer
                        , "MLGW returned no customer records");
                    lg.Info("ProcessXML", logParams + "|No Customer records");
                    AddCustomerBilling(db, lg, Convert.ToInt64(saId));
                    return;
                }
                if (xml.Descendants("Customers").Descendants("Customer").Count() > 1)
                {
                    SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.MultiCustomer
                        , "MLGW returned multiple customer records, cannot choose");
                    lg.Info("ProcessXML", logParams + "|More than 1 customer record, cannot chose, not doing anything...");
                    return;
                }

                var customers = from customerRecord in xml.Descendants("Customers").Elements("Customer")
                                select customerRecord;
                foreach (var cust in customers)
                {
                    if (cust.Elements().Count() <= 0)
                    {
                        SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.EmptyCustomer
                            , "MLGW returned empty customer record");
                        lg.Info("ProcessXML", logParams + "|Only one empty Customer record");
                        AddCustomerBilling(db, lg, Convert.ToInt64(saId));
                        return;
                    }

                    String startDt = String.Empty, endDt = String.Empty, currDt = String.Empty;
                    double billAmt = 0;
                    int count = 0;
                    var bills = from billRecords in cust.Elements("BillingHistory").Elements("Bills").Elements("Bill")
                                select billRecords;
                    foreach (var custbill in bills)
                    {
                        try
                        {
                            if (bills.Elements().Count() == 1 && custbill.Element("UnitOfMeasurement") == null)
                                break;

                            //String unit = custbill.Element("UnitOfMeasurement").Value;
                            //if (unit.ToLower() == "kwh")
                            //{
                            currDt = custbill.Element("BillingDate").Value;
                            if (startDt.Length <= 0) startDt = currDt;
                            if (endDt.Length <= 0) endDt = currDt;
                            if (Convert.ToDateTime(startDt) > Convert.ToDateTime(currDt)) startDt = currDt;
                            if (Convert.ToDateTime(endDt) < Convert.ToDateTime(currDt)) endDt = currDt;

                            String amount = custbill.Element("Amount").Value;
                            if (amount.Length > 0) billAmt += Convert.ToDouble(amount);
                            count++;
                            //}
                        }
                        catch (Exception ex)
                        {
                            lg.Error("ProcessXML:for Loop", ex, logParams + "|custbill:" + custbill.ToString());
                        }
                    }

                    if (count > 0)
                    {
                        billAmt /= count;
                        AddCustomerBilling(db, lg, Convert.ToInt64(saId), Convert.ToDouble(billAmt), startDt, endDt);
                        SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.Success
                            , "Billing Amount: " + billAmt + " from " + startDt + " to " + endDt);
                    }
                    else
                    {
                        SaveMLGWLookupLog(db, lg, Convert.ToInt64(saId), acNo, addr, (int)StatusType.NoBill, "MLGW returned 0 bill records");
                        lg.Info("ProcessXML", logParams + "|0 bill records.");
                    }
                }
            }
            catch (Exception ex)
            {
                lg.Error("ProcessXML", ex, logParams + "|text:" + text);
            }
        }

        private static Stream ConvertStringToStream(EDM.Common.Log lg, String s)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                lg.Error("ConvertStringToStream", ex);
                return null;
            }
        }

        private static Boolean AddCustomerBilling(SqlDb db, EDM.Common.Log lg, long ServiceAddressId)
        {
            Hashtable prms = new Hashtable();
            prms["ServiceAddressID"] = ServiceAddressId;

            db.SetSql("p_UT_AU_CustomerBillingInfo", prms);
            lg.Debug("AddCustomerBilling", db.SqlStmt);
            DataSet ds = db.ExecuteQuery();
            if (SqlDb.IsEmpty(ds)) return false;

            DataRow dr = ds.Tables[0].Rows[0];
            return SqlDb.CheckLongDBNull(dr["ServiceAddressID"]) > 0 ? true : false;
        }

        private static Boolean AddCustomerBilling(SqlDb db, EDM.Common.Log lg, long ServiceAddressId, Double AvgBillAmt, String StartDate, String EndDate)
        {
            Hashtable prms = new Hashtable();
            prms["ServiceAddressID"] = ServiceAddressId;
            prms["AvgBillAmt"] = AvgBillAmt;
            prms["StartDate"] = StartDate;
            prms["EndDate"] = EndDate;

            db.SetSql("p_UT_AU_CustomerBillingInfo", prms);
            lg.Debug("AddCustomerBilling", db.SqlStmt);
            DataSet ds = db.ExecuteQuery();
            if (SqlDb.IsEmpty(ds)) return false;

            DataRow dr = ds.Tables[0].Rows[0];
            return SqlDb.CheckLongDBNull(dr["ServiceAddressID"]) > 0 ? true : false;
        }

        private static Boolean SaveMLGWLookupLog(SqlDb db, EDM.Common.Log lg, long ServiceAddressId, String AccountNumber, String Address1, int StatusId, String Status)
        {
            Hashtable prms = new Hashtable();
            prms["ServiceAddressID"] = ServiceAddressId;
            prms["AccountNumber"] = AccountNumber;
            prms["Address1"] = Address1;
            prms["StatusID"] = StatusId;
            prms["Status"] = Status;

            db.SetSql("p_UT_AU_MLGWLookupLog", prms);
            //lg.Debug("SaveMLGWLookupLog", db.SqlStmt);
            DataSet ds = db.ExecuteQuery();
            if (SqlDb.IsEmpty(ds)) return false;

            DataRow dr = ds.Tables[0].Rows[0];
            return SqlDb.CheckLongDBNull(dr["LogID"]) > 0 ? true : false;
        }

        #endregion --- Private Methods ----
    }
}
