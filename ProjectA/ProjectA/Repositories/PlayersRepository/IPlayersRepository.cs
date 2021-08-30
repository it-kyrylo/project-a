using ProjectA.Models.PlayersModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories.PlayersRepository
{
    public interface IPlayersRepository
    {
        public Task<Player> GetPlayerDataAsync(string playerName);

        public Task<IEnumerable<Player>> GetAllPlayersAsync();
    }
}
