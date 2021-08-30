using ProjectA.Models.PlayersModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.Statistics
{
    interface IStatisticsService
    {
        public Task<IEnumerable<Tuple<int, string>>> GetTopScorers(int toPlace);

        public Task<IEnumerable<Tuple<int, string>>> GetTopScorersInATeamAsync(string teamName, int toPlace);

        public Task<IEnumerable<Element>> GetPLayersOfPositionInTeam(string teamName, string position);
    }
}
