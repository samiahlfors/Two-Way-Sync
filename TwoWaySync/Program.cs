using TwoWaySync.Services;

namespace TwoWaySync
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Setup database repository
            var mappingRepository = new MappingRepository();
            
            // Setup Remote API Client
            var remoteApiClient = new RemoteApiClient();
            
            // Setup Local API
            var localApi = new LocalApi();
            
            // Run Sync Engine
            var syncEngine = new SyncEngine(mappingRepository, remoteApiClient, localApi);
            syncEngine.Execute();
        }
    }
}