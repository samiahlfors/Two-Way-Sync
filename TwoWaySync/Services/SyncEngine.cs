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
            
            // Get remote tasks updated after last sync date
            var remoteTasks = _apiClient.GetTasks(TimeZoneHelper.ConvertDateTimeToUnix(lastSyncUtc), 0, TasksPerRun);
            
            // Loop through all tasks
            foreach (var remoteTask in remoteTasks)
            {
                // Get mapping
                var entityMapping = _repository.GetEntityByRemoteId("Task", remoteTask.Id);
                
                // Get (or create) local company
                var localCompanyId = GetOrCreateLocalCompany(remoteTask.RelatedCompanyId);
                
                // If the mapping returns null, it's new and we can create it locally
                if (entityMapping == null)
                {
                    var localTask = _localApi.CreateOrUpdateTask(new LocalTask
                    {
                        Id = Guid.NewGuid(),
                        Contents = remoteTask.Body,
                        Deadline = TimeZoneHelper.ConvertToUtc(remoteTask.Deadline),
                        Completed = remoteTask.Finished,
                        CompletedDate = DateTime.UtcNow, // Remote task does not have a matching field
                        CompanyId = localCompanyId,
                        ChangedDate = DateTime.UtcNow
                    });
                    
                    // Save mapping
                    _repository.SaveMapping("Task", localTask.Id, remoteTask.Id);
                }
                else
                {
                    // The Task is known and has been changed on the remote side
                    // Since the task has the potential to be changed on both sides,
                    // we need to check if there are any local changes here as well
                    
                    // Get the local task
                    var localTask = _localApi.GetTaskById(entityMapping.LocalId);
                    
                    // Check if ChangedDate of localTask is "more recent" than the last sync date
                    // That means that the local task has been updated, and this needs to reflect on the remote side
                    if (localTask.ChangedDate > entityMapping.LastSynced)
                    {
                        // Update both sides, both local and remote
                        
                        // Local task
                        localTask.Completed = remoteTask.Finished;
                        localTask.CompanyId = localCompanyId;
                        _localApi.CreateOrUpdateTask(localTask);
                        
                        // Remote task
                        _apiClient.UpdateTask(entityMapping.RemoteId, new
                        {
                            body = localTask.Contents,
                            deadline = TimeZoneHelper.ConvertToSwedishTime(remoteTask.Deadline),
                        });
                    }
                    else
                    {
                        // The local task was not updated, so we can sync the remote task and merge it with the local one
                        localTask.Contents = remoteTask.Body;
                        localTask.Deadline = TimeZoneHelper.ConvertToUtc(remoteTask.Deadline);
                        localTask.Completed = remoteTask.Finished;
                        localTask.CompanyId = localCompanyId;
                        
                        // Save the task
                        _localApi.CreateOrUpdateTask(localTask);
                    }
                }
            }
            
            // Save current time as "last_sync_remote_task"
            // This should save the last processed task date
            // Only save if any changes were made
            if (remoteTasks.Count > 0)
            {
                _repository.SaveSyncStamp("last_sync_remote_task", remoteTasks.Max(task => task.LastModifiedDate));   
            }
        }

        private void SyncTasksLocalToRemote()
        {
            // Get the last sync time, and convert it to UTC
            // This should keep track of what's being synced at the time as well.
            var lastSync = _repository.GetSyncStamp("last_sync_local_task");
            var lastSyncSwe = TimeZoneHelper.ConvertToUtc(lastSync);
            
            // Get local tasks updated after last sync date
            var localTasks = _localApi.GetTasks(lastSyncSwe, DateTime.Now);
            
            // Loop through all tasks
            foreach (var localTask in localTasks)
            {
                // Get mapping
                var entityMapping = _repository.GetEntityByLocalId("Task", localTask.Id);
                
                // Get (or create) remote company
                var remoteCompanyId = GetOrCreateRemoteCompany(localTask.CompanyId);
                
                // If the entity mapping does not exist, we have not yet created it on the remote server
                if (entityMapping == null)
                {
                    // Create remote task
                    // Local task time zone is UTC, convert to Swedish time 
                    var remoteTask = _apiClient.CreateTask(new RemoteTaskDto
                    {
                        Body = localTask.Contents,
                        Deadline = TimeZoneHelper.ConvertToSwedishTime(localTask.Deadline),
                        Finished = localTask.Completed,
                        RelatedCompanyId = remoteCompanyId
                    });
                    
                    // Save mapping
                    _repository.SaveMapping("Task", localTask.Id, remoteTask.Id);
                }
                else
                {
                    // The Task has been updated locally and needs to be updated on the remote server
                    _apiClient.UpdateTask(entityMapping.RemoteId, new
                    { 
                        body = localTask.Contents,
                        deadline = TimeZoneHelper.ConvertToSwedishTime(localTask.Deadline),
                        finished = localTask.Completed,
                        related_company_id = remoteCompanyId
                    });
                }
            }
            
            // Save current time as "last_sync_local_task"
            // This should save the last processed task date
            // Only save if any changes were made
            if (localTasks.Count > 0)
            {
                _repository.SaveSyncStamp("last_sync_local_task", localTasks.Max(task => task.ChangedDate));   
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
                    // Local API does not have an update company method, create a new one
                    _localApi.CreateCompany(remoteCompany.Name);
                }
            }
            
            _repository.SaveSyncStamp("last_sync_remote_company", DateTime.UtcNow);
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