using System.Collections.Generic;
using TwoWaySync.Models.Remote;

namespace TwoWaySync.Interfaces
{
    public interface IRemoteApiClient
    {
        // GET /tasks/?last_modified_from=N&offset=N&page_size=N
        List<RemoteTaskDto> GetTasks(int lastModifiedFrom, int offset, int pageSize);
        // GET /tasks/{Id}
        RemoteTaskDto GetTasks(int id);
        // PATCH /tasks/{Id}
        void UpdateTask(int id); // TODO: Also pass in some data here
        // POST /tasks/
        RemoteTaskDto CreateTask(RemoteTaskDto task);
        
        // GET /companies/?last_modified_from=N&offset=N&page_size=N&name=S
        List<RemoteCompanyDto> GetCompanies(int lastModifiedFrom, int offset, int pageSize, string name);
        // GET /companies/{Id}
        RemoteCompanyDto GetCompanies(int id);
        // PATCH /companies/{Id}
        void UpdateCompany(int id); // TODO: Also pass in some data here
        // POST /companies/
        RemoteCompanyDto CreateCompany(RemoteCompanyDto company);
    }
}