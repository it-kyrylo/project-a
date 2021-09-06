using ProjectA.Models.PlayersModels;
using ProjectA.Services.Statistics.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.Statistics
{
    public interface IStatisticsService
    {
        public Task<Element> GetPayerData(string playerName);
        public Task<IEnumerable<ScorersData>> GetTopScorersAsync(int toPlace);

        public Task<IEnumerable<ScorersData>> GetTopScorersInATeamAsync(string teamName, int toPlace);

        public Task<IEnumerable<Element>> GetPLayersOfPositionInTeamAsync(string teamName, string position);

        public Task<int> TimesPlayerHasBeenInDreamTeamAsync(string playerName);

        public Task<IEnumerable<PlayerDreamTeamData>> PlayersInDreamTeamOfTeamAsync(string teamName);
    }
}
