using GalaSoft.MvvmLight;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.ViewModel.Main
{
    public class MainViewModel : ViewModelBase
    {

        private bool _isTaskBoardVisible;
        public bool IsTaskBoardVisible
        {
            get { return _isTaskBoardVisible; }
            private set
            {
                _isTaskBoardVisible = value; 
                RaisePropertyChanged();
                RaisePropertyChanged("IsProjectInitializerVisible");
            }
        }

        public bool IsProjectInitializerVisible => !_isTaskBoardVisible;

        public MainViewModel(IProjectPathsReadonlyService projectPathsService, RegistryViewModel registryViewModel)
        {
            _isTaskBoardVisible = projectPathsService.IsProjectPathChosen;
            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            registryViewModel.InitializeRegistry();
        }

        private void OnProjectPathChanged()
        {
            IsTaskBoardVisible = true;
        }
    }
}