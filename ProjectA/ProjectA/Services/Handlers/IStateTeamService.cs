using System.Threading.Tasks;

namespace ProjectA.Services.Handlers
{
    public interface IStateTeamService
    {
        Task<string> GetAllTeamsAsync();

        Task<string> GetStrongestTeamAwayAsync();

        Task<string> GetStrongestTeamHomeAsync();

        Task<string> GetTeamByNameAsync(string name);

        Task<string> GetTeamWithMostDrawsAsync();

        Task<string> GetTeamWithMostLossesAsync();

        Task<string> GetTeamsWithMostWinsAsync();

        Task<string> GetTopThreeStrongestTeamsAsync();

    }
}
