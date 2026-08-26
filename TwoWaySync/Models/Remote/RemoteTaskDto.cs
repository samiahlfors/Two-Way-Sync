using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TwoWaySync.Models.Remote
{
    // https://stackoverflow.com/a/54540599
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class RemoteTaskDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Deadline { get; set; }
        public bool Finished { get; set; }
        public int RelatedCompanyId  { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}