using System;
using System.Linq;
using TwoWaySync.Interfaces;
using TwoWaySync.Models.Internal;
using TwoWaySync.Models.Remote;

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
            
            // Get remote tasks updated after said sync date
            var tasks = _apiClient.GetTasks(lastSyncUtc, 0, TasksPerRun);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Get the remote company
                var remoteCompany = _apiClient.GetCompany(task.RelatedCompanyId);
                
                // Get (or create) the corresponding local company
                var localCompany = GetOrCreateLocalCompany(remoteCompany);
                
                // Get local task (ID)
                var localTaskId = _repository.GetEntityByRemoteId("Task", task.Id).LocalId;
                
                // If task found, update task
                // Else, create a new one
            }
            
            // Save current time as "last_sync_remote_task"
            // This should save the last processed task date
            // Only save if any changes were made
            if (tasks.Count > 0)
            {
                _repository.SaveSyncStamp("last_sync_remote_task", tasks.Max(task => task.LastModifiedDate));   
            }
        }

        private void SyncTasksLocalToRemote()
        {
            // Get the last sync time, and convert it to UTC
            // This should keep track of what's being synced at the time as well.
            var lastSync = _repository.GetSyncStamp("last_sync_local_task");
            var lastSyncSwe = TimeZoneHelper.ConvertToUtc(lastSync);
            
            // Get local tasks updated after said sync date
            var tasks = _localApi.GetTasks(lastSyncSwe, DateTime.Now);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Get the local company
                var localCompany = _localApi.GetCompanyById(task.CompanyId);
                
                // Get (or create) the corresponding remote company
                var remoteCompany = GetOrCreateRemoteCompany(localCompany);
                
                // Get remote task (ID)
                var remoteTaskId = _repository.GetEntityByLocalId("Task", task.Id);
                
                // If task found, update task
                // Else, create a new one
            }
            
            // Save current time as "last_sync_local_task"
            // This should save the last processed task date
            // Only save if any changes were made
            if (tasks.Count > 0)
            {
                _repository.SaveSyncStamp("last_sync_local_task", tasks.Max(task => task.ChangedDate));   
            }
        }
        
        // Companies
        private void SyncCompaniesLocalToRemote()
        {
            // TODO: Make sure this gets executed first, since "Membrain is the master for any given property"
            // Get last sync
            // Get and loop through local companies
            // If remote name does not match with local name, update company remotely
            // TODO: Save sync stamp here, I think..?
        }

        private void SyncCompaniesRemoteToLocal()
        {
            // Get last sync
            // Get and loop through remote companies
            // If local name does not match with remote name, update local company
            // TODO: Save sync stamp here, I think..?
        }
        
        // Get or create new company
        private int GetOrCreateRemoteCompany(Company company)
        {
            // Either return a company ID if it exists, or create a new one and return that
            return 0;
        }

        private Guid GetOrCreateLocalCompany(RemoteCompanyDto remoteCompany)
        {
            // Either return a company ID if it exists, or create a new one and return that
            return Guid.NewGuid();
        }
        
        // Called every N minutes
        // TODO: Check if this should be renamed to Execute or similar
        private void Run()
        {
            // Run local company name syncing first
            SyncCompaniesLocalToRemote();
            SyncCompaniesRemoteToLocal();
            SyncTasksRemoteToLocal();
            SyncTasksLocalToRemote();
        }
    }
}