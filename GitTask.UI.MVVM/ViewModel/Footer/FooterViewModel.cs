using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        public ObservableCollection<string> ProjectMembers { get; }

        public string ProjectName { get; }
        public string CurrentUserName { get; }

        public FooterViewModel(IQueryService<Project> projectQueryService, IQueryService<ProjectMember> projectMemberQueryService)
        {
            ProjectName = projectQueryService.GetAll().First().Title;
            CurrentUserName = "Current User";
            ProjectMembers = new ObservableCollection<string>(projectMemberQueryService.GetAll().Select(x => x.Name));
        }
    }
}