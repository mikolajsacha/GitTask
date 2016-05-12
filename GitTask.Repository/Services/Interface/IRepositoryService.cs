using System;
using System.Collections.Generic;

namespace GitTask.Repository.Services.Interface
{
    public interface IRepositoryService
    {
        event Action RepositoryInitalized;

        IEnumerable<string> GetAllCommitersNames();
        bool RepositoryExists(string projectPath);
    }
}
