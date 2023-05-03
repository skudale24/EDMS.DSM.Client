using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Setting
{
    public class DotNetCharge
    {
        #region --- Properties ---
        public String Module;
        public String Message;

        public long ProgramId;
        public long ByUserId;

        public String StoreName = String.Empty;
        public String StoreNumber = String.Empty;
        public String SecretKey = String.Empty;
        public String LoginID = String.Empty;
        public String Password = String.Empty;
        public String Mode = String.Empty;

        public String MerchantId = String.Empty;
        public String PublicKey = String.Empty;
        public String PrivateKey = String.Empty;
        public String BraintreeMode = String.Empty;

        public String CreditCardProcessGateway = String.Empty;
        #endregion

        #region --- Constructors ---
        public DotNetCharge() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public DotNetCharge(String module) : this() { Module = module; }
        #endregion

        #region --- Methods ---
        /// <summary>
        /// Module, StoreName, StoreNumber, SecretKey, LoginID, Password, Mode are required. ProgramId, ByUserId are fetched 
        /// from Session, if not available, please specify.
        /// </summary>
        public Boolean Save()
        {
            String logParams = "ProgramId:" + ProgramId + "|StoreName:" + StoreName + "|StoreNumber:" + StoreNumber
                + "|SecretKey:" + SecretKey + "|LoginID:" + LoginID + "|Mode:" + Mode + "|ByUserId:" + ByUserId;
            try
            {
                Hashtable prms = new Hashtable();
                prms[Fields.ProgramID] = ProgramId;
                prms[Fields.StoreName] = StoreName;
                prms[Fields.StoreNumber] = StoreNumber;
                prms[Fields.SecretKey] = SecretKey;
                prms[Fields.LoginID] = LoginID;
                prms[Fields.Password] = Password;
                prms[Fields.Mode] = Mode;
                prms[Fields.MerchantID] = MerchantId;
                prms[Fields.PublicKey] = PublicKey;
                prms[Fields.PrivateKey] = PrivateKey;
                prms[Fields.BraintreeMode] = BraintreeMode;
                prms[Fields.CCProcessGateway] = CreditCardProcessGateway;
                prms[Fields.ByUserID] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_U_SettingDotNetCharge", prms, out SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error updating records.";
                    return false;
                }

                return MsSql.CheckLongDBNull(ds.Tables[0].Rows[0]["PaymentSettingID"]) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Module is required. ProgramId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean GetById()
        {
            String logParams = "ProgramId:" + ProgramId;
            try
            {
                Hashtable prms = new Hashtable();
                prms[Fields.ProgramID] = ProgramId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_SettingDotNetCharge", prms, out SqlforLog);

                DataSet ds = MsSql.ExecuteNoTransQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching records.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                StoreName = MsSql.CheckStringDBNull(dr[Fields.StoreName]);
                StoreNumber = MsSql.CheckStringDBNull(dr[Fields.StoreNumber]);
                SecretKey = MsSql.CheckStringDBNull(dr[Fields.SecretKey]);
                LoginID = MsSql.CheckStringDBNull(dr[Fields.LoginID]);
                Password = MsSql.CheckStringDBNull(dr[Fields.Password]);
                Mode = MsSql.CheckStringDBNull(dr[Fields.Mode]);

                MerchantId = MsSql.CheckStringDBNull(dr[Fields.MerchantID]);
                PublicKey = MsSql.CheckStringDBNull(dr[Fields.PublicKey]);
                PrivateKey = MsSql.CheckStringDBNull(dr[Fields.PrivateKey]);
                BraintreeMode = MsSql.CheckStringDBNull(dr[Fields.BraintreeMode]);

                CreditCardProcessGateway = MsSql.CheckStringDBNull(dr[Fields.CCProcessGateway]);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
        #endregion
    }
}
