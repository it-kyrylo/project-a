using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.Handlers;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.States;
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
        {
            this.stateProvider = stateProvider;
            this.players = players;
            this.teamService = teamService;
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
                StateType.TeamsMenuState=> new TeamsMenuState(stateProvider),
                StateType.SearchTeamState => new SearchTeamState(stateProvider,teamService),
                StateType.AllTeamsState => new AllTeamsState(stateProvider,teamService),
                StateType.TopThreeTeamsState => new TopThreeTeamsState(stateProvider,teamService),
                StateType.MostWinsTeamState => new MostWinsTeamState(stateProvider,teamService),
                StateType.MostLossesTeamState => new MostLossesTeamState(stateProvider,teamService),
                StateType.StrongestTeamHomeState => new StrongestTeamHomeState(stateProvider,teamService),
                StateType.StrongestTeamAwayState => new StrongestTeamAwayState(stateProvider,teamService),
                StateType.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
    }
}
