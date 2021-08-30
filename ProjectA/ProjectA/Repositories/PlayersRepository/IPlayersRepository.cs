using ProjectA.Models.PlayersModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories.PlayersRepository
{
    public interface IPlayersRepository
    {
        public Task<Element> GetPlayerDataAsync(string playerName);

        public Task<IEnumerable<Element>> GetAllPlayersAsync();
    }
}
