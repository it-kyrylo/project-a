using ProjectA.Models.Teams;
using ProjectA.Models.PlayersModels;
using ProjectA.Models;
using Refit;
using System.Threading.Tasks;

namespace ProjectA.Clients
{
    public interface IFantasyPremierLeagueClient
    {
        [Get("/api/bootstrap-static/")]
        Task<TeamsData> LoadBootstrapStaticDataTeamsAsync();

        [Get("/api/bootstrap-static/")]
        Task<ElementsData> LoadBootstrapPlayersDataAsync();
    }
}
