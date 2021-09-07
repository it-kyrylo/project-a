
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.States;

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
                StateType.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
        //public IState GetVehicle(StateTypes state)
        //{
        //    throw new NotImplementedException();
        //    // with a switch case the factory will return instance of a State classes we will create
        //}
    }
}
