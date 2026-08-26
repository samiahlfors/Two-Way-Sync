using System;

namespace TwoWaySync.Models.Internal
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}