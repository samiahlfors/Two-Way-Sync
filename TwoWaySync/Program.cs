using System;
using System.Net.Http;
using TwoWaySync.Services;

namespace TwoWaySync
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            var apiClient = new RemoteApiClient(httpClient);
            
            try
            {
                var tasks = apiClient.GetTasks(lastModifiedFrom: 0, offset: 0);
                Console.WriteLine(tasks.Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}