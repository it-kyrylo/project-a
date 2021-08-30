using ProjectA.Models.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.Teams
{
    public interface ITeamService
    {
        Task<TeamServiceModel> GetTeamByNameAsync(string name);

        Task<IEnumerable<TeamServiceModel>> GetAllTeamsAsync();

        Task<TeamServiceModel> GetStrongestTeamHomeAsync();

        Task<IEnumerable<TeamServiceModel>> GetTopThreeStrongestTeamsAsync();

        Task<TeamServiceModel> GetStrongestTeamAwayAsync();

        Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostWinsAsync();

        Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostLossesAsync();

        Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostDrawsAsync();


    }
}
