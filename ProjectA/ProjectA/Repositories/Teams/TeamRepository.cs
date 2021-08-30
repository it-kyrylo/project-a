using ProjectA.Clients;
using ProjectA.Models.Teams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Repositories.Teams
{
    public class TeamRepository :ITeamRepository
    {
        private readonly IFantasyPremierLeagueClient _leagueClient;

        public TeamRepository(IFantasyPremierLeagueClient leagueClient)
        {
            _leagueClient = leagueClient;
        }

        public async Task<Team> GetTeamByNameAsync(string name)
        {
            var allTeams = await GetAllTeamsAsync();
       
            var team = allTeams.FirstOrDefault(n => n.Name == name);
       
            return team;
        }


       public async Task<IEnumerable<Team>> GetAllTeamsAsync()
       {
           var teamsData = await _leagueClient.LoadBootstrapStaticDataTeamsAsync();
        
           return teamsData.Teams;       
       }

    }
}
