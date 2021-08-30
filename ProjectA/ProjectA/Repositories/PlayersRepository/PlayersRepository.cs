using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectA.Infrastructure;
using ProjectA.Clients;
using ProjectA.Models.PlayersModels;
using ProjectA.Repositories.PlayersRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class PlayersRepository : IPlayersRepository
{
    private readonly IFantasyPremierLeagueClient _leagueClient;

    public PlayersRepository(IFantasyPremierLeagueClient leagueClient)
	{
        this._leagueClient = leagueClient;
    }

    public async Task<Player> GetPlayerDataAsync(string playerName)
    {
        var allPlyers = await this.GetAllPlayersAsync();
        return allPlyers.FirstOrDefault(p => KeyBuilder.Build(p.FirstName, p.LastName) == playerName);
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        var playersDataPerformance = await _leagueClient.LoadBootstrapPlayersDataAsync();
        return playersDataPerformance.Players;
    }
}
