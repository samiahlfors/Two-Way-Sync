using System;
using TwoWaySync.Interfaces;
using TwoWaySync.Models;

namespace TwoWaySync.Services
{
    public class MappingRepository : IMappingRepository
    {
        public EntityMapping GetEntityByLocalId(string entityType, Guid localId) => new EntityMapping();
        public EntityMapping GetEntityByRemoteId(string entityType, int remoteId) => new EntityMapping();
        public void SaveMapping(string entityType, Guid localId, int remoteId) { }
        public DateTime GetSyncStamp(string entityType) => DateTime.Now;
        public void SaveSyncStamp(string entityType, DateTime syncStamp) { }
    }
}