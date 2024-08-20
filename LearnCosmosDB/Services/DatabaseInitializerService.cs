using LearnCosmosDB.Models;

namespace LearnCosmosDB.Services
{
    public class DatabaseInitializerService : IHostedService
    {
        private readonly GlobalSettings _globalSettings;

        public DatabaseInitializerService(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _globalSettings.GetDatabaseAsync();
                await _globalSettings.GetContainerAsync("Family");  
                await _globalSettings.GetContainerAsync("Child"); 
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
