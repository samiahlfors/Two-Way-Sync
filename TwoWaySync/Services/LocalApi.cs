using System;
using System.Collections.Generic;
using TwoWaySync.Interfaces;
using TwoWaySync.Models.Internal;

namespace TwoWaySync.Services
{
    public class LocalApi : ILocalApi
    {
        public Company GetCompanyByName(string companyName) => new Company();
        public Company GetCompanyById(Guid companyId) => new Company();
        public Company CreateCompany(string companyName) => new Company();
        public List<Company> GetCompanies(DateTime changedFromStart, DateTime changedFromEnd) => new List<Company>();
        
        public Task GetTaskById(Guid taskId) =>  new Task();
        public Task CreateOrUpdateTask(Task task) => new Task();
        public List<Task> GetTasks(DateTime changedFromStart, DateTime changedFromEnd) => new List<Task>();
    }
}