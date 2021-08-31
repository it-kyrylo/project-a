using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectA.Infrastructure;
using ProjectA.Repositories.Teams;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Models.PlayersModels;
using ProjectA.Models.Teams;

namespace ProjectA.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IPlayersRepository _playersRepository;
        private readonly ITeamRepository _teamsRepository;

        public StatisticsService(IPlayersRepository playersRepository, ITeamRepository teamsRepository)
        {
            this._playersRepository = playersRepository;
            this._teamsRepository = teamsRepository;
        }

        private string GetPositionName(int positionIndex)
        {
            switch (positionIndex)
            {
                case 1:
                    return "Goalkeeper";
                case 2:
                    return "Defender";
                case 3:
                    return "Midfielder";
                case 4:
                    return "Forward";
                default:
                    return "Wrong index";
            }
        }

        private int GetPositionIndex(string positionName)
        {
            switch (positionName)
            {
                case "Goalkeeper":
                    return 1;
                case "Defender":
                    return 2;
                case "Midfielder":
                    return 3;
                case "Forward":
                    return 4;
                default:
                    return -1;
            }
        }

        public async Task<Tuple<Element, string>> GetPayerData(string playerName)
        {
            Element player = await this._playersRepository.GetPlayerDataAsync(playerName);
            if(player == null)
            {
                return null;
            }
            return new Tuple<Element, string>(player, this.GetPositionName(player.GamePosition));
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetTopScorersAsync(int toPlace)
        {
            IEnumerable<Element> playerData = await this._playersRepository.GetAllPlayersAsync();
            var scores = playerData
                .OrderByDescending(p => p.GoalsScored)
                .ThenByDescending(p => KeyBuilder.Build(p.FirstName, p.LastName))
                .Select(p => new Tuple<int, string>(p.GoalsScored, KeyBuilder.Build(p.FirstName, p.LastName)));

            if (toPlace >= scores.Count() || toPlace < 0)
            {
                return scores;
            }
            return scores.Take(toPlace);
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetTopScorersInATeamAsync(string teamName, int toPlace)
        {
            Team team = await this._teamsRepository.GetTeamByNameAsync(teamName);
            if (team == null)
            {
                return null;
            }

            IEnumerable<Element> playerData = await this._playersRepository.GetAllPlayersAsync();
            var scores = playerData
                .OrderByDescending(p => p.GoalsScored)
                .ThenByDescending(p => KeyBuilder.Build(p.FirstName, p.LastName))
                .Select(p => new Tuple<int, string>(p.GoalsScored, KeyBuilder.Build(p.FirstName, p.LastName)));

            if (toPlace >= scores.Count() || toPlace < 0)
            {
                return scores;
            }
            return scores.Take(toPlace);
        }

        public async Task<IEnumerable<Element>> GetPLayersOfPositionInTeamAsync(string teamName, string position)
        {
            int positionIndex = this.GetPositionIndex(position);
            Team team = await this._teamsRepository.GetTeamByNameAsync(teamName);
            if (team == null)
            {
                return null;
            }

            return this._playersRepository.GetAllPlayersAsync().Result.Where(p => p.CurrentTeam == team.Id && p.GamePosition == positionIndex);
        }

        public async Task<int> TimesPlayerHasBeenInDreamTeamAsync(string playerName)
        {
            Element player = await this._playersRepository.GetPlayerDataAsync(playerName);
            if (player == null)
            {
                return -1;
            }
            return player.DreamTeamCount;
        }

        public async Task<IEnumerable<Tuple<int, string, string>>> PlayersInDreamTeamOfTeamAsync(string teamName)
        {
            Team team = await this._teamsRepository.GetTeamByNameAsync(teamName);
            if (team == null)
            {
                return null;
            }

            IEnumerable<Element> playerData = await this._playersRepository.GetAllPlayersAsync();
            var players = playerData
                .Where(p => p.CurrentTeam == team.Id && p.DreamTeamCount > 0)
                .Select(p => new Tuple<int, string, string>(p.DreamTeamCount, this.GetPositionName(p.GamePosition), KeyBuilder.Build(p.FirstName, p.LastName)));

            return players;
        }
    }
}
