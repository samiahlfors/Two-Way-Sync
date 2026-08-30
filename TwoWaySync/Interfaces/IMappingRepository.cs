using System;
using TwoWaySync.Models;

namespace TwoWaySync.Interfaces
{
    public interface IMappingRepository
    {
        EntityMapping GetEntityByLocalId (string entityType, Guid localId);
        EntityMapping GetEntityByRemoteId(string entityType, int remoteId);
        void SaveMapping();
        DateTime GetSyncStamp(string entityType);
        void SaveSyncStamp(string entityType, DateTime syncStamp);
    }
}