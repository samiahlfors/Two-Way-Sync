using System.Collections.Generic;
using System.Net.Http;
using TwoWaySync.Interfaces;
using TwoWaySync.Models.Remote;

namespace TwoWaySync.Services
{
    public class RemoteApiClient : IRemoteApiClient
    {
        
        private readonly HttpClient _httpClient;

        public RemoteApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public RemoteTaskDto GetTasks(int id)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTask(int id)
        {
            throw new System.NotImplementedException();
        }

        public RemoteTaskDto CreateTask(RemoteTaskDto task)
        {
            throw new System.NotImplementedException();
        }

        public List<RemoteCompanyDto> GetCompanies(int lastModifiedFrom, int offset, int pageSize, string name)
        {
            throw new System.NotImplementedException();
        }

        public RemoteCompanyDto GetCompanies(int id)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCompany(int id)
        {
            throw new System.NotImplementedException();
        }

        public RemoteCompanyDto CreateCompany(RemoteCompanyDto company)
        {
            throw new System.NotImplementedException();
        }

        List<RemoteTaskDto> IRemoteApiClient.GetTasks(int lastModifiedFrom, int offset, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}