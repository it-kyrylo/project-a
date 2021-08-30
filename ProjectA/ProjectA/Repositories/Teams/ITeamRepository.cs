using ProjectA.Models.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories.Teams
{
    public interface ITeamRepository
    {        
        Task<IEnumerable<Team>> GetAllTeamsAsync();

         Task<Team> GetTeamByNameAsync(string name);
    }
}
