using Microsoft.AspNetCore.Mvc;
using ProjectA.Handlers;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.Statistics;
using ProjectA.Services.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

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
