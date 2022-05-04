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

        // I let this here to have a visual representation of how to use the server API on /Test
        //private class TestData : GetParams
        //{
        //    public string Name { get; set; }
        //    public long Age { get; set; }
        //};

        ///**
        // * method used to test the connection to the server
        // * https://zetcode.com/csharp/getpostrequest/
        // * post with params doesnt work (even if it works when posting with python, for example
        // * so we use get, sorry lads
        // */
        //private async Task<ServerData> Test()
        //{
        //    string url = "http://localhost:5018/Test";
        //    var test = new TestData() { Name = "jean", Age = (long)42 };

        //    var client = new HttpClient();
        //    var response = await client.GetAsync(url + "?" + test.GetParamsString());

        //    string result = response.Content.ReadAsStringAsync().Result;
        //    Debug.WriteLine(result);

        //    return new ServerData
        //    {
        //        PayoutId = "id_test",
        //        PayoutStatus = "status_test",
        //        MerchantAccount = "account_test",
        //        AmountValue = 10,
        //        AmountCurrency = "CHF",
        //        Reference = "ref_test",
        //        ClientApiKey = "api_test",
        //    };
        //}

        // Ask to request a payement on the CashRegister server
        public async Task<ServerData> MakePayement(string cardNumber, string expiryMonth, string expiryYear, string securityCode, double amount, string reference)
        {
            // amount is set in long => 10.40CHF => 1040
            long lAmount = (long)(amount * 100);
            if (lAmount != amount * 100)
                throw new AmountInvalidException();

            // TODO: encrypt things
            string encryptedCardNumber = $"test_{cardNumber}";
            string encryptedExpiryMonth = $"test_{expiryMonth}";
            string encryptedExpiryYear = $"test_{expiryYear}";
            string encryptedSecurityCode = $"test_{securityCode}";

            string amountCurrency = "CHF";

            string clientApiKey = "debug_api_key";

            // Didnt get to use a .env file on a xamarin app
            //
            //string base_url = Environment.GetEnvironmentVariable("CASHREGISTER_ENTRYPOINT");
            //string port = Environment.GetEnvironmentVariable("CASHREGISTER_PORT");
            //string endpoint = Environment.GetEnvironmentVariable("CASHREGISTER_ENDPOINT");
            //string url = base_url + ":" + port + "/" + endpoint;

            string url = "http://localhost:5018/Payout";

            var data = new ClientData
            {
                EncryptedCardNumber = encryptedCardNumber,
                EncryptedExpiryMonth = encryptedExpiryMonth,
                EncryptedExpiryYear = encryptedExpiryYear,
                EncryptedSecurityCode = encryptedSecurityCode,

                AmountValue = lAmount,
                AmountCurrency = amountCurrency,

                Reference = reference,
                ClientApiKey = clientApiKey
            };

            var client = new HttpClient();

            var response = await client.GetAsync(url + "?" + data.GetParamsString());

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new PaymentFailedException(response.Content.ReadAsStringAsync().Result);
            }

            ServerData serverData = ServerData.CreateFromJsonString(response.Content.ReadAsStringAsync().Result);

            return serverData;
            
            // if proxy issue
            // https://docs.microsoft.com/en-us/troubleshoot/developer/webapps/iis/development/make-get-request
        }
    }
}
