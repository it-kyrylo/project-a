using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.States;

namespace ProjectA.Factory
{
    public interface IStateFactory
    {
        IState GetState(StateType state);
    }
}
