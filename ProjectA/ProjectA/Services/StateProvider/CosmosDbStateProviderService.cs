using Microsoft.Azure.Cosmos;
using ProjectA.Models.StateOfChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Services.StateProvider
{
    public class CosmosDbStateProviderService : ICosmosDbStateProviderService
    {
        private Container _container;
        public CosmosDbStateProviderService(CosmosClient cosmosDbClient, string databaseName, string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        private async Task<ChatState> GetContainerItemAsync(long Chat_Id)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.Chat_Id = @Chat_Id";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@Chat_Id", Chat_Id);
            FeedIterator<ChatState> query = this._container.GetItemQueryIterator<ChatState>(queryDefinition);

            var results = new List<ChatState>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            if(results.Count() != 0)
            {
                return results[0];
            }
            return null;
        }

        public async Task AddChatStateAsync(ChatState state)
        {
            await _container.CreateItemAsync(state, new PartitionKey(state.Chat_Id));
        }

        public async Task DeletechatStateAsync(long Chat_Id)
        {
            ChatState toDelete = await this.GetContainerItemAsync(Chat_Id);
            await _container.DeleteItemAsync<ChatState>(toDelete.Id, new PartitionKey(toDelete.Chat_Id));
        }

        public async Task<ChatState> GetChatStateAsync(long Chat_Id)
        {
            ChatState response = await this.GetContainerItemAsync(Chat_Id);
            return response;
        }

        public async Task UpdateChatStateAsync(ChatState item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(item.Chat_Id));
        }
    }
}
