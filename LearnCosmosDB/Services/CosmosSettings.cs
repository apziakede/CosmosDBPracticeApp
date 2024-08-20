using Microsoft.Azure.Cosmos;

namespace LearnCosmosDB.Services
{
    public class CosmosSettings
    {
        public string ConnectionString { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    } 
}
