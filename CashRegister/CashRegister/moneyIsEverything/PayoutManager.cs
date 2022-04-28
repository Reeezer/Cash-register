using CashRegister.moneyIsEverything.models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;

namespace CashRegister.moneyIsEverything
{
    // ICI
    // https://docs.adyen.com/get-started-with-adyen
    /**
     * class allowing online payements using a payment gateway
     */
    public class PayoutManager
    {
        // singleton
        private static PayoutManager instance = null;

        public static PayoutManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PayoutManager();
                }
                return instance;
            }
        }

        private PayoutManager()
        {

        }

        private class TestData : GetParams {
            public string name { get; set; }
            public long age { get; set; }
        };

        /**
         * method used to test the connection to the server
         * https://zetcode.com/csharp/getpostrequest/
         * post with params doesnt work (even if it works when posting with python, for example
         * so we use get, sorry lads
         */
        private async Task<ServerData> Test()
        {
            string url = "http://localhost:55000/Test";
            var test = new TestData() { name = "jean", age=(long)42 };

            var client = new HttpClient();
            var response = await client.GetAsync(url + test.GetParamsString());

            string result = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(result);

            return new ServerData
            {
                PayoutId = "id_test",
                PayoutStatus = "status_test",
                MerchantAccount = "account_test",
                AmountValue = 10,
                AmountCurrency = "CHF",
                Reference = "ref_test",
                ClientApiKey = "api_test",
            };
        }

        public async Task<ServerData> MakePayement(uint cardNumber, uint expiryMonth, uint expiryYear, uint securityCode, float amount, string reference)
        {
            //return await Test();
            
            // amount is set in long => 10.40CHF => 1040
            long lAmount = (long)(amount * 100);
            if (lAmount != amount * 100)
                throw new amountInvalidException();

            // TODO: encrypt things
            string encryptedCardNumber = "test_4111111111111111";
            string encryptedExpiryMonth = "test_03";
            string encryptedExpiryYear = "test_2030";
            string encryptedSecurityCode = "test_737";

            string amountCurrency = "CHF";

            string clientApiKey = "debug_api_key";

            string base_url = Environment.GetEnvironmentVariable("CASHREGISTER_ENTRYPOINT");
            string port = Environment.GetEnvironmentVariable("CASHREGISTER_PORT");
            string endpoint = Environment.GetEnvironmentVariable("CASHREGISTER_ENDPOINT");
            string url = base_url + ":" + port + "/" + endpoint;
            Debug.WriteLine("url\n" + url + "\n");
            return await Test();
            string _url = "http://localhost:55000/Payout";

            var data = new ClientData
            {
                encryptedCardNumber = encryptedCardNumber,
                encryptedExpiryMonth = encryptedExpiryMonth,
                encryptedExpiryYear = encryptedExpiryYear,
                encryptedSecurityCode = encryptedSecurityCode,

                amountValue = lAmount,
                amountCurrency = amountCurrency,

                reference = reference,
                ClientApiKey = clientApiKey
            };

            var client = new HttpClient();
            Debug.WriteLine("sending request...");
            Debug.WriteLine(url + data.GetParamsString());
            var response = await client.GetAsync(url + data.GetParamsString());
            Debug.WriteLine("response:");
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(response.Content);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new PaymentFailedException(response.Content.ReadAsStringAsync().Result);
            }
            
                ServerData serverData = ServerData.CreateFromJsonString(response.Content.ReadAsStringAsync().Result);

            Debug.WriteLine("payout: " + serverData);
            return serverData;
            // proxy thing ?
            // https://docs.microsoft.com/en-us/troubleshoot/developer/webapps/iis/development/make-get-request
        }
    }
}
