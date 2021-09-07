using Microsoft.AspNetCore.Mvc;

namespace ProjectA.Controllers
{
    [ApiController]
    [Route("/api/controller")]
    public class BotController : ControllerBase
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("I am alive");
        }
    }
}
