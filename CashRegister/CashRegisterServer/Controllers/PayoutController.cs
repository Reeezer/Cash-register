using Adyen;
using Adyen.Service;
using CashRegister.moneyIsEverything.models;
using CashRegisterServer.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

using System.Runtime.Serialization;
using System.Text.Json;

namespace CashRegisterServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PayoutController : ControllerBase
    {
        private readonly ILogger<PayoutController> _logger;

        public PayoutController(ILogger<PayoutController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Ask to the Adyen API to execute a payement
        /// 
        /// params are the same as the Post method
        /// </summary>
        /// <returns>same return as the Post method</returns>
        [HttpGet(Name = "GetPayout")]
        public IActionResult Get(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode,
            long amountValue, string amountCurrency, string reference, string clientApiKey)
        {
            return Post(encryptedCardNumber, encryptedExpiryMonth, encryptedExpiryYear, encryptedSecurityCode, amountValue, amountCurrency, reference, clientApiKey);
        }


        /// <summary>
        /// this method is here only because the CashRegister client couldnt do a Post request with params
        /// even if I got to do it with python
        /// </summary>
        /// <param name="encryptedCardNumber">encrypter card number</param>
        /// <param name="encryptedExpiryMonth">encrypted expiry month</param>
        /// <param name="encryptedExpiryYear">encrypted expiry year</param>
        /// <param name="encryptedSecurityCode">encrypted security code</param>
        /// <param name="amountValue">value to pay, in long (10.40CHF => 1040)</param>
        /// <param name="amountCurrency">currency of the money ("EUR", "CHF", ...)</param>
        /// <param name="reference">reference of the payement</param>
        /// <param name="clientApiKey">api key of the client</param>
        /// <returns>a response indicating if the payement has been proceeded, or the payement issues</returns>
        [HttpPost(Name = "PostPayout")]	
        public IActionResult Post(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode,
            long amountValue, string amountCurrency, string reference, string clientApiKey)
        {
            // TODO: log transaction
            
            // Check if the client API key is valid
            if (!IsClientApiKeyValid(clientApiKey))
            {
                return BadRequest("Invalid client API key");
            }
            try
            {
                ServerData data = DoThingWithAdyen(encryptedCardNumber, encryptedExpiryMonth, encryptedExpiryYear, encryptedSecurityCode, 
                    amountCurrency, amountValue, reference);
                
                data.ClientApiKey = clientApiKey;
                
                // TODO: add to log transaction OK
                return Ok(JsonSerializer.Serialize<ServerData>(data));
            } catch (CustomException500 e)
            {
                // TODO: add to log transaction KO
                return StatusCode(500, e.Message);
            } catch(Exception e)
            {
                // TODO: add to log transaction KO
                return StatusCode(501, e.Message);
            }
        }

        /// <summary>
        /// verify if the client requesting the API is valid
        /// </summary>
        /// <param name="clientApiKey">API key of the client</param>
        /// <returns>return if it is valid or no</returns>
        private bool IsClientApiKeyValid(string clientApiKey)
        {
            // TODO: add db control
            return true;
        }

        /// <summary>
        /// create and execute a request on the Adyen API
        /// https://docs.adyen.com/online-payments/api-only?tab=codeBlockmethods_request_dcSnj_cs_6
        /// 
        /// params are the same as the Post method
        /// </summary>
        /// <returns>return is the same as the Post method</returns>
        /// <exception cref="CustomException500">Exception raised if there is an issue with the server (see exception details for more)</exception>
        private ServerData DoThingWithAdyen(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode, 
            string amountCurrency, long amountToPay, string reference)
        {
            // get the API Key and the merchant account from the environment variables
            string? apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string? merchantAccount = Environment.GetEnvironmentVariable("MERCHANT_ACCOUNT");
            if (apiKey == null || merchantAccount == null)
            {
                throw new CustomException500("API_KEY or MERCHANT_ACCOUNT not set");
            }
            
            
            var client = new Client(apiKey, Adyen.Model.Enum.Environment.Test);
            var checkout = new Checkout(client);
                        
            var details = new Adyen.Model.Checkout.DefaultPaymentMethodDetails
            {
                Type = "scheme",
                EncryptedCardNumber = encryptedCardNumber,
                EncryptedExpiryMonth = encryptedExpiryMonth,
                EncryptedExpiryYear = encryptedExpiryYear,
                EncryptedSecurityCode = encryptedSecurityCode
            };
            
            var amount = new Adyen.Model.Checkout.Amount(amountCurrency, amountToPay);

            var paymentRequest = new Adyen.Model.Checkout.PaymentRequest
            {
                PaymentMethod = details,
                MerchantAccount = merchantAccount,
                Amount = amount,
                Reference = reference,
                ReturnUrl = @"dunno"
            };
            _ = checkout.Payments(paymentRequest);

            if (!amount.Value.HasValue)
            {
                throw new CustomException500("Amount is undefined");
            }
            
            return new ServerData
            {
                PayoutId = "an id mha man",
                PayoutStatus = "status",
                MerchantAccount = merchantAccount,
                AmountValue = amount.Value.Value,
                AmountCurrency = "CHF",
                Reference = reference
            };
        }

        // I let this to have a visual representation of what's happening with adyen
        //
        //private ServerData Test(string apiKey, string merchantAccount)
        //{
        //    var client = new Client(apiKey, Adyen.Model.Enum.Environment.Test);
        //    var checkout = new Checkout(client);
        //    var details = new Adyen.Model.Checkout.DefaultPaymentMethodDetails
        //    {
        //        Type = "scheme",
        //        EncryptedCardNumber = "test_4111111111111111",
        //        EncryptedExpiryMonth = "test_03",
        //        EncryptedExpiryYear = "test_2030",
        //        EncryptedSecurityCode = "test_737"
        //    };
        //    var amount = new Adyen.Model.Checkout.Amount("EUR", 1000);
        //    var paymentRequest = new Adyen.Model.Checkout.PaymentRequest
        //    {
        //        PaymentMethod = details,
        //        MerchantAccount = merchantAccount,
        //        Amount = amount,
        //        Reference = "Reference 01231ca2",
        //        ReturnUrl = @"http://localhost:8000/redirect?shopperOrder=myRef"
        //    };
        //    _ = checkout.Payments(paymentRequest);
        //    return new ServerData
        //    {
        //        PayoutId = "an id mha man",
        //        PayoutStatus = "status",
        //        MerchantAccount = "merch",
        //        AmountValue = 1000,
        //        AmountCurrency = "CHF",
        //        Reference = "reference"
        //    };
        //}
    }
}