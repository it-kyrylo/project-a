using ProjectA.Models.PlayersModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.PlayersSuggestion
{
    public interface IPlayerSuggestionService
    {
        Task<IEnumerable<PlayerSpecificStatsModel>> GetByPricePointsPerGameRatio(string position, double minPrice, double maxPrice);

        Task<IEnumerable<PlayerSpecificStatsModel>> GetByCurrentForm(string position, double minPrice, double maxPrice);

        Task<IEnumerable<PlayerSpecificStatsModel>> GetByInfluenceThreatCreativityRank(string position, double minPrice, double maxPrice);

        Task<IEnumerable<PlayerSpecificStatsModel>> GetByPointsPerPrice(string position, double minPrice, double maxPrice);

        Task<IEnumerable<PlayerOverallStatsModel>> GetByOverallStats(string position, double minPrice, double maxPrice);
    }
}
