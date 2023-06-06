using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace EDMS.DSM.Server.Security
{
    public class SecurityToken
    {
        public SecurityToken(double expireTimeStamp, string aspNetUserId, long orgId, string emailId)
        {
            AspNetUserId = aspNetUserId;
            ExpireTimeStamp = expireTimeStamp;
            OrgId = orgId;
            EmailId = emailId;
        }

        public string AspNetUserId { get; }
        public double ExpireTimeStamp { get; }
        private string EmailId { get; }
        private long OrgId { get; }


        private const string _encryptionKey = "27d35be88c4841f795698299b03ec874";
        public string Encryption()
        {
            string text = ToString();
            string key = _encryptionKey;
            byte[] _key = Encoding.UTF8.GetBytes(key);

            using (Aes aes = Aes.Create())
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor(_key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(text);
                            }
                        }

                        byte[] iv = aes.IV;
                        byte[] encrypted = ms.ToArray();
                        byte[] result = new byte[iv.Length + encrypted.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);
                        return Base64UrlTextEncoder.Encode(result);// Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static SecurityToken Decrypt(string encrypted)
        {
            byte[] b = Base64UrlTextEncoder.Decode(encrypted); //Convert.FromBase64String(encrypted);
            string key = _encryptionKey;
            byte[] iv = new byte[16];
            byte[] cipher = new byte[b.Length - iv.Length];
            ;
            Buffer.BlockCopy(b, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(b, iv.Length, cipher, 0, b.Length - iv.Length);
            byte[] _key = Encoding.UTF8.GetBytes(key);
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(_key, iv))
                {
                    string result = string.Empty;
                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                    }

                    result = result.Replace("\r", "");
                    string[] str = result.Split(';');
                    Dictionary<string, string> dictionary = str.Where((t, iCount) => iCount != str.Length).Select(t => t.Split('='))
                        .ToDictionary(strValue => strValue[0], strValue => strValue[1]);
                    return new SecurityToken(Convert.ToDouble(dictionary["TS"]), dictionary["AUID"], long.Parse(dictionary["OD"]), dictionary["EID"]);
                }
            }
        }
        public override string ToString()
        {
            return $"TS={ExpireTimeStamp.ToString(CultureInfo.InvariantCulture)};AUID={AspNetUserId};OD={OrgId.ToString()};EID={EmailId}";
        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks);
        }

        public static RefreshToken CreateRefreshToken(int expiryInDays = 7)
        {
            byte[] random = new byte[100];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(random);
                var refreshToken = new RefreshToken
                {
                    Token = Base64UrlTextEncoder.Encode(random),//Convert.ToBase64String(random),
                    Expires = DateTime.UtcNow.AddDays(expiryInDays),
                    Created = DateTime.UtcNow
                };
                return refreshToken;
            }
        }
    }
}