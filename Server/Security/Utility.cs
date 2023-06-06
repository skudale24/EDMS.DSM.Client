using System.Text;

namespace EDMS.DSM.Server.Security
{
    public class Utility
    {
        public enum TokenType
        {
            Login,
            AccountVerification
        }


        public static string EncodeString(string toEncode)
        {
            try
            {
                if (toEncode == null) return string.Empty;

                var toEncodeAsBytes = Encoding.UTF8.GetBytes(toEncode);


                var returnValue = Convert.ToBase64String(toEncodeAsBytes);


                return returnValue;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Decodes the string.
        /// </summary>
        /// <param name="encodedData">The encoded data.</param>
        /// <returns></returns>
        public static string DecodeString(string encodedData)
        {
            try
            {
                if (encodedData == null) return string.Empty;

                //var CryptedData = EncryptStringToBytes_Aes(encodedData, );

                var encodedDataAsBytes
                    = Convert.FromBase64String(encodedData);

                var returnValue =
                    Encoding.UTF8.GetString(encodedDataAsBytes);

                returnValue = returnValue.Replace("\0", "");

                return returnValue;
            }
            catch
            {
                return null;
            }
        }
    }
}
