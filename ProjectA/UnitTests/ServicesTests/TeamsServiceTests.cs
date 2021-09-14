using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectA.Infrastructure;
using ProjectA.Models.Teams;
using ProjectA.Repositories.Teams;
using ProjectA.Services.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{

    [TestFixture]
    class TeamsServiceTests
    {
        private IMapper _mapper;
        private ITeamService teamService;
        private Mock<ITeamRepository> mock;

        private IEnumerable<Team> teams;
        private List<TeamServiceModel> teamServiceModels;

        [SetUp]
        public void Initialize()
        {
            Profile myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            mock = new Mock<ITeamRepository>();
            teamService = new TeamService(mock.Object, mapper);

            teams = new List<Team>()
            {
                new Team()
                {
                    Name = "Chelsea",
                    Strength = 5,
                    StrengthHome = 2,
                    StrengthAway = 7,
                    Win = 2,
                    Loss = 8,
                    Draw = 1
                },
                new Team()
                {
                    Name = "Man City",
                    Strength = 4,
                    StrengthHome = 3,
                    StrengthAway = 2,
                    Win = 9,
                    Loss = 4,
                    Draw = 2
                },
                new Team()
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

            teamServiceModels = new List<TeamServiceModel>()
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
        public async Task GetAllTeamsAsync_ReturnsAllTeams()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetAllTeamsAsync();

            actual.Should().BeEquivalentTo(teamServiceModels);
        }

        [Test]
        public async Task GetStrongestTeamAwayAsync_ReturnsStrongestTeamAway()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetStrongestTeamAwayAsync();

            actual.Should().BeEquivalentTo(teamServiceModels.First());
        }

        [Test]
        public async Task GetStrongestTeamHomeAsync_ReturnsStrongestTeamHome()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetStrongestTeamHomeAsync();

            actual.Should().BeEquivalentTo(teamServiceModels.Last());
        }

        [Test]
        public async Task GetTopThreeStrongestTeamsAsync_ReturnsTopThreeStrongestTeams()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetTopThreeStrongestTeamsAsync();

            actual.Should().BeEquivalentTo(teamServiceModels);
        }

        [Test]
        public async Task GetTeamByNameAsync_ReturnsTeamByName()
        {
            var name = "Juve";

            mock.Setup(t => t.GetTeamByNameAsync(name)).ReturnsAsync(teams.Last());

            var actual = await teamService.GetTeamByNameAsync(name);

            actual.Should().BeEquivalentTo(teamServiceModels.Last());
        }

        [Test]
        public async Task GetTeamsWithMostDrawsAsync_ReturnsTeamsWithMostDraws()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetTeamsWithMostDrawsAsync();

            actual.Should().BeEquivalentTo(teamServiceModels.Where(t => t.Name == "Man City"));
        }

        [Test]
        public async Task GetTeamsWithMostLossesAsync_ReturnsTeamsWithMostLosses()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetTeamsWithMostLossesAsync();

            actual.Should().BeEquivalentTo(teamServiceModels.Where(t => t.Name == "Chelsea"));
        }

        [Test]
        public async Task GetTeamsWithMostWinsAsync_ReturnsTeamsWithMostWins()
        {
            mock.Setup(t => t.GetAllTeamsAsync()).ReturnsAsync(teams);

            var actual = await teamService.GetTeamsWithMostWinsAsync();

            actual.Should().BeEquivalentTo(teamServiceModels.Where(t => t.Name == "Man City"));
        }
    }
}
