using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;
using GitTask.Repository.Model;
using GitTask.UI.MVVM.Messages;

namespace GitTask.UI.MVVM.ViewModel.Footer
{
    public class FooterViewModel : ViewModelBase
    {
        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            private set
            {
                _projectName = value;
                RaisePropertyChanged();
            }
        }

        private ProjectMember _currentUser;
        public ProjectMember CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                _currentUser = value;
                RaisePropertyChanged();
            }
        }

        public FooterViewModel(IQueryService<Project> projectQueryService)
        {
            var projects = projectQueryService.GetAll().ToList();
            if (projects.Any())
            {
                _projectName = projects.First().Title;
            }

            projectQueryService.ElementAdded += ProjectQueryServiceOnElementAdded;

            Messenger.Default.Register<SetCurrentUserMessage>(this, OnSetCurrentUserMessage);
        }

        private void OnSetCurrentUserMessage(SetCurrentUserMessage currentUserMessage)
        {
            CurrentUser = currentUserMessage.CurrentUser;
        }

        private void ProjectQueryServiceOnElementAdded(Project project)
        {
            ProjectName = project.Title;
        }
    }
}