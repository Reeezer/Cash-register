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

        [HttpGet(Name = "GetPayout")]
        public IActionResult Get(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode,
            long amountValue, string amountCurrency, string reference, string clientApiKey)
        {
            return Post(encryptedCardNumber, encryptedExpiryMonth, encryptedExpiryYear, encryptedSecurityCode, amountValue, amountCurrency, reference, clientApiKey);
        }            

        [HttpPost(Name = "PostPayout")]
        // 'encryptedCardNumber': 'test_4111111111111111',
        // 'encryptedExpiryMonth': 'test_03',
        // 'encryptedExpiryYear': 'test_2030',
        // 'encryptedSecurityCode': 'test_737',
        // 'amountValue': 300,
        // 'amountCurrency': 'CHF',
        // 'reference': 'Reference ' + str(uuid.uuid4()),	
        public IActionResult Post(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode,
            long amountValue, string amountCurrency, string reference, string clientApiKey)
        {
            // Check if the client API key is valid
            if (checkClientApiKey(clientApiKey) == false)
            {
                return BadRequest("Invalid client API key");
            }
                try
            {
                //var data = test(apiKey, merchantAccount);

                ServerData data = DoThingWithAdyen(encryptedCardNumber, encryptedExpiryMonth, encryptedExpiryYear, encryptedSecurityCode, 
                    amountCurrency, amountValue, reference);
                
                data.ClientApiKey = clientApiKey;
                
                return Ok(JsonSerializer.Serialize<ServerData>(data));
            } catch (CustomException500 e)
            {
                return StatusCode(500, e.Message);
            } catch(Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }

        /**
         * verify if the client requesting the API is valid
         */
        private bool checkClientApiKey(string clientApiKey)
        {
            // TODO: add db thing
            return true;
        }
        
            // https://docs.adyen.com/online-payments/api-only?tab=codeBlockmethods_request_dcSnj_cs_6
            private ServerData DoThingWithAdyen(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode, 
            string amountCurrency, long amountToPay, string reference)
        {
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
            var paymentsResponse = checkout.Payments(paymentRequest);

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

        private ServerData test(string apiKey, string merchantAccount)
        {
            var client = new Client(apiKey, Adyen.Model.Enum.Environment.Test);
            var checkout = new Checkout(client);
            var details = new Adyen.Model.Checkout.DefaultPaymentMethodDetails
            {
                Type = "scheme",
                EncryptedCardNumber = "test_4111111111111111",
                EncryptedExpiryMonth = "test_03",
                EncryptedExpiryYear = "test_2030",
                EncryptedSecurityCode = "test_737"
            };
            var amount = new Adyen.Model.Checkout.Amount("EUR", 1000);
            var paymentRequest = new Adyen.Model.Checkout.PaymentRequest
            {
                PaymentMethod = details,
                MerchantAccount = merchantAccount,
                Amount = amount,
                Reference = "Reference 01231ca2",
                ReturnUrl = @"http://localhost:8000/redirect?shopperOrder=myRef"
            };
            var paymentsResponse = checkout.Payments(paymentRequest);
            return new ServerData
            {
                PayoutId = "an id mha man",
                PayoutStatus = "status",
                MerchantAccount = "merch",
                AmountValue = 1000,
                AmountCurrency = "CHF",
                Reference = "reference"
            };
        }
    }
}