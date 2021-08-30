using ProjectA.Models.PlayersModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories.PlayersRepository
{
    public interface IPlayersRepository
    {
        public Task<Players> GetPlayerDataAsync(string playerName);

        public Task<IEnumerable<Players>> GetAllPlayersAsync();
    }
}
