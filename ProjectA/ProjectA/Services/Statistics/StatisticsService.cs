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
            switch (positionName.ToUpper())
            {
                case "GOALKEEPER":
                    return 1;
                case "DEFENDER":
                    return 2;
                case "MIDFIELDER":
                    return 3;
                case "FORWARD":
                    return 4;
                default:
                    return -1;
            }
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
                    return "No such position";
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
                .OrderByDescending(p => p.Goals_Scored)
                .ThenByDescending(p => KeyBuilder.Build(p.First_Name, p.Second_Name))
                .Select(p => new ScorersData {
                    PlayerName = KeyBuilder.Build(p.First_Name, p.Second_Name), 
                    ScoredGoals = p.Goals_Scored
                });

            if (toPlace >= scores.Count())
            {
                return scores;
            }
            else if (toPlace <= 0)
            {
                return null;
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
                .OrderByDescending(p => p.Goals_Scored)
                .ThenByDescending(p => KeyBuilder.Build(p.First_Name, p.Second_Name))
                .Select(p => new ScorersData
                {
                    PlayerName = KeyBuilder.Build(p.First_Name, p.Second_Name),
                    ScoredGoals = p.Goals_Scored
                }); 

            if (toPlace >= scores.Count())
            {
                return scores;
            }
            else if(toPlace <= 0)
            {
                return null;
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

            return this._playersRepository.GetAllPlayersAsync().Result.Where(p => p.Team == team.Id && p.Element_Type == positionIndex);
        }

        public async Task<int> TimesPlayerHasBeenInDreamTeamAsync(string playerName)
        {
            Element player = await this._playersRepository.GetPlayerDataAsync(playerName);
            if (player == null)
            {
                return -1;
            }
            return player.Dreamteam_Count;
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
                .Where(p => p.Team == team.Id && p.Dreamteam_Count > 0)
                .Select(p => new PlayerDreamTeamData 
                {
                    DreamTeamCount = p.Dreamteam_Count, 
                    PlayerPosition = this.GetPositionName(p.Element_Type), 
                    PlayerName = KeyBuilder.Build(p.First_Name, p.Second_Name)
                });

            return players;
        }
    }
}
