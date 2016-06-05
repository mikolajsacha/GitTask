using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Repository.EntityHistory;
using GitTask.Domain.Model.Repository.ProjectHistory;

namespace GitTask.Domain.Services.Interface
{
    public interface IRepositoryService
    {
        event Action RepositoryInitalized;

        Task<IEnumerable<ProjectMember>> GetAllUniqueCommiters();
        Task<EntityHistory> GetEntityHistory<TModel>(TModel modelObject);
        Task<ProjectHistory> GetProjectHistory();
        bool RepositoryExists(string projectPath);
    }
}
