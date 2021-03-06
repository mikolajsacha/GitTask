﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;

namespace GitTask.Repository.Services.Interface
{
    public interface IRepositoryService
    {
        event Action RepositoryInitalized;

        Task<IEnumerable<ProjectMember>> GetAllCommiters();
        bool RepositoryExists(string projectPath);
    }
}
