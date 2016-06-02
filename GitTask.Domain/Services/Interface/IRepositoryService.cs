﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Services.Interface
{
    public interface IRepositoryService
    {
        event Action RepositoryInitalized;

        Task<IEnumerable<ProjectMember>> GetAllUniqueCommiters();
        Task<DateTime> GetCreationDate<TModel>(TModel modelObject);
        bool RepositoryExists(string projectPath);
    }
}
