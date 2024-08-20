using LearnCosmosDB.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace LearnCosmosDB.Models
{
    public class GlobalSettings
    {
        private readonly CosmosSettings _cosmosDbSettings;
        private CosmosClient _client;
        public GlobalSettings(IOptions<CosmosSettings> cosmosDbSettings)
        {
            _cosmosDbSettings = cosmosDbSettings.Value; 
            _client = new CosmosClient(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.AccountKey);
        }

        public async Task<Database> GetDatabaseAsync()
        {
            try
            { 
                var response = await _client.CreateDatabaseIfNotExistsAsync(_cosmosDbSettings.DatabaseName);
                return response.Database;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Container> GetContainerAsync(string containerName)
        {
            try
            { 
                var database = await _client.GetDatabase(_cosmosDbSettings.DatabaseName).ReadAsync();
                var response = await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
                return response.Container;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
