using System;

namespace TwoWaySync.Models.Internal
{
    public class LocalTask
    {
        public Guid Id { get; set; }
        public string Contents { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedDate { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}