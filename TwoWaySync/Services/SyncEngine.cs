using System;
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
        
        // Tasks
        private void SyncTasksRemoteToLocal()
        {
            // Get the last sync time
            // This should keep track of what's being synced at the time as well.
            var lastSync = _repository.GetSyncStamp("last_sync");
            
            // Get tasks after said sync date
            var tasks = _apiClient.GetTasks(lastSync, 0, TasksPerRun);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Check if company already exists, else, create a new one
            }
            
            // Save current time as "last_sync"
            // This should save the last processed task date
            // Only save if any changes were made
            _repository.SaveSyncStamp("last_sync", DateTime.Now);
        }
        private void SyncTasksLocalToRemote() {}
        
        // Companies
        private void SyncCompaniesRemote() {}
        private void SyncCompaniesLocal() {}
        
        // Called every N minutes
        private void Run()
        {
            SyncTasksRemoteToLocal();
            SyncTasksLocalToRemote();
            SyncCompaniesRemote();
            SyncCompaniesLocal();
        }
    }
}