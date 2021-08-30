using ProjectA.Models.Teams;
using Refit;
using System.Threading.Tasks;

namespace ProjectA.Clients
{
    public interface IFantasyPremierLeagueClient
    {
        [Get("/api/bootstrap-static/")]
        Task<TeamsData> LoadBootstrapStaticDataTeamsAsync();
    }
}
