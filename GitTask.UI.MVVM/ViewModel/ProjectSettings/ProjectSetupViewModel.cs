using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.ProjectSettings
{
    public class ProjectSetupViewModel : ViewModelBase
    {
        private readonly IProjectQueryService _projectQueryService;

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                RaisePropertyChanged();
            }
        }

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public ProjectSetupViewModel(IProjectQueryService projectQueryService)
        {
            _projectQueryService = projectQueryService;
            _okCommand = new RelayCommand(OnOkClick);
        }

        private async void OnOkClick()
        {
            _projectQueryService.SetTitle(_projectName);
            await _projectQueryService.SaveChanges();
        }
    }
}
