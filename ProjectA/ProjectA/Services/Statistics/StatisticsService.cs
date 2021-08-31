using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectA.Infrastructure;
using ProjectA.Repositories.Teams;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Models.PlayersModels;
using ProjectA.Models.Teams;
using ProjectA.Services.Statistics.ServiceModels;

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

        public async Task<Element> GetPayerData(string playerName)
        {
            Element player = await this._playersRepository.GetPlayerDataAsync(playerName);
            if(player == null)
            {
                return null;
            }
            return player;
        }

        public async Task<IEnumerable<ScorersData>> GetTopScorersAsync(int toPlace)
        {
            IEnumerable<Element> playerData = await this._playersRepository.GetAllPlayersAsync();
            var scores = playerData
                .OrderByDescending(p => p.GoalsScored)
                .ThenByDescending(p => KeyBuilder.Build(p.FirstName, p.LastName))
                .Select(p => new ScorersData {
                    PlayerName = KeyBuilder.Build(p.FirstName, p.LastName), 
                    ScoredGoals = p.GoalsScored
                });

            if (toPlace >= scores.Count() || toPlace < 0)
            {
                return scores;
            }
            return scores.Take(toPlace);
        }

        public async Task<IEnumerable<ScorersData>> GetTopScorersInATeamAsync(string teamName, int toPlace)
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
                .Select(p => new ScorersData
                {
                    PlayerName = KeyBuilder.Build(p.FirstName, p.LastName),
                    ScoredGoals = p.GoalsScored
                }); 

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
            if (team == null || positionIndex == -1)
            {
                return null;
            }

            return this._playersRepository.GetAllPlayersAsync().Result.Where(p => p.CurrentTeam == team.Id && p.GamePositionIndex == positionIndex);
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

        public async Task<IEnumerable<PlayerDreamTeamData>> PlayersInDreamTeamOfTeamAsync(string teamName)
        {
            Team team = await this._teamsRepository.GetTeamByNameAsync(teamName);
            if (team == null)
            {
                return null;
            }

            IEnumerable<Element> playerData = await this._playersRepository.GetAllPlayersAsync();
            var players = playerData
                .Where(p => p.CurrentTeam == team.Id && p.DreamTeamCount > 0)
                .Select(p => new PlayerDreamTeamData 
                {
                    DreamTeamCount = p.DreamTeamCount, 
                    PlayerPosition = p.GamePositionName, 
                    PlayerName = KeyBuilder.Build(p.FirstName, p.LastName)
                });

            return players;
        }
    }
}
