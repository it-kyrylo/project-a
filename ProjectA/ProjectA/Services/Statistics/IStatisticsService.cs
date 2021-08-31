using ProjectA.Models.PlayersModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.Statistics
{
    interface IStatisticsService
    {
        public Task<IEnumerable<Tuple<int, string>>> GetTopScorersAsync(int toPlace);

        public Task<IEnumerable<Tuple<int, string>>> GetTopScorersInATeamAsync(string teamName, int toPlace);

        public Task<IEnumerable<Element>> GetPLayersOfPositionInTeamAsync(string teamName, string position);

        public Task<int> TimesPlayerHasBeenInDreamTeamAsync(string playerName);

        public Task<IEnumerable<Tuple<int, string, string>>> PlayersInDreamTeamOfTeamAsync(string teamName);
    }
}
