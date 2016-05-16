using System;
using System.Collections.Generic;
using GitTask.Domain.Model.Project;

namespace GitTask.Repository.Services.Interface
{
    public interface IRepositoryService
    {
        event Action RepositoryInitalized;

        IEnumerable<ProjectMember> GetAllCommiters();
        bool RepositoryExists(string projectPath);
    }
}
