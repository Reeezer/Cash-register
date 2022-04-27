using CashRegister.moneyIsEverything.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CashRegister.moneyIsEverything
{
    // ICI
    // https://docs.adyen.com/get-started-with-adyen
    /**
     * class allowing online payements using TWINT payment gateway
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

        private class Test : GetParams {
            public string name { get; set; }
            public long age { get; set; }
        };

        /**
         * method used to test the connection to the server
         * https://zetcode.com/csharp/getpostrequest/
         * post with params doesnt work (even if it works when posting with python, for example
         * so we use get, sorry lads
         */
        private async void test()
        {
            string url = "http://localhost:55000/Test";
            var test = new Test() { name = "jean", age=(long)42 };

            var client = new HttpClient();
            var response = await client.GetAsync(url + test.GetParamsString());

            string result = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(result);
        }
        public async Task<ServerData> MakePayement(uint cardNumber, uint expiryMonth, uint expiryYear, uint securityCode, float amount, string reference)
        {
            //test();
            //return;
            
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

            string url = "http://localhost:55000/Payout";

            var data = new ClientData
            {
                encryptedCardNumber = encryptedCardNumber,
                encryptedExpiryMonth = encryptedExpiryMonth,
                encryptedExpiryYear = encryptedExpiryYear,
                encryptedSecurityCode = encryptedSecurityCode,

                amountValue = lAmount,
                amountCurrency = amountCurrency,

                reference = reference
            };

            var client = new HttpClient();
            var response = await client.GetAsync(url + data.GetParamsString());

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new PaymentFailedException(response.Content.ReadAsStringAsync().Result);
            }
            
                ServerData serverData = ServerData.CreateFromJsonString(response.Content.ReadAsStringAsync().Result);

            return serverData;
            // proxy thing ?
            // https://docs.microsoft.com/en-us/troubleshoot/developer/webapps/iis/development/make-get-request
        }
    }
}
