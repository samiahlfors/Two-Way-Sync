using TwoWaySync.Interfaces;

namespace TwoWaySync.Services
{
    public class SyncEngine
    {
        private const int TasksPerRun = 100;
        private IMappingRepository _repository;
        private IRemoteApiClient _apiClient;

        public SyncEngine(IMappingRepository repository, IRemoteApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }
    }
}