using ProjectA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories
{
    public interface ITeamRepository
    {        
        Task<IEnumerable<Team>> GetAllTeamsAsync();

         Task<Team> GetTeamByNameAsync(string name);
    }
}
