using System;
using System.IO;
using System.Net;
using System.Text.Json;

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

        private class Data
        {
            public string encryptedCardNumber { get; set; }
            public string encryptedExpiryMonth { get; set; }
            public string encryptedExpiryYear { get; set; }
            public string encryptedSecurityCode { get; set; }
            public long amount { get; set; }
            public string currency { get; set; }
            public string reference { get; set; }
        }
        private void test()
        {
            // TODO: post on server
        }
        public void MakePayement(uint cardNumber, uint expiryMonth, uint expiryYear, uint securityCode, float amount, string reference)
        {
            test();
            return;
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

            string url = "http://localhost:55003/Payout";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var data = new Data
            {
                encryptedCardNumber = encryptedCardNumber,
                encryptedExpiryMonth = encryptedExpiryMonth,
                encryptedExpiryYear = encryptedExpiryYear,
                encryptedSecurityCode = encryptedSecurityCode,

                amount = lAmount,
                currency = amountCurrency,

                reference = reference
            };
            var jsonData = JsonSerializer.Serialize(data);
            request.GetRequestStream().Write(System.Text.Encoding.UTF8.GetBytes(jsonData), 0, jsonData.Length);

            var response = request.GetResponse();
            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
            Console.WriteLine("\nresponse: " + httpWebResponse.StatusCode + " " + httpWebResponse.StatusDescription);

            var reader = new StreamReader(response.GetResponseStream());
            string outputData = reader.ReadToEnd();
            Console.WriteLine("data: " + outputData + "\n");

            // proxy thing ?
            // https://docs.microsoft.com/en-us/troubleshoot/developer/webapps/iis/development/make-get-request
        }
    }
}
