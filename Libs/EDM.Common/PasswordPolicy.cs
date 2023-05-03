using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VTI.Common;

namespace EDM.Common
{
    public class PasswordPolicy
    {
        #region --- Members ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId = 0;

        private EDM.Common.Log Lg;
        private SqlDb Db;
        private String _configKey = String.Empty;
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
                Lg = new Log(_configKey);
                Lg.ModuleName = Module + ":EDM.Common.PasswordPolicy";
            }
        }

        public int MinimumLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireSpecialCharacter { get; set; }
        public String SpecialCharacters { get; set; }
        public int ExpirationDays { get; set; }
        #endregion

        #region --- Constructors ---
        public PasswordPolicy() { ProgramId = EDM.Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; ConfigKey = String.Empty; Init(); }
        public PasswordPolicy(String module) : this() { Module = module; ConfigKey = String.Empty; }

        public void Init(String configKey = "")
        {
            ConfigKey = configKey;
            Db = new SqlDb(ConfigKey);

            EDM.Setting.DB stg = new EDM.Setting.DB(ConfigKey, ProgramId);
            MinimumLength = Convert.ToInt32(stg.GetByKey(EDM.Setting.Key.PasswordPolicy_MinimumLength));
            RequireUppercase = stg.GetByKey(EDM.Setting.Key.PasswordPolicy_RequireUppercase) == "1" ? true : false;
            RequireLowercase = stg.GetByKey(EDM.Setting.Key.PasswordPolicy_RequireLowercase) == "1" ? true : false;
            RequireDigit = stg.GetByKey(EDM.Setting.Key.PasswordPolicy_RequireDigit) == "1" ? true : false;
            RequireSpecialCharacter = stg.GetByKey(EDM.Setting.Key.PasswordPolicy_RequireSpecialCharacters) == "1" ? true : false;
            SpecialCharacters = Convert.ToString(stg.GetByKey(EDM.Setting.Key.PasswordPolicy_SpecialCharacters));
            ExpirationDays = Convert.ToInt32(stg.GetByKey(EDM.Setting.Key.PasswordPolicy_ExpirationDays));
        }
        #endregion

        #region --- Methods ---
        public bool IsValidPassword(string password)
        {
            if (!HasMinimumLength(password, MinimumLength)) return false;
            if (RequireUppercase && !HasUpperCaseLetter(password)) return false;
            if (RequireLowercase && !HasLowerCaseLetter(password)) return false;
            if (RequireDigit && !HasDigit(password)) return false;
            if (RequireSpecialCharacter && !HasSpecialChar(password)) return false;
            return true;
        }
        private bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one uppercase letter
        /// </summary>
        private bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one lowercase letter
        /// </summary>
        private bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one digit
        /// </summary>
        private bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one special character
        /// </summary>
        private bool HasSpecialChar(string password)
        {
            //return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
             return TestPassword(password, SpecialCharacters);
        }

        public string GetDisplayMessage()
        {
            string displayMessage = String.Empty;
            if (MinimumLength > 0)
            {
                displayMessage = "(Password must contain at least " + MinimumLength.ToString() + " or more characters";

                if ((RequireUppercase || RequireLowercase || RequireSpecialCharacter || RequireDigit)
                    && !displayMessage.Contains(", including at least one"))
                {
                    displayMessage = displayMessage + ", including at least one";
                }

                if (RequireUppercase)
                {
                    displayMessage = displayMessage + " uppercase,";
                }

                if (RequireLowercase)
                {
                    displayMessage = displayMessage + " lowercase,";
                }
                displayMessage = displayMessage.EndsWith(",") ? displayMessage.Substring(0, displayMessage.Length - 1) : displayMessage;

                if (!RequireSpecialCharacter && !RequireDigit)
                {
                    if (RequireUppercase && RequireLowercase)
                    {
                        displayMessage = displayMessage.Replace(" uppercase, lowercase", " uppercase and lowercase");
                    }
                    displayMessage = displayMessage + " character.";
                }
                else if (!RequireSpecialCharacter && RequireDigit)
                {
                    displayMessage = displayMessage + (displayMessage.EndsWith("at least one") ? " number." : " and number.");
                }
                else if (RequireSpecialCharacter && !RequireDigit)
                {
                    displayMessage = displayMessage + (displayMessage.EndsWith("at least one") ? " special character." : " and special character.");
                }
                else if (RequireSpecialCharacter && RequireDigit)
                {
                    displayMessage = displayMessage + (displayMessage.EndsWith("at least one") ? " special character and number." : ", special character and number.");
                }

                displayMessage = displayMessage + ")";
            }
            return displayMessage;
        }

        public bool IsPasswordExpired(String LastPasswordChangeDate)
        {
            bool result = false;
            try
            {
                DateTime objLastPassChangeDate = Convert.ToDateTime(LastPasswordChangeDate, new System.Globalization.CultureInfo("en-US"));
                if (ExpirationDays <= 0) { result = false; }
                else if(objLastPassChangeDate.AddDays(ExpirationDays).Date < DateTime.Now.Date)
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Module, Module + ":EDM.Common.PasswordPolicy", "IsPasswordExpired", ex);
                result = false;
            }
            return result;
        }

        private bool TestPassword(string password, string allowedSpecialCharacter)
        {
            bool result = true;
            Regex specialCh = new Regex("^[" + allowedSpecialCharacter + "]*$", RegexOptions.Compiled);
            Char[] ca = password.ToCharArray();
            foreach (Char c in ca)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    //Console.WriteLine("found symbol:{0} ", c);
                    if (!specialCh.IsMatch(c.ToString()))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
