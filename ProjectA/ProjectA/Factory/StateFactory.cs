using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.States;
using ProjectA.States.PlayersSuggestion;

namespace ProjectA.Factory
{
    public class StateFactory : IStateFactory
    {
        private readonly ICosmosDbStateProviderService stateProvider;
        private readonly IPlayerSuggestionService players;

        public StateFactory(ICosmosDbStateProviderService stateProvider, IPlayerSuggestionService players)
        {
            this.stateProvider = stateProvider;
            this.players = players;
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
                StateType.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
    }
}
