using ProjectA.Services.Teams;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectA.Services.Handlers
{
    public class HandlerTeamService : IHandlerTeamService
    {
        private readonly ITeamService _teamService;
        private StringBuilder stringBuilder;
        public HandlerTeamService(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<string> GetAllTeamsAsync()
        {
            stringBuilder = new StringBuilder();
            var teams = await _teamService.GetAllTeamsAsync();

            foreach (var team in teams)
            {
                stringBuilder
                    .Append($"{team.Name} | Strength : {team.Strength} | Wins : {team.Win} | Losses : {team.Loss} | Draws : {team.Draw}");

                stringBuilder.AppendLine();
            }


            return stringBuilder.ToString();
        }

        public async Task<string> GetStrongestTeamAwayAsync()
        {
            stringBuilder = new StringBuilder();

            var team = await _teamService.GetStrongestTeamAwayAsync();

            
            return stringBuilder
                  .Append($"{team.Name} | Strength away : {team.StrengthAway}")
                  .ToString();
        }

        public async Task<string> GetStrongestTeamHomeAsync()
        {
            stringBuilder = new StringBuilder();

            var team = await _teamService.GetStrongestTeamAwayAsync();


            return stringBuilder
                  .Append($"{team.Name} | Strength home : {team.StrengthHome}")
                  .ToString();
        }

        public async Task<string> GetTeamByNameAsync(string name)
        {
            stringBuilder = new StringBuilder();

            var team = await _teamService.GetTeamByNameAsync(name);


            return stringBuilder
                  .Append($"{team.Name} | Strength : {team.Strength} | Wins : {team.Win} | Losses : {team.Loss}")
                  .ToString();
        }

        public async Task<string> GetTeamWithMostDrawsAsync()
        {
            stringBuilder = new StringBuilder();

            var teams = await _teamService.GetTeamsWithMostDrawsAsync();

            var team = teams.First();


            return stringBuilder.Append(team.Name).ToString();

        }

        public async Task<string> GetTeamWithMostLossesAsync()
        {
            stringBuilder = new StringBuilder();

            var teams = await _teamService.GetTeamsWithMostLossesAsync();

            var team = teams.Last();


            return stringBuilder.Append(team.Name).ToString();
        }

        public async Task<string> GetTeamsWithMostWinsAsync()
        {
            stringBuilder = new StringBuilder();

            var teams = await _teamService.GetTeamsWithMostDrawsAsync();

            var team = teams.First();


            return stringBuilder.Append(team.Name).ToString();
        }

        public async Task<string> GetTopThreeStrongestTeamsAsync()
        {
            stringBuilder = new StringBuilder();

           var teams = await _teamService.GetTopThreeStrongestTeamsAsync();

            foreach (var team in teams)
            {
                stringBuilder.Append($"{team.Name} | Strength : {team.Strength}");

                stringBuilder.AppendLine();
            }


            return stringBuilder.ToString();

        }
    }
}
