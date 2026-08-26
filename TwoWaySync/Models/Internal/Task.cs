using System;

namespace TwoWaySync.Models.Internal
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}