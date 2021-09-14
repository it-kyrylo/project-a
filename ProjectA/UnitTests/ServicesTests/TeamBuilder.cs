using ProjectA.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ServicesTests
{
    public class TeamBuilder
    {

        private TeamServiceModel team;

        private TeamBuilder()
        {
            this.team = new TeamServiceModel();
        }

        public static TeamBuilder Builder()
        {
            return new TeamBuilder();
        }

        public TeamBuilder SetName(String name)
        {
            this.team.Name = name;
            return this;
        }

        public TeamBuilder SetStrength(int strength)
        {
            this.team.Strength = strength;
            return this;
        }

        public TeamBuilder SetStrengthHome(int strengthHome)
        {
            this.team.StrengthHome = strengthHome;
            return this;
        }

        public TeamServiceModel Build()
        {
            return this.team;
        }
    }
}
