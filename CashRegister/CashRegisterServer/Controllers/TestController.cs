using Microsoft.AspNetCore.Mvc;

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
        /// <summary>
        /// url to test if the server is up and running
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="age">age</param>
        /// <returns>custom hello world, for test purpose</returns>
        [HttpGet(Name = "PostTest")]
        public string Test(string name, long age)
        {
            return "bonjour " + name + " agé de " + age;
        }
    }
}