using System;
using System.Collections.Generic;
using TwoWaySync.Interfaces;
using TwoWaySync.Models.Remote;

namespace TwoWaySync.Services
{
    public class RemoteApiClient : IRemoteApiClient
    {
        public List<RemoteTaskDto> GetTasks(DateTime lastModifiedFrom, int offset, int pageSize) => new List<RemoteTaskDto>();
        public RemoteTaskDto GetTask(int id) => new RemoteTaskDto();
        public void UpdateTask(int id, object task) { }
        public RemoteTaskDto CreateTask(RemoteTaskDto task) => new RemoteTaskDto();
        public List<RemoteCompanyDto> GetCompanies(DateTime lastModifiedFrom, int offset, int pageSize, string name) => new List<RemoteCompanyDto>();
        public RemoteCompanyDto GetCompany(int id) => new RemoteCompanyDto();
        public void UpdateCompany(int id, object company) { }
        public RemoteCompanyDto CreateCompany(RemoteCompanyDto company) => new RemoteCompanyDto();
    }
}