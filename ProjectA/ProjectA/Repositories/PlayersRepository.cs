using Newtonsoft.Json;
using ProjectA.Models;
using ProjectA.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class PlayersRepository<T> : IPlayersRepository<T> where T : class
{
	private readonly Dictionary<string, T> playersData;

    public async Task<PlayersRepository<T>> Create()
    {
        PlayersRepository<T> currentClass = new PlayersRepository<T>();
        List<T> data = await currentClass.LoadPlayers();
        currentClass.PopulatePlayersData(data);
        return currentClass;
    }

    private PlayersRepository()
	{
        this.playersData = new Dictionary<string, T>();
    }

    private async Task<List<T>> LoadPlayers()
    {
        const string URL = "https://fantasy.premierleague.com/api/bootstrap-static/";

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(URL);

            HttpResponseMessage response = await client.GetAsync(URL);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<T>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
    }

    private void PopulatePlayersData(List<T> data)
    {
        foreach (T currentData in data)
        {
            string key;
            if (currentData.GetType() == typeof(PerformancePlayerData))
            {
                PerformancePlayerData tmp = (PerformancePlayerData)(object)currentData;
                key = tmp.FirstName + " " + tmp.LastName;
            }
            else
            {
                StatisticsPlayerData tmp = (StatisticsPlayerData)(object)currentData;
                key = tmp.FirstName + " " + tmp.LastName;
            }
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
            int id;
            T currentPlayer = this.playersData[playerName];
            if (currentPlayer.GetType() == typeof(PerformancePlayerData))
            {
                PerformancePlayerData tmp = (PerformancePlayerData)(object)currentPlayer;
                id = tmp.Id;
            }
            else
            {
                StatisticsPlayerData tmp = (StatisticsPlayerData)(object)currentPlayer;
                id = tmp.Id;
            }
            return id;
        }
        return -1;
    }

    public IEnumerable<T> GetAllPlayers()
    {
        return this.playersData.Values;
    }
}
