using System.Collections.Generic;

namespace GitTask.Repository.Services.Interface
{
    public interface IRepositoryService
    {
        IEnumerable<string> GetAllCommitersNames();
        bool RepositoryExists(string projectPath);
    }
}
