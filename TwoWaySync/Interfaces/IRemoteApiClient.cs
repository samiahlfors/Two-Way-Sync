using System;
using System.Collections.Generic;
using TwoWaySync.Models.Remote;

namespace TwoWaySync.Interfaces
{
    public interface IRemoteApiClient
    {
        // GET /tasks/?last_modified_from=N&offset=N&page_size=N
        List<RemoteTaskDto> GetTasks(long lastModifiedFrom, int offset, int pageSize);
        // GET /tasks/{Id}
        RemoteTaskDto GetTask(int id);
        // PATCH /tasks/{Id}
        void UpdateTask(int id, object task);
        // POST /tasks/
        RemoteTaskDto CreateTask(RemoteTaskDto task);
        
        // GET /companies/?last_modified_from=N&offset=N&page_size=N&name=S
        List<RemoteCompanyDto> GetCompanies(long lastModifiedFrom, int offset = 0, int pageSize = 0, string name = null);
        // GET /companies/{Id}
        RemoteCompanyDto GetCompany(int id);
        // PATCH /companies/{Id}
        void UpdateCompany(int id, object company);
        // POST /companies/
        RemoteCompanyDto CreateCompany(RemoteCompanyDto company);
    }
}