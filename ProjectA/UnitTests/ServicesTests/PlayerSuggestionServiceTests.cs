using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectA.Models.PlayersModels;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Services.PlayersSuggestion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    class PlayerSuggestionServiceTests
    {
        private PlayerSuggestionService playerSuggestionService;

        private List<Element> elements;
        private List<PlayerSpecificStatsModel> players;

        private const string position = "2";
        private const double minPrice = 4;
        private const double maxPrice = 12;

        [SetUp]
        public void Initialize()
        {
            elements = new List<Element>()
            {
                new Element()
                {
                    Element_Type = 2,
                    Now_Cost = 50,
                    Points_Per_Game = "10",
                    Form = "5",
                    Ict_Index_Rank_Type = 5
                },
                new Element()
                {
                    Element_Type = 2,
                    Now_Cost = 60,
                    Points_Per_Game = "12",
                    Form = "5",
                    Ict_Index_Rank_Type = 1
                }
            };

            players = new List<PlayerSpecificStatsModel>()
            {
                new PlayerSpecificStatsModel()
                {
                    Position = "defender",
                    Price = 6,
                    PointsPerGame = 12,
                    Form = 5,
                    InfluenceCreativityThreatRank = 1
                },
                new PlayerSpecificStatsModel()
                {
                    Position = "defender",
                    Price = 5,
                    PointsPerGame = 10,
                    Form = 5,
                    InfluenceCreativityThreatRank = 5
                }
            };
        }

        [Test]
        public async Task GetByPricePointsPerGameRatio_ReturnsPlayersByPricePointsPerGameRatio()
        {
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(p => p.GetAllPlayersAsync()).ReturnsAsync(elements);

            playerSuggestionService = new PlayerSuggestionService(mock.Object);

            var actual = await playerSuggestionService.GetByPricePointsPerGameRatio(position, minPrice, maxPrice);

            actual.Should().BeEquivalentTo(players);
        }

        [Test]
        public async Task GetByCurrentForm_ReturnsPlayersByCurrentForm()
        {
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(p => p.GetAllPlayersAsync()).ReturnsAsync(elements);

            playerSuggestionService = new PlayerSuggestionService(mock.Object);

            var actual = await playerSuggestionService.GetByCurrentForm(position, minPrice, maxPrice);

            actual.Should().BeEquivalentTo(players);
        }

        [Test]
        public async Task GetByInfluenceThreatCreativityRank_ReturnsPlayersByInfluenceThreatCreativityRank()
        {
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(p => p.GetAllPlayersAsync()).ReturnsAsync(elements);

            playerSuggestionService = new PlayerSuggestionService(mock.Object);

            var actual = await playerSuggestionService.GetByInfluenceThreatCreativityRank(position, minPrice, maxPrice);

            actual.Should().BeEquivalentTo(players);
        }

        [Test]
        public async Task GetByPointsPerPrice_ReturnsPlayersByPointsPerPrice()
        {
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(p => p.GetAllPlayersAsync()).ReturnsAsync(elements);

            playerSuggestionService = new PlayerSuggestionService(mock.Object);

            var actual = await playerSuggestionService.GetByPointsPerPrice(position, minPrice, maxPrice);

            actual.Should().BeEquivalentTo(players);
        }

        [Test]
        public async Task GetByOverallStats_ReturnsPlayersByOverallStats()
        {
            var mock = new Mock<IPlayersRepository>();
            mock.Setup(p => p.GetAllPlayersAsync()).ReturnsAsync(elements);

            playerSuggestionService = new PlayerSuggestionService(mock.Object);

            var actual = await playerSuggestionService.GetByOverallStats(position, minPrice, maxPrice);

            var players = new List<PlayerOverallStatsModel>()
            {
                new PlayerOverallStatsModel()
                {
                    Position = "defender",
                    Price = 6,
                    OverallStats = 16.0

                },
                new PlayerOverallStatsModel()
                {
                    Position = "defender",
                    Price = 5,
                    OverallStats = 10.0
                }
            };

            actual.Should().BeEquivalentTo(players);
        }
    }
}
