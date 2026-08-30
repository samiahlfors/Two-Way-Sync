using System;

namespace TwoWaySync.Models
{
    public class EntityMapping
    {
        public string EntityType { get; set; }
        public Guid LocalId { get; set; }
        public int RemoteId { get; set; }
        public DateTime LastSynced { get; set; }
    }
}