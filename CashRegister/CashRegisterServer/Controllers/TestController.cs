using Microsoft.AspNetCore.Mvc;

namespace CashRegisterServer.Controllers
{
    /**
     * url to test if the server is up and running
     */
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<PayoutController> _logger;

        public TestController(ILogger<PayoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "PostTest")]
        public string Test(string name, long age)
        {
            return "bonjour " + name + " agé de " + age;
        }
    }
}