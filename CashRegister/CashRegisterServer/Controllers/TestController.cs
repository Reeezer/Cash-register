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
    public class TestController : ControllerBase
    {
        private readonly ILogger<PayoutController> _logger;

        public TestController(ILogger<PayoutController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostTest")]
        public string Post()
        {
            return "bonjour";
        }
    }
}