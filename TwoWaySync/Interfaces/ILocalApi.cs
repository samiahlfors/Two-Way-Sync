using System;
using System.Collections.Generic;
using TwoWaySync.Models.Internal;

namespace TwoWaySync.Interfaces
{
    public interface ILocalApi
    {
        Company GetCompanyByName(string companyName);
        Company GetCompanyById(Guid companyId);
        Company CreateCompany(string companyName);
        List<Company> GetCompanies(DateTime changedFromStart, DateTime changedFromEnd);

        Task GetTaskById(Guid taskId);
        Task CreateOrUpdateTask(Task task);
        List<Task> GetTasks(DateTime changedFromStart, DateTime changedFromEnd);
    }
}



/*
Companies.GetCompanyByName(string) returns Company
Companies.GetCompanyById(guid) returns Company
Companies.CreateCompany(string) returns Company
Companies.GetCompanies(DateTime changedFromStart, DateTime changedFromEnd) returns List<Company>

Tasks.GetTaskById(guid) returns Task
Tasks.CreateOrUpdateTask(Task) returns Task
Tasks.GetTasks(DateTime changedFromStart, DateTime changedFromEnd) returns List<Task>
*/