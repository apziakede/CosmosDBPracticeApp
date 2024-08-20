using LearnCosmosDB.Models;
using Microsoft.Azure.Cosmos;
using MongoDB.Driver;

namespace LearnCosmosDB.Repositories
{
    public class FamilyRepository
    {
        private readonly Container _container;

        public FamilyRepository(Container container)
        {
            _container = container;
        }

        public async Task<List<Family>> GetAllFamiliesAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Family>(query);
            var families = new List<Family>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                families.AddRange(response);
            }

            return families;
        }

        public async Task<Family> GetFamilyByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Family>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task CreateFamilyAsync(Family family)
        {
            await _container.CreateItemAsync(family, new PartitionKey(family.id));
        }

        public async Task UpdateFamilyAsync(string id, Family family)
        {
            await _container.UpsertItemAsync(family, new PartitionKey(id));
        }

        public async Task DeleteFamilyAsync(string id)
        {
            await _container.DeleteItemAsync<Family>(id, new PartitionKey(id));
        }
    }
}
