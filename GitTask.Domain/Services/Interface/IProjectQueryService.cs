using System;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Services.Interface
{
    public interface IProjectQueryService
    {
        event Action<ProjectMember> UserAdded;
        event Action<string> ProjectTitleChanged;
        event Action<Project> ProjectChanged;

        Project Project { get; }

        void AddUser(ProjectMember user);
        void SetTitle(string title);
        Task SaveChanges();
    }
}
