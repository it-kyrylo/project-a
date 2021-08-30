using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectA.Infrastructure;
using ProjectA.Clients;
using ProjectA.Models.PlayersModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ProjectA.Repositories.PlayersRepository
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IFantasyPremierLeagueClient _leagueClient;

        public PlayersRepository(IFantasyPremierLeagueClient leagueClient)
        {
            _leagueClient = leagueClient;
        }

        public async Task<Players> GetPlayerDataAsync(string playerName)
        {
            var allPlyers = await GetAllPlayersAsync();
            return allPlyers.FirstOrDefault(p => KeyBuilder.Build(p.FirstName, p.LastName) == playerName);
        }

        public async Task<IEnumerable<Players>> GetAllPlayersAsync()
        {
            var playersDataPerformance = await _leagueClient.LoadBootstrapPlayersDataAsync();
            return playersDataPerformance.Players;
        }
    }
}