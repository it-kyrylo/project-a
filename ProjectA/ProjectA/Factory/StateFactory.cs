using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.Handlers;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using ProjectA.States;
using ProjectA.States.PlayersStatistics;
using ProjectA.States.PlayersSuggestion;
using ProjectA.States.TeamsStatistic;

namespace ProjectA.Factory
{
    public class StateFactory : IStateFactory
    {
        private readonly ICosmosDbStateProviderService stateProvider;
        private readonly IPlayerSuggestionService playerSuggestionService;
        private readonly IStateTeamService teamService;
        private readonly IStatisticsService statisticsService;

        public StateFactory(
            ICosmosDbStateProviderService stateProvider, 
            IPlayerSuggestionService playerSuggestionService, 
            IStatisticsService statisticsService, 
            IStateTeamService teamService)
        {
            this.stateProvider = stateProvider;
            this.playerSuggestionService = playerSuggestionService;
            this.statisticsService = statisticsService;
            this.teamService = teamService;
        }

        public IState GetState(StateType state)
        {
            var result = state switch
            {
                StateType.SuggestionsMenuState => new SuggestionsMenuState(stateProvider),
                StateType.PlayersByOverallStatsState => new PlayersByOverallStatsState(stateProvider, playerSuggestionService),
                StateType.PlayersByFormState => new PlayersByFormState(stateProvider, playerSuggestionService),
                StateType.PlayersByPointsPerGameState => new PlayersByPointsPerGameState(stateProvider, playerSuggestionService),
                StateType.PlayersByITCRank => new PlayersByITCRank(stateProvider, playerSuggestionService),
                StateType.PlayersByPointsPerPriceState => new PlayersByPointsPerPriceState(stateProvider, playerSuggestionService),

                StateType.StatisticsMenuState => new StatisticsMenuState(stateProvider),
                StateType.PlayerDataState => new PlayerDataState(stateProvider, statisticsService),
                StateType.PlayersInDreamTeamOfTeamState => new PlayersInDreamTeamOfTeamState(stateProvider, statisticsService),
                StateType.PlayersOfPositionInTeamState => new PlayersOfPositionInTeamState(stateProvider, statisticsService),
                StateType.TimesPlayerHasBeenInDreamTeamState => new TimesPlayerHasBeenInDreamTeamState(stateProvider, statisticsService),
                StateType.TopScorersInTeamState => new TopScorersInTeamState(stateProvider, statisticsService),
                StateType.TopScorersState => new TopScorersState(stateProvider, statisticsService),

                StateType.TeamsMenuState => new TeamsMenuState(stateProvider),
                StateType.SearchTeamState => new SearchTeamState(stateProvider, teamService),
                StateType.AllTeamsState => new AllTeamsState(stateProvider, teamService),
                StateType.TopThreeTeamsState => new TopThreeTeamsState(stateProvider, teamService),
                StateType.MostWinsTeamState => new MostWinsTeamState(stateProvider, teamService),
                StateType.MostLossesTeamState => new MostLossesTeamState(stateProvider, teamService),
                StateType.StrongestTeamHomeState => new StrongestTeamHomeState(stateProvider, teamService),
                StateType.StrongestTeamAwayState => new StrongestTeamAwayState(stateProvider, teamService),

                StateType.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
    }
}
