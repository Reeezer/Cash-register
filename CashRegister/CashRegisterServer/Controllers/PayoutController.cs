using Microsoft.AspNetCore.Mvc;

using System.IO;
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
            public float Amount { get; set; }
            public string Sender { get; set; }
        
        }

        private readonly ILogger<PayoutController> _logger;

        public PayoutController(ILogger<PayoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPayout")]
        public IActionResult Get()
        {
            string? payoutStatus = Environment.GetEnvironmentVariable("API_KEY");
            string? merchantAccount = Environment.GetEnvironmentVariable("MERCHANT_ACCOUNT");
            Console.WriteLine("\n\nBONJOUR: " + payoutStatus + " " + merchantAccount);
            var data = new PayoutData()
            {
                PayoutId = "12345",
                PayoutStatus = payoutStatus,
                MerchantAccount = merchantAccount,
                Amount = 100.00f,
                Sender = "John Doe"
            };
            

            return Ok(data);
        }
    }
}