using ProjectA.Models.StateOfChatModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectA.Services.StateProvider
{
    public interface ICosmosDbStateProviderService
    {
        Task AddChatStateAsync(ChatState state);

        Task DeletechatStateAsync(long Chat_Id);

        Task<ChatState> GetChatStateAsync(long Chat_Id);

        Task UpdateChatStateAsync(ChatState item);
    }
}
