using AutoMapper;
using ProjectA.Models.Teams;
using ProjectA.Repositories.Teams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TeamServiceModel>> GetAllTeamsAsync()
        {
            return _mapper.Map<IEnumerable<TeamServiceModel>>(await _teamRepository.GetAllTeamsAsync());
        }

        public async Task<TeamServiceModel> GetStrongestTeamAwayAsync()
        {
            var allTeams = await _teamRepository.GetAllTeamsAsync();

            var strongestTeamAway = allTeams
                .OrderByDescending(sa=>sa.StrengthAway)
                .ThenByDescending(s=>s.Strength);

            
            return _mapper.Map<TeamServiceModel>(strongestTeamAway);
        }

        public async Task<TeamServiceModel> GetStrongestTeamHomeAsync()
        {
            var allTeams = await _teamRepository.GetAllTeamsAsync();

            var strongestTeamHome = allTeams
                .OrderByDescending(sh=>sh.StrengthHome)
                .ThenByDescending(s=>s.Strength);

            
            return _mapper.Map<TeamServiceModel>(strongestTeamHome);
        }

        public async Task<IEnumerable<TeamServiceModel>> GetTopThreeStrongestTeamsAsync()
        {
            var allTeams = await _teamRepository.GetAllTeamsAsync();

            var topThreeStrongestTeams = allTeams
                .OrderByDescending(s=>s.Strength)
                .ThenByDescending(sa=>sa.StrengthAway)
                .Take(3);

            
            return _mapper.Map<IEnumerable<TeamServiceModel>>(topThreeStrongestTeams);
        }

        public async Task<TeamServiceModel> GetTeamByNameAsync(string name)
        {
            var team =await _teamRepository.GetTeamByNameAsync(name);

            if (team!=null)
            {
                return _mapper.Map<TeamServiceModel>(team);
            }

            return new TeamServiceModel() { Name = "There is no such team" };
        }

        public async Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostDrawsAsync()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            var maxDraws = teams.Max(d => d.Draw);

            var teamsWithMostDraws = teams.Where(d =>d.Draw == maxDraws);


            return _mapper.Map<IEnumerable<TeamServiceModel>>(teamsWithMostDraws);
        }

        public async Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostLossesAsync()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            var maxLosses = teams.Max(l => l.Loss);

            var teamsWithMostLosses = teams.Where(l => l.Loss == maxLosses);


            return _mapper.Map<IEnumerable<TeamServiceModel>>(teamsWithMostLosses);
        }

        public async Task<IEnumerable<TeamServiceModel>> GetTeamsWithMostWinsAsync()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            var maxWins = teams.Max(w=>w.Win);

            var teamsWithMostWins = teams.Where(l => l.Win == maxWins);


            return _mapper.Map<IEnumerable<TeamServiceModel>>(teamsWithMostWins);
        }

     
    }
}
