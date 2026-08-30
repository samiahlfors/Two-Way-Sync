using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwoWaySync.Models.Remote;

namespace TwoWaySync.Services
{
    public class RemoteApiClient
    {
        
        private readonly HttpClient _httpClient;

        public RemoteApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        // GET /tasks/?last_modified_from=N&offset=N&page_size=N
        public async Task<string> GetTasks(int lastModifiedFrom, int offset = 0, int pageSize = 100)
        {
            await _httpClient.GetAsync("tasks/");
            return "success, hopefully";
        }
    }
}