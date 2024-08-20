using LearnCosmosDB.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace LearnCosmosDB.Services
{
    public interface IChildService
    {
        Task<ItemResponse<Child>> CreateChildAsync(Child child);
        Task<Child> GetChildAsync(string id, string partitionKey);
        Task<IEnumerable<Child>> GetChildrenAsync();
        Task DeleteChildAsync(string id, string partitionKey);
        Task<ItemResponse<Child>> UpdateChildAsync(Child child);
    }

    public class ChildService : IChildService
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public ChildService(IOptions<CosmosSettings> cosmosDbSettings)
        {
            _client = new CosmosClient(cosmosDbSettings.Value.ConnectionString, cosmosDbSettings.Value.AccountKey);
            _container = _client.GetContainer(cosmosDbSettings.Value.DatabaseName, "Child");
        }
        public async Task<ItemResponse<Child>> CreateChildAsync(Child child)
        {
            return await _container.CreateItemAsync(child, new PartitionKey(child.id));
        }

        public async Task<ItemResponse<Child>> UpdateChildAsync(Child child)
        {
            return await _container.UpsertItemAsync(child, new PartitionKey(child.id));
        }

        public async Task DeleteChildAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<Child>(id, new PartitionKey(partitionKey));
        }

        public async Task<Child> GetChildAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<Child>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Child>> GetChildrenAsync()
        {
            var query = _container.GetItemQueryIterator<Child>();
            var results = new List<Child>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
