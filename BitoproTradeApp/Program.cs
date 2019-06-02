using System;
using Newtonsoft.Json;

namespace BitoproApp
{
    class Program
    {

        static void Main(string[] args)
        {
            string apikey = "";
            string apiSecret = "";
            string email = "";

            //初始化編碼用物件
            StringEncrypt se = new StringEncrypt(apikey, apiSecret, email);

            BitoproClinet client = new BitoproClinet();

            string json1 = getOrderJsonBody("buy", "limit", 0.01, 1000);
            se.setPayload(json1);

            //下訂單
            string result = client.createOrder(apikey, se.Payload(), se.Signature(), "bito_twd", json1);
        }

        public static string getOrderJsonBody(string action, string type, double price, double amount)
        {
            string _price = price.ToString();
            string _amount = amount.ToString();
            var j = new
            {
                action = action,
                type = type,
                price = _price,
                amount = _amount,
                timestamp = getnonce()
            };
            return JsonConvert.SerializeObject(j);
        }
        private static long getnonce()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 當地時區
            return (long)(DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
