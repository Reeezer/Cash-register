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

        [HttpPost(Name = "PostPayout")]
        public IActionResult Post(float amount, string sender)
        {
            string? apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string? merchantAccount = Environment.GetEnvironmentVariable("MERCHANT_ACCOUNT");
            if (apiKey == null || merchantAccount == null)
            {
                return StatusCode(500, "API_KEY or MERCHANT_ACCOUNT not set");
            }
            var data = DoThingWithAdyen(amount, sender);
            
            return Ok(data);
        }

        private PayoutData DoThingWithAdyen(float amount, string sender)
        {
            var data = new PayoutData()
            {
                PayoutId = "12345",
                PayoutStatus = "dunno bro",
                MerchantAccount = "merchantAccount",
                Amount = amount,
                Sender = sender
            };
            return data;
        }
    }
}