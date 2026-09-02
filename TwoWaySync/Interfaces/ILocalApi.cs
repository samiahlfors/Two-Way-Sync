using System;
using System.Collections.Generic;
using TwoWaySync.Models.Internal;

namespace TwoWaySync.Interfaces
{
    public interface ILocalApi
    {
        LocalCompany GetCompanyByName(string companyName);
        LocalCompany GetCompanyById(Guid companyId);
        LocalCompany CreateCompany(string companyName);
        List<LocalCompany> GetCompanies(DateTime changedFromStart, DateTime changedFromEnd);

        LocalTask GetTaskById(Guid taskId);
        LocalTask CreateOrUpdateTask(LocalTask task);
        List<LocalTask> GetTasks(DateTime changedFromStart, DateTime changedFromEnd);
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