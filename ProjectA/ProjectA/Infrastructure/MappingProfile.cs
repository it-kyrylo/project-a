using AutoMapper;
using ProjectA.Models.Teams;

namespace ProjectA.Infrastructure
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamServiceModel>();
        }
    }
}
