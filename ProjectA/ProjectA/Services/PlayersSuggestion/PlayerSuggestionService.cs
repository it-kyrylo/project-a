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
        private const double PriceDivisor = 10.0;
        private const double UsualFPLPlayerMinPrice = 3.7;
        private const double UsualFPLPlayerMaxPrice = 14.1;

        private readonly IPlayersRepository players;

        public PlayerSuggestionService(IPlayersRepository players)
        {
            this.players = players;
        }

        public async Task<IEnumerable<PlayerSpecificStatsModel>> GetByPricePointsPerGameRatio(string position, double minPrice, double maxPrice)
        {
            ParsePosition(position, out bool parsedPosition, out PlayerPosition playerPosition);

            if (!parsedPosition || !IsPriceRangeValid(minPrice, maxPrice))
            {
                return null;
            }

            TransformPricesInIntegers(ref minPrice, ref maxPrice);

            var allPlayers = await this.players.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.GamePositionIndex == playerPosition &&
                    p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .OrderByDescending(p => p.PointsPerGame)
                .ThenBy(p => p.Price)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Position = playerPosition.ToString(),
                    PointsPerGame = p.PointsPerGame,
                    Price = p.Price / PriceDivisor,
                    Form = p.Form,
                    TotalPoints = p.TotalPoints,
                    InfluenceCreativityThreatRank = p.IndexRank
                })
                .Take(SuggestedPlayersCount);


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

            var allPlayers = await this.players.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.GamePositionIndex == playerPosition &&
                    p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Position = playerPosition.ToString(),
                    PointsPerGame = p.PointsPerGame,
                    Price = p.Price / PriceDivisor,
                    Form = p.Form,
                    TotalPoints = p.TotalPoints,
                    InfluenceCreativityThreatRank = p.IndexRank
                })
                .OrderByDescending(p => p.Form)
                .Take(SuggestedPlayersCount);

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

            var allPlayers = await this.players.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.GamePositionIndex == playerPosition &&
                    p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Position = playerPosition.ToString(),
                    PointsPerGame = p.PointsPerGame,
                    Price = p.Price / PriceDivisor,
                    Form = p.Form,
                    TotalPoints = p.TotalPoints,
                    InfluenceCreativityThreatRank = p.IndexRank
                })
                .OrderBy(p => p.InfluenceCreativityThreatRank)
                .Take(SuggestedPlayersCount);

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

            var allPlayers = await this.players.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.GamePositionIndex == playerPosition &&
                    p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .Select(p => new PlayerSpecificStatsModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Position = playerPosition.ToString(),
                    PointsPerGame = p.PointsPerGame,
                    Price = p.Price / PriceDivisor,
                    Form = p.Form,
                    TotalPoints = p.TotalPoints,
                    InfluenceCreativityThreatRank = p.IndexRank
                })
                .OrderByDescending(p => p.TotalPoints * 1.0 / p.Price)
                .Take(SuggestedPlayersCount);

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

            var allPlayers = await this.players.GetAllPlayersAsync();

            var suggestedPlayers = allPlayers
                .Where(p =>
                    (PlayerPosition)p.GamePositionIndex == playerPosition &&
                    p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .Select(p => new PlayerOverallStatsModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Position = playerPosition.ToString(),
                    Price = p.Price / PriceDivisor,
                    OverallStats = (p.Form + p.PointsPerGame) - p.IndexRank
                })
                .OrderByDescending(p => p.OverallStats)
                .ThenBy(p => p.Price)
                .Take(SuggestedPlayersCount);

            return suggestedPlayers;
        }

        private static void ParsePosition(
            string position,
            out bool parsedPosition,
            out PlayerPosition playerPosition)
            => parsedPosition = Enum.TryParse(position.ToLower(), out playerPosition);

        private static bool IsPriceRangeValid(double minPrice, double maxPrice)
        {
            if (minPrice <= UsualFPLPlayerMinPrice ||
                minPrice >= maxPrice ||
                maxPrice >= UsualFPLPlayerMaxPrice)
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