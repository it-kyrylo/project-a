using ProjectA.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Clients
{
    public interface IFantasyPremierLeagueClient
    {
        [Get("/api/bootstrap-static/")]
        Task<TeamsData> LoadBootstrapStaticDataTeamsAsync();

        [Get("/api/bootstrap-static/")]
        Task<string> LoadBootstrapStaticPlayersDataTeamsAsync();
    }
}
