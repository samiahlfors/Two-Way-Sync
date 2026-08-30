using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwoWaySync.Services
{
    public class RemoteApiClient
    {
        
        private readonly HttpClient _httpClient;

        public RemoteApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> GetTasks()
        {
            await _httpClient.GetAsync("tasks/");
            return "success, hopefully";
        }
    }
}