using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class ProjectSetupViewModel : ViewModelBase
    {
        private readonly IQueryService<Project> _projectQueryService;

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

        public ProjectSetupViewModel(IQueryService<Project> projectQueryService)
        {
            _projectQueryService = projectQueryService;
            _okCommand = new RelayCommand(OnOkClick);
        }

        private async void OnOkClick()
        {
            _projectQueryService.AddNew(new Project
            {
                Title = _projectName,
                IsInitialized = true // TODO: kiedy koniec inicjalizacji?
            });
            await _projectQueryService.SaveChanges();
        }
    }
}
