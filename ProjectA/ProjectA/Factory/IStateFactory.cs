using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Factory
{
    public interface IStateFactory
    {
        public  IState GetVehicle(StateTypes state);
    }
}
