using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.States;
using System;

namespace ProjectA.Factory
{
    public class StateFactory : IStateFactory
    {
        public IState GetVehicle(StateTypes state)
        {
            throw new NotImplementedException();
            // with a switch case the factory will return instance of a State classes we will create
        }
    }
}
