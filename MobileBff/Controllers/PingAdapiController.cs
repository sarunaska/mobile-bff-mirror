using Microsoft.AspNetCore.Mvc;
using MobileBff.Services;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingAdapiController : ControllerBase
    {
        private readonly IPingAdapiService pingAdapiService;

        public PingAdapiController(IPingAdapiService pingAdapiService)
        {
            this.pingAdapiService = pingAdapiService;
        }

        [HttpGet]
        public async Task<bool> Ping()
        {
            return await pingAdapiService.Ping();
        }
    }
}
