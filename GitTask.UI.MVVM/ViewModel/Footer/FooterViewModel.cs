using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        public ObservableCollection<ProjectMember> ProjectMembers { get; }

        public string ProjectName { get; }
        public ProjectMember CurrentUser { get; }

        public FooterViewModel(IQueryService<Project> projectQueryService, IQueryService<ProjectMember> projectMemberQueryService)
        {
            ProjectName = projectQueryService.GetAll().First().Title;
            CurrentUser = projectMemberQueryService.GetAll().First();
            ProjectMembers = new ObservableCollection<ProjectMember>(projectMemberQueryService.GetAll());
        }
    }
}