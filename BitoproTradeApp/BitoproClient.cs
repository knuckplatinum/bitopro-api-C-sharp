using System;
using System.Text;
using System.Net.Http;


namespace BitoproApp
{
    public class BitoproClinet
    {
        private const string baseUrl = "https://api.bitopro.com/v2";
        private const string _apikey = "X-BITOPRO-APIKEY";
        private const string _payload = "X-BITOPRO-PAYLOAD";
        private const string _signature = "X-BITOPRO-SIGNATURE";

        //#authenticated
        //getAccountBalance  /accounts/balance
        //getOrderHistory    /orders/history
        //getOrders          /orders/{pair}?page=int &active=boolean
        //createOrder        /orders/{pair}    need post body
        //cancelOrder        /orders/{pair}/{id}
        //getOrderStatus     /orders/{pair}/{orderId}

        //#public
        //getOrderBookByPair /order-book/{pair}
        //getTickerByPair    /ticker/{pair}
        //getTickers         /tickers/{pair} <-optional
        //getPairTrades      /trades/{pair}

        #region Public Method

        public string getOrderBookByPair(string pair)
        {
            return normalRequestMethod("/order-book/", pair);
        }

        public string getTickerByPair(string pair)
        {
            return normalRequestMethod("/ticker/", pair);
        }

        public string getTickers(string pair = "")
        {
            return normalRequestMethod("/tickers/", pair);
        }

        public string getPairTrades(string pair = "")
        {
            return normalRequestMethod("/trades/", pair);
        }

        #endregion

        #region Authenticated Method

        public string getAccountBalance(string apikey, string payload, string signature)
        {
            
            return verifyRequestMethod(HttpMethod.Get, "/accounts/balance", apikey, payload, signature, "", "");
        }

        public string getOrderHistory(string apikey, string payload, string signature)
        {
            return verifyRequestMethod(HttpMethod.Get, "/orders/history", apikey, payload, signature, "", "");
        }

        public string getOrders(string apikey, string payload, string signature, string pair, int page = 1, bool active = true)
        {
            return verifyRequestMethod(HttpMethod.Get, "/orders/", apikey, payload, signature, pair, "?page=" + page + "&active" + active);
        }

        public string createOrder(string apikey, string payload, string signature, string pair, string body)
        {
            return verifyRequestMethod(HttpMethod.Post, "/orders/", apikey, payload, signature, pair, "", body);
        }

        public string cancelOrder(string apikey, string payload, string signature, string pair, string id)
        {
            return verifyRequestMethod(HttpMethod.Delete, "/orders/", apikey, payload, signature, pair, "/" + id);
        }

        public string getOrderStatus(string apikey, string payload, string signature, string pair, string id)
        {
            return verifyRequestMethod(HttpMethod.Get, "/orders/", apikey, payload, signature, pair, "/" + id);
        }
        #endregion 


        public string verifyRequestMethod(HttpMethod method, string url, string apiKey, string payLoad, string Signature, string pairs, string parameters, string body = "")
        {
            string r = "";
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(method, baseUrl + url + pairs + parameters);

                if (!String.IsNullOrEmpty(body))
                {
                    requestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                requestMessage.Headers.Add(_apikey, apiKey);
                if (method != HttpMethod.Delete)
                {
                    requestMessage.Headers.Add(_payload, payLoad);
                }
                requestMessage.Headers.Add(_signature, Signature);

                HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

                if (response.StatusCode.ToString() == "OK")
                {
                    r = response.Content.ReadAsStringAsync().Result.ToString();
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            return r;

        }

        private string normalRequestMethod(string url, string pair)
        {
            string r = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, baseUrl + url + pair))
                {
                    using (HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult())
                    {
                        if (response.StatusCode.ToString() == "OK")
                        {
                            r = response.Content.ReadAsStringAsync().Result.ToString();
                        }
                        else
                        {
                            Console.WriteLine("Error");
                        }
                    }
                }
            }
            return r;
        }
    }



}