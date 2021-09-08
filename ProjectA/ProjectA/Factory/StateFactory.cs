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
        private readonly IPlayerSuggestionService players;
        private readonly IStateTeamService teamService;
        public StateFactory(ICosmosDbStateProviderService stateProvider, IPlayerSuggestionService players,IStateTeamService teamService)
        private readonly IStatisticsService statisticsService;

        public StateFactory(ICosmosDbStateProviderService stateProvider, IPlayerSuggestionService players, IStatisticsService statisticsService)
        {
            this.stateProvider = stateProvider;
            this.players = players;
            this.statisticsService = statisticsService;
        }

        public IState GetState(StateType state)
        {
            var result = state switch
            {
                StateType.SuggestionsMenuState => new SuggestionsMenuState(stateProvider),
                StateType.PlayersByOverallStatsState => new PlayersByOverallStatsState(stateProvider, players),
                StateType.PlayersByFormState => new PlayersByFormState(stateProvider, players),
                StateType.PlayersByPointsPerGameState => new PlayersByPointsPerGameState(stateProvider, players),
                StateType.PlayersByITCRank => new PlayersByITCRank(stateProvider, players),
                StateType.PlayersByPointsPerPriceState => new PlayersByPointsPerPriceState(stateProvider, players),
                StateType.StatisticsMenuState => new StatisticsMenuState(stateProvider),
                StateType.PlayerDataState => new PlayerDataState(stateProvider, statisticsService),
                StateType.PlayersInDreamTeamOfTeamState => new PlayersInDreamTeamOfTeamState(stateProvider, statisticsService),
                StateType.TimesPlayerHasBeenInDreamTeamState => new TimesPlayerHasBeenInDreamTeamState(stateProvider, statisticsService),
                StateType.TopScorersInTeamMenuState => new TopScorersInTeamMenuState(stateProvider, statisticsService),
                StateType.TopScorersState => new TopScorersState(stateProvider, statisticsService),
                StateType.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
    }
}
