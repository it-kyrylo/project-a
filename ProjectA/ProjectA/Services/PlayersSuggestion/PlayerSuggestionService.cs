using ProjectA.Models.PlayersModels;
using ProjectA.Repositories;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Services.PlayersSuggestion.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Services.PlayersSuggestion
{
    public class PlayerSuggestionService : IPlayerSuggestionService
    {
        private const int SuggestedPlayersCount = 5;
        private const double DoubleDivisor = 1.0;
        private const double PriceDivisor = 10.0;
        private const double MinPriceValue = 0;
        private readonly IPlayersRepository playersRepository;

        public PlayerSuggestionService(IPlayersRepository playersRepository)
        {
            this.playersRepository = playersRepository;
        }

        public async Task<IEnumerable<PlayerSpecificStatsModel>> GetByPricePointsPerGameRatio(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool parsedPosition, out PlayerPosition playerPosition);

            if (!parsedPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.playersRepository.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.Element_Type == playerPosition &&
                    p.Now_Cost >= minPrice &&
                    p.Now_Cost <= maxPrice)
                .OrderByDescending(p => double.Parse(p.Points_Per_Game))
                .ThenBy(p => p.Now_Cost)
                .Take(SuggestedPlayersCount)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.First_Name,
                    LastName = p.Second_Name,
                    Position = playerPosition.ToString(),
                    PointsPerGame = double.Parse(p.Points_Per_Game),
                    Price = p.Now_Cost / PriceDivisor,
                    Form = double.Parse(p.Form),
                    TotalPoints = p.Total_Points,
                    InfluenceCreativityThreatRank = p.Ict_Index_Rank_Type
                });

            return suggestedPlayers;
        }

        public async Task<IEnumerable<PlayerSpecificStatsModel>> GetByCurrentForm(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool parsedPosition, out PlayerPosition playerPosition);

            if (!parsedPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.playersRepository.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.Element_Type == playerPosition &&
                    p.Now_Cost >= minPrice &&
                    p.Now_Cost <= maxPrice)
                .OrderByDescending(p => double.Parse(p.Form))
                .Take(SuggestedPlayersCount)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.First_Name,
                    LastName = p.Second_Name,
                    Position = playerPosition.ToString(),
                    PointsPerGame = double.Parse(p.Points_Per_Game),
                    Price = p.Now_Cost / PriceDivisor,
                    Form = double.Parse(p.Form),
                    TotalPoints = p.Total_Points,
                    InfluenceCreativityThreatRank = p.Ict_Index_Rank_Type
                });

            return suggestedPlayers;
        }

        public async Task<IEnumerable<PlayerSpecificStatsModel>> GetByInfluenceThreatCreativityRank(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool parsedPosition, out PlayerPosition playerPosition);

            if (!parsedPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.playersRepository.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.Element_Type == playerPosition &&
                    p.Now_Cost >= minPrice &&
                    p.Now_Cost <= maxPrice)
                .OrderBy(p => p.Ict_Index_Rank_Type)
                .Take(SuggestedPlayersCount)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.First_Name,
                    LastName = p.Second_Name,
                    Position = playerPosition.ToString(),
                    PointsPerGame = double.Parse(p.Points_Per_Game),
                    Price = p.Now_Cost / PriceDivisor,
                    Form = double.Parse(p.Form),
                    TotalPoints = p.Total_Points,
                    InfluenceCreativityThreatRank = p.Ict_Index_Rank_Type
                });

            return suggestedPlayers;
        }

        public async Task<IEnumerable<PlayerSpecificStatsModel>> GetByPointsPerPrice(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool parsedPosition, out PlayerPosition playerPosition);

            if (!parsedPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.playersRepository.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.Element_Type == playerPosition &&
                    p.Now_Cost >= minPrice &&
                    p.Now_Cost <= maxPrice)
                .OrderByDescending(p => p.Total_Points * DoubleDivisor / p.Now_Cost)
                .Take(SuggestedPlayersCount)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.First_Name,
                    LastName = p.Second_Name,
                    Position = playerPosition.ToString(),
                    PointsPerGame = double.Parse(p.Points_Per_Game),
                    Price = p.Now_Cost / PriceDivisor,
                    Form = double.Parse(p.Form),
                    TotalPoints = p.Total_Points,
                    InfluenceCreativityThreatRank = p.Ict_Index_Rank_Type
                });

            return suggestedPlayers;
        }

        public async Task<IEnumerable<PlayerOverallStatsModel>> GetByOverallStats(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool isValidPosition, out PlayerPosition playerPosition);

            if (!isValidPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.playersRepository.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.Element_Type == playerPosition &&
                    p.Now_Cost >= minPrice &&
                    p.Now_Cost <= maxPrice)
                .OrderByDescending(p => (double.Parse(p.Form) + double.Parse(p.Points_Per_Game)) - p.Ict_Index_Rank_Type)
                .ThenBy(p => p.Now_Cost)
                .Take(SuggestedPlayersCount)
                .Select(p => new PlayerOverallStatsModel
                {
                    Id = p.Id,
                    FirstName = p.First_Name,
                    LastName = p.Second_Name,
                    Position = playerPosition.ToString(),
                    Price = p.Now_Cost / PriceDivisor,
                    OverallStats = (double.Parse(p.Form) + double.Parse(p.Points_Per_Game)) - p.Ict_Index_Rank_Type
                });

            return suggestedPlayers;
        }

        private static void ParsePosition(
            string position,
            out bool parsedPosition,
            out PlayerPosition playerPosition)
            => parsedPosition = Enum.TryParse(position.ToLower(), out playerPosition);

        private static bool IsPriceRangeValid(double minPrice, double maxPrice)
        {
            if (minPrice <= MinPriceValue ||
                minPrice >= maxPrice)
            {
                return false;
            }

            return true;
        }

        private static void TransformPricesInIntegers(ref double minPrice, ref double maxPrice)
        {
            minPrice *= PriceDivisor;
            maxPrice *= PriceDivisor;
        }
    }
}