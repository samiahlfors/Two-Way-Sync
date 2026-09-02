using System;
using System.Linq;
using TwoWaySync.Interfaces;
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
            var tasks = _apiClient.GetTasks(TimeZoneHelper.ConvertDateTimeToUnix(lastSyncUtc), 0, TasksPerRun);
            
            // Loop through all tasks
            foreach (var task in tasks)
            {
                // Get the remote company
                var remoteCompany = _apiClient.GetCompany(task.RelatedCompanyId);
                
                // Get (or create) the corresponding local company
                var localCompany = GetOrCreateLocalCompany(remoteCompany.Id);
                
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
                var remoteCompany = GetOrCreateRemoteCompany(localCompany.Id);
                
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
            // Get last sync
            var lastSync = _repository.GetSyncStamp("last_sync_local_company");
            
            // Get and loop through local companies
            var localCompanies = _localApi.GetCompanies(lastSync, DateTime.UtcNow);
            foreach (var localCompany in localCompanies)
            {
                // Find correct mapping to match remote company with local
                var entityMapping = _repository.GetEntityByLocalId("Company", localCompany.Id);
                if (entityMapping == null) continue; // Meaning, nothing to sync
                
                // If remote name does not match with local name, update company remotely
                var remoteCompany = _apiClient.GetCompany(entityMapping.RemoteId);
                if (localCompany.Name != remoteCompany.Name)
                {
                    _apiClient.UpdateCompany(entityMapping.RemoteId, new { name = localCompany.Name });
                }
            }

            _repository.SaveSyncStamp("last_sync_local_company", DateTime.UtcNow);
        }

        private void SyncCompaniesRemoteToLocal()
        {
            // Get last sync
            var lastSync = _repository.GetSyncStamp("last_sync_remote_company");
            
            // Get remote companies that have been modified after last sync
            var remoteCompanies = _apiClient.GetCompanies(TimeZoneHelper.ConvertDateTimeToUnix(lastSync));
            foreach (var remoteCompany in remoteCompanies)
            {
                // Find correct mapping to match local company with the remote one
                var entityMapping = _repository.GetEntityByRemoteId("Company", remoteCompany.Id);
                if (entityMapping == null) continue; // Nothing to sync
                
                // If the local name doesn't match with the remote one, update company
                var localCompany = _localApi.GetCompanyById(entityMapping.LocalId);
                if (localCompany.Name != remoteCompany.Name)
                {
                    // Update company
                }
            }
        }
        
        // Get or create new company
        private int GetOrCreateRemoteCompany(Guid id)
        {
            // Get the entity mapping, and if it exists, just return the ID
            var entityMapping = _repository.GetEntityByLocalId("Company", id);
            if (entityMapping != null) return entityMapping.RemoteId;

            // Get local company if it doesn't exist remotely
            var localCompany = _localApi.GetCompanyById(id);
            
            // Make sure it exists remotely
            var remoteCompany = _apiClient.GetCompanies(0, 0, 1, localCompany.Name).FirstOrDefault();
            
            // If it doesn't, create it on the remote server
            if (remoteCompany == null)
            {
                remoteCompany = _apiClient.CreateCompany(new RemoteCompanyDto { Name = localCompany.Name });
            }
            
            // Save the mapping
            _repository.SaveMapping("Company", id, remoteCompany.Id);
            
            // Return the ID
            return remoteCompany.Id;
        }

        private Guid GetOrCreateLocalCompany(int id)
        {
            // Get the entity mapping, and if it exists, just return the ID
            var entityMapping = _repository.GetEntityByRemoteId("Company", id);
            if (entityMapping != null) return entityMapping.LocalId;
            
            // If the company does not exist, get it from the client
            var remoteCompany = _apiClient.GetCompany(id);
            
            // Check if company exists locally
            var localCompany = _localApi.GetCompanyByName(remoteCompany.Name);
            
            // If it doesn't, create it
            if (localCompany == null)
            {
                localCompany = _localApi.CreateCompany(remoteCompany.Name);
            }
            
            // Save the mapping
            _repository.SaveMapping("Company", localCompany.Id, id);
            
            // Finally, return the ID
            return localCompany.Id;
        }
        
        // Called every N minutes
        public void Execute()
        {
            // Run local company name syncing first
            SyncCompaniesLocalToRemote();
            SyncCompaniesRemoteToLocal();
            SyncTasksRemoteToLocal();
            SyncTasksLocalToRemote();
        }
    }
}