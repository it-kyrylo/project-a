using ProjectA.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Repositories
{
    public interface ITeamRepository
    {        
        Task<IEnumerable<Team>> GetAllTeams();

        Team GetTeamByName(string name);
    }
}
