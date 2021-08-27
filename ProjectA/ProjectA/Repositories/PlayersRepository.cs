using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectA.Auxiliary;
using ProjectA.Clients;
using ProjectA.Models;
using ProjectA.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayersRepository<T> : IPlayersRepository<T> where T : class
{
	private Dictionary<string, T> playersData;
    private readonly IFantasyPremierLeagueClient _leagueClient;

    public PlayersRepository(IFantasyPremierLeagueClient leagueClient)
	{
        this.playersData = new Dictionary<string, T>();
        this._leagueClient = leagueClient;
        List<T> data = this.LoadPlayers().Result;
        this.PopulatePlayersData(data);
    }

    private async Task<List<T>> LoadPlayers()
    {
        string response = await this._leagueClient.LoadBootstrapStaticPlayersDataTeamsAsync();
        string elements = JObject.Parse(response)["elements"].ToString(); ;
        return JsonConvert.DeserializeObject<List<T>>(elements);
    }

    private void PopulatePlayersData(List<T> data)
    {
        foreach (T currentData in data)
        {
            Player tmp = (Player)(object)currentData;
            string key = KeyBuilder.Build(tmp.FirstName, tmp.LastName);
            this.playersData[key] = currentData;
        }
    }

    public T GetPlayerData(string playerName)
    {
        if(this.playersData.ContainsKey(playerName) == true)
        {
            return this.playersData[playerName];
        }
        return null;
    }

    public int GetPlayerId(string playerName)
    {
        if (this.playersData.ContainsKey(playerName) == true)
        {
            Player tmp = (Player)(object)this.playersData[playerName];
            int id = tmp.Id;
            return id;
        }
        return -1;
    }

    public IEnumerable<T> GetAllPlayers()
    {
        return this.playersData.Values;
    }
}
