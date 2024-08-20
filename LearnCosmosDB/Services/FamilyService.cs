using LearnCosmosDB.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace LearnCosmosDB.Services
{
    public interface IFamilyService
    {
        Task<ItemResponse<Family>> CreateFamilyAsync(Family item);
        Task<Family> GetFamilyAsync(string id, string partitionKey);
        Task<IEnumerable<Family>> GetFamiliesAsync();
        Task DeleteFamilyAsync(string id, string partitionKey);
        Task<ItemResponse<Family>> UpdateFamilyAsync(Family family);
    }


    public class FamilyService : IFamilyService
    {
        private readonly CosmosClient _client;
        private readonly Container _container; 
          
        public FamilyService(IOptions<CosmosSettings> cosmosDbSettings)
        {
            _client = new CosmosClient(cosmosDbSettings.Value.ConnectionString, cosmosDbSettings.Value.AccountKey); 
            _container = _client.GetContainer(cosmosDbSettings.Value.DatabaseName,"Family");
        }
         
        public async Task<ItemResponse<Family>> CreateFamilyAsync(Family item)
        {
            return await _container.CreateItemAsync(item, new PartitionKey(item.id));
        }

        public async Task<ItemResponse<Family>> UpdateFamilyAsync(Family family)
        {
           return await _container.UpsertItemAsync(family, new PartitionKey(family.id));
        }

        public async Task<Family> GetFamilyAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<Family>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Family>> GetFamiliesAsync()
        {
            var query = _container.GetItemQueryIterator<Family>();
            var results = new List<Family>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task DeleteFamilyAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<Family>(id, new PartitionKey(partitionKey));
        }
    }
}
