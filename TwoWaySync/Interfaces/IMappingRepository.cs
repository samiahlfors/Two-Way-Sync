using System;
using TwoWaySync.Models;

namespace TwoWaySync.Interfaces
{
    public interface IMappingRepository
    {
        EntityMapping GetEntityByLocalId (string entityType, Guid localId);
        EntityMapping GetEntityByRemoteId(string entityType, int remoteId);
    }
}