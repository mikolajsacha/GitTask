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

        private bool _isProjectInitializerVisible;
        public bool IsProjectInitializerVisible
        {
            get { return _isProjectInitializerVisible; }
            private set
            {
                _isProjectInitializerVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isMergingToolVisible;
        public bool IsMergingToolVisible
        {
            get { return _isMergingToolVisible; }
            private set
            {
                _isMergingToolVisible = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel(IProjectPathsReadonlyService projectPathsService,
                             RegistryViewModel registryViewModel,
                             IMergingService mergingService)
        {
            _isTaskBoardVisible = false;
            _isMergingToolVisible = false;
            _isProjectInitializerVisible = true;

            _isTaskBoardVisible = projectPathsService.IsProjectPathChosen;
            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
            mergingService.MergingCompleted += MergingServiceOnMergingCompleted;
            registryViewModel.InitializeRegistry();
        }

        private void MergingServiceOnMergingCompleted()
        {
            IsProjectInitializerVisible = false;
            IsMergingToolVisible = false;
            IsTaskBoardVisible = true;
        }

        private void OnProjectPathChanged()
        {
            IsProjectInitializerVisible = false;
            IsTaskBoardVisible = false;
            IsMergingToolVisible = true;
        }
    }
}