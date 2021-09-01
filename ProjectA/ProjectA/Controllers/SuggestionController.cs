using Microsoft.AspNetCore.Mvc;
using ProjectA.Models.PlayersModels;
using ProjectA.Services.PlayersSuggestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Controllers
{
    [ApiController]
    [Route("/api/controller")]
    public class SuggestionController : ControllerBase
    {
        private readonly IPlayerSuggestionService players;

        public SuggestionController(IPlayerSuggestionService players)
        {
            this.players = players;
        }

        [HttpGet("{position}/{minPrice}/{maxPrice}")]
        public async Task<ActionResult<IEnumerable<PlayerSpecificStatsModel>>> GetPlayers(string position, double minPrice, double maxPrice)
        {
            var players = await this.players.GetByPricePointsPerGameRatio(position, minPrice, maxPrice);

            return players.ToList();
        }
    }
}
