using System;
using TwoWaySync.Models;

namespace TwoWaySync.Interfaces
{
    // Map local and remote tasks and companies as well as Sync Stamps
    public interface IMappingRepository
    {
        // Usage: GetEntityByLocalId("Task", new Guid("8f825fd0-c3a2-4000-9941-89a0ba056ee4"))
        // Usage: GetEntityByRemoteId("Company", 45)
        // This should return an EntityMapping object
        EntityMapping GetEntityByLocalId (string entityType, Guid localId);
        EntityMapping GetEntityByRemoteId(string entityType, int remoteId);
        
        // This is to save and keep track of mappings
        void SaveMapping(string entityType, Guid localId, int remoteId);
        
        // Sync stamps are used to keep track of specific synchronisations
        // I.e. local tasks or remote companies
        DateTime GetSyncStamp(string entityType);
        void SaveSyncStamp(string entityType, DateTime syncStamp);
    }
}