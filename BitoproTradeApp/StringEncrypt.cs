using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BitoproApp
{
    public class StringEncrypt
    {
        private string apikey = "";
        private string apisecret = "";
        private string email = "";
        private string payload = "";

        /// <summary>
        /// 初始固定參數
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="apisecret"></param>
        /// <param name="email"></param>
        public StringEncrypt(string apikey, string apisecret, string email)
        {
            this.apikey = apikey;
            this.apisecret = apisecret;
            this.email = email;
        }

        public void setPayload()
        {
            this.payload = Base64(getDefaultJsonBody());
        }

        public void setPayload(string jsonbody)
        {
            this.payload = Base64(jsonbody);
        }

        public string Payload()
        {
            return this.payload;
        }

        public string Signature()
        {
            return HmacSHA384(this.payload, this.apisecret);
        }

        private string getDefaultJsonBody()
        {
            var j = new
            {
                identity = this.email,
                nonce = getnonce()
            };

            return JsonConvert.SerializeObject(j);
        }

        private long getnonce()
        {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 當地時區
            return (long)(DateTime.Now - startTime).TotalMilliseconds;
        }

        #region Encrypt Method
        private static string SHA256Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));

            StringBuilder rst = new StringBuilder();
            for (int i = 0; i < tmpByte.Length; i++)
            {
                rst.Append(tmpByte[i].ToString("x2"));
            }
            sha256.Clear();
            return rst.ToString();
            //return GetStringValue(tmpByte);
        }
        private static string SHA384Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA384 sha384 = new SHA384Managed();
            tmpByte = sha384.ComputeHash(GetKeyByteArray(strIN));

            StringBuilder rst = new StringBuilder();
            for (int i = 0; i < tmpByte.Length; i++)
            {
                rst.Append(tmpByte[i].ToString("x2"));
            }
            sha384.Clear();
            return rst.ToString();

        }
        private static string GetStringValue(byte[] Byte)
        {
            string tmpString = "";
            UTF8Encoding Asc = new UTF8Encoding();
            tmpString = Asc.GetString(Byte);
            return tmpString;
        }
        private static byte[] GetKeyByteArray(string strKey)
        {
            UTF8Encoding Asc = new UTF8Encoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = Asc.GetBytes(strKey);
            return tmpByte;
        }
        private static string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        private static string HmacSHA384(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha384 = new HMACSHA384(keyByte))
            {
                byte[] hashmessage = hmacsha384.ComputeHash(messageBytes);
                return ByteToString(hashmessage);
            }
        }

        private static string Base64(string message)
        {
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
            byte[] bytedata = encode.GetBytes(message);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            return strPath;
        }
        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("x2"); // hex format
            }
            return (sbinary);
        }
        #endregion
    }

}