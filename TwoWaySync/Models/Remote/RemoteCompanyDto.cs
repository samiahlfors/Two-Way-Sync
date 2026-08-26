using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TwoWaySync.Models.Remote
{
    // https://stackoverflow.com/a/54540599
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class RemoteCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}