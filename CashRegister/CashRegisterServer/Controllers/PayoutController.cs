using Adyen;
using Adyen.Model.Checkout;
using Adyen.Service;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Runtime.Serialization;

namespace CashRegisterServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PayoutController : ControllerBase
    {
        private class PayoutData
        {
            public string PayoutId { get; set; }
            public string PayoutStatus { get; set; }
            public string MerchantAccount { get; set; }
            public Amount Amount { get; set; }
            public string Reference { get; set; }
        
        }

        private readonly ILogger<PayoutController> _logger;

        public PayoutController(ILogger<PayoutController> logger)
        {
            _logger = logger;
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
            long amountValue, string amountCurrency, string reference)
        {
            try
            {
                //var data = test(apiKey, merchantAccount);

                var data = DoThingWithAdyen(encryptedCardNumber, encryptedExpiryMonth, encryptedExpiryYear, encryptedSecurityCode, 
                    amountCurrency, amountValue, reference);
                
                return Ok(data);
            } catch (CustomException500 e)
            {
                return StatusCode(500, e.Message);
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // https://docs.adyen.com/online-payments/api-only?tab=codeBlockmethods_request_dcSnj_cs_6
        private PayoutData DoThingWithAdyen(string encryptedCardNumber, string encryptedExpiryMonth, string encryptedExpiryYear, string encryptedSecurityCode, 
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
            Console.WriteLine("\n" + paymentsResponse + "\n");

            return new PayoutData
            {
                PayoutId = "an id mha man",
                PayoutStatus = "status",
                MerchantAccount = merchantAccount,
                Amount = amount,
                Reference = reference
            };
        }

        private PayoutData test(string apiKey, string merchantAccount)
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
            return new PayoutData
            {
                PayoutId = "an id mha man",
                PayoutStatus = "status",
                MerchantAccount = "merch",
                Amount = new Adyen.Model.Checkout.Amount("EUR", 1000),
                Reference = "reference"
            };
        }

        [Serializable]
        private class CustomException500 : Exception
        {
            public CustomException500(string? message) : base(message)
            {
            }

            public CustomException500(string? message, Exception? innerException) : base(message, innerException)
            {
            }

            protected CustomException500(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}