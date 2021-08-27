using Newtonsoft.Json;
using ProjectA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Repositories
{
    public class TeamRepository :ITeamRepository
    {
        private readonly IFantasyPremierLeagueRepository _leagueRepository;

        public TeamRepository(IFantasyPremierLeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Team GetTeamByName(string name)
        {
            var allTeams = GetAllTeams().Result;
       
            var team = allTeams.FirstOrDefault(n => n.Name == name);
       
            return team;
        }


       public async Task<IEnumerable<Team>> GetAllTeams()
       {

           var teamsData = JsonConvert.DeserializeObject<TeamsData>(await _leagueRepository.LoadBootstrapStaticData());
       
       
           return teamsData.Teams;
       
       }

    }
}
