using GalaSoft.MvvmLight;
using GitTask.Domain.Services.Interface;

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

        public FooterViewModel(IProjectQueryService projectQueryService)
        {
            if (projectQueryService.Project != null)
            {
                _projectName = projectQueryService.Project.Title;
            }

            projectQueryService.ProjectTitleChanged += ProjectQueryServiceOnTitleChanged;
        }

        private void ProjectQueryServiceOnTitleChanged(string newTitle)
        {
            if (newTitle != null)
            {
                ProjectName = newTitle;
            }
        }
    }
}