using System;
using TwoWaySync.Interfaces;

namespace TwoWaySync.Services
{
    public class SyncEngine
    {
        private const int TasksPerRun = 100;
        private IMappingRepository _repository;
        private IRemoteApiClient _apiClient;
        private ILocalApi _localApi;

        public SyncEngine(IMappingRepository repository, IRemoteApiClient apiClient, ILocalApi localApi)
        {
            _repository = repository;
            _apiClient = apiClient;
            _localApi = localApi;
        }
        
        // Tasks
        private void SyncTasksRemoteToLocal()
        {
            // Get the last sync time, and convert it to UTC
            // This should keep track of what's being synced at the time as well.
            var lastSync = _repository.GetSyncStamp("last_sync_remote_task");
            var lastSyncUtc = TimeZoneHelper.ConvertToUtc(lastSync);
            
            // Get tasks after said sync date
            var tasks = _apiClient.GetTasks(lastSyncUtc, 0, TasksPerRun);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Check if company already exists, else, create a new one
                var remoteCompany = _apiClient.GetCompany(task.RelatedCompanyId);
                // var localCompany = 
            }
            
            // Save current time as "last_sync"
            // This should save the last processed task date
            // Only save if any changes were made
            _repository.SaveSyncStamp("last_sync_remote_task", DateTime.Now);
        }

        private void SyncTasksLocalToRemote()
        {
            // Get the last sync time, and convert it to UTC
            // This should keep track of what's being synced at the time as well.
            var lastSync = _repository.GetSyncStamp("last_sync_local_task");
            var lastSyncSwe = TimeZoneHelper.ConvertToUtc(lastSync);
            
            // Get tasks after said sync date
            var tasks = _localApi.GetTasks(lastSyncSwe, DateTime.Now);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Check if company already exists, else, create a new one
                var localCompany = _localApi.GetCompanyById(task.CompanyId);
            }
            
            // Save current time as "last_sync"
            // This should save the last processed task date
            // Only save if any changes were made
            _repository.SaveSyncStamp("last_sync_local_task", DateTime.Now);
        }
        
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