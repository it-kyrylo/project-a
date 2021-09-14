using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectA.Models.Teams;
using ProjectA.Services.Handlers;
using ProjectA.Services.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.ServicesTests;

namespace UnitTests
{
    [TestFixture]
    public class StateTeamServiceTests
    {
        private StateTeamService stateTeamService;
        private Mock<ITeamService> mock;
        private IEnumerable<TeamServiceModel> teams;
        private TeamServiceModel team;

        [SetUp]
        public void Initialize()
        {
            mock = new Mock<ITeamService>();
            stateTeamService = new StateTeamService(mock.Object);

            teams = new List<TeamServiceModel>()
            {
                new TeamServiceModel()
                {
                    Name = "Chelsea",
                    Strength = 5,
                    StrengthHome = 2,
                    StrengthAway = 7,
                    Win = 2,
                    Loss = 8,
                    Draw = 1
                },
                new TeamServiceModel()
                {
                    Name = "Man City",
                    Strength = 4,
                    StrengthHome = 3,
                    StrengthAway = 2,
                    Win = 9,
                    Loss = 4,
                    Draw = 2
                },
                new TeamServiceModel()
                {
                    Name = "Juve",
                    Strength = 3,
                    StrengthHome = 4,
                    StrengthAway = 3,
                    Win = 5,
                    Loss = 6,
                    Draw = 1
                }
            };
        }

        [Test]
        public async Task GetAllTeams_ReturnsAllTeams()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await stateTeamService.GetAllTeamsAsync();

            actual.Should().BeEquivalentTo(
                "Chelsea | Strength : 5 | Wins : 2 | Losses : 8 | Draws : 1"
                + Environment.NewLine
                + "Man City | Strength : 4 | Wins : 9 | Losses : 4 | Draws : 2"
                + Environment.NewLine
                + "Juve | Strength : 3 | Wins : 5 | Losses : 6 | Draws : 1"
                + Environment.NewLine);
        }

        [Test]
        public async Task GetStrongestTeamAway_ReturnsStrongestTeamAway()
        {         
            mock.Setup(t => t.GetStrongestTeamAwayAsync()).ReturnsAsync(teams.First());

            var actual = await stateTeamService.GetStrongestTeamAwayAsync();

            actual.Should().BeEquivalentTo("Chelsea | Strength away : 7");
        }

        [Test]
        public async Task GetStrongestTeamHome_ReturnsStrongestTeamHome()
        {
            mock.Setup(t => t.GetStrongestTeamHomeAsync()).ReturnsAsync(teams.First());

            var actual = await stateTeamService.GetStrongestTeamHomeAsync();

            actual.Should().BeEquivalentTo("Chelsea | Strength home : 2");
        }

        [Test]
        public async Task GetTeamByName_ReturnsTeamByName()
        {
            string name = "Chelsea";
            mock.Setup(t => t.GetTeamByNameAsync(name)).ReturnsAsync(teams.First());

            var actual = await stateTeamService.GetTeamByNameAsync(name);

            actual.Should().BeEquivalentTo($"Chelsea | Strength : 5 | Wins : 2 | Losses : 8");
        }

        [Test]
        public async Task GetTopThreeStrongestTeams_ReturnsTopThreeStrongestTeams()
        {          
            mock.Setup(t => t.GetTopThreeStrongestTeamsAsync()).ReturnsAsync(teams);

            var actual = await stateTeamService.GetTopThreeStrongestTeamsAsync();

            actual.Should().BeEquivalentTo(
                "Chelsea | Strength : 5"
                + Environment.NewLine
                + "Man City | Strength : 4"
                + Environment.NewLine
                + "Juve | Strength : 3"
                + Environment.NewLine);
        }

        [Test]
        public async Task GetTeamsWithMostDraws_ReturnsWithMostDraws()
        {
            mock.Setup(t => t.GetTeamsWithMostDrawsAsync()).ReturnsAsync(teams);

            var actual = await stateTeamService.GetTeamWithMostDrawsAsync();

            actual.Should().BeEquivalentTo("Chelsea");
        }

        [Test]
        public async Task GetTeamWithMostLossesAsync_ReturnsWithMostLosses()
        {
            mock.Setup(t => t.GetTeamsWithMostLossesAsync()).ReturnsAsync(teams);

            var actual = await stateTeamService.GetTeamWithMostLossesAsync();

            actual.Should().BeEquivalentTo("Juve | Losses : 6");
        }

        [Test]
        public async Task GetTeamsWithMostWins_ReturnsWithMostWins()
        {
            mock.Setup(t => t.GetTeamsWithMostWinsAsync()).ReturnsAsync(teams);

            var actual = await stateTeamService.GetTeamsWithMostWinsAsync();

            actual.Should().BeEquivalentTo("Chelsea | Wins : 2");
        }
    }
}
