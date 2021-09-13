using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectA.Clients;
using ProjectA.Models.Teams;
using ProjectA.Repositories.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TeamStatisticsTests.Repository
{
    [TestFixture]
    class TeamRepositoryTests
    {
        private Mock<IFantasyPremierLeagueClient> _leagueClientMock;
        private ITeamRepository _teamsRepository;

        [SetUp]
        public void SetUp()
        {
            _leagueClientMock = new Mock<IFantasyPremierLeagueClient>();

            _leagueClientMock
                .Setup(x => x.LoadBootstrapStaticDataTeamsAsync().Result)
                .Returns(new TeamsData()
                {
                    Teams = new List<Team>(){ new Team {Id=1, Name = "Arsenal" }
                }
                });

            _teamsRepository = new TeamRepository(_leagueClientMock.Object);
        }

        [Test]
        public void GetAllTeams_CompareCollectionType_ReturnListCollection()
        {
            var actualResult = _teamsRepository.GetAllTeamsAsync().Result;

            actualResult.Should().BeOfType(typeof(List<Team>));
        }

        [Test]
        public void GetAllTeam_CheckNumberOfTeams_ReturnOne()
        {
            var expectedNumberOfTeams = 1;
            var actualNumberOfTeams = _teamsRepository.GetAllTeamsAsync().Result.Count();

            actualNumberOfTeams.Should().Equals(expectedNumberOfTeams);
        }

        [Test]
        public void GetTeamByName_SearchForExistingTeamCaseSensetive_ReturnsArsenalTeam()
        {
            var expectedTeamName = "Arsenal";
            var actualTeam = _teamsRepository.GetTeamByNameAsync(expectedTeamName).Result;

            actualTeam.Name.Should().BeEquivalentTo(expectedTeamName);

        }

        [Test]
        public void GetTeamByName_SearchForExistingTeamCaseInsensitive_ReturnsArsenalTeam()
        {
            var expectedTeamName = "Arsenal";
            var actualTeam = _teamsRepository.GetTeamByNameAsync("arsenal").Result;

            actualTeam.Name.Should().BeEquivalentTo(expectedTeamName);

        }

        [Test]
        public void GetTeamByName_SearchNoneExistingTeam_ReturnsNull()
        {
            var actualTeam = _teamsRepository.GetTeamByNameAsync("CSKA").Result;

            actualTeam.Should().BeNull();
        }
    }
}
