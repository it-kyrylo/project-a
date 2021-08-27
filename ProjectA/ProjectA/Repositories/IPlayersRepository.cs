
using ProjectA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Repositories
{
    public interface IPlayersRepository<T> where T : class
    {
        public Task<PlayersRepository<T>> Create();

        public T GetPlayerData(string playerName);

        public int GetPlayerId(string playerName);

        public IEnumerable<T> GetAllPlayers();
    }
}
