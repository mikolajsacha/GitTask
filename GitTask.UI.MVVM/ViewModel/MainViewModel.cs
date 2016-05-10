using GalaSoft.MvvmLight;
using GitTask.Repository.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel
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

        public MainViewModel(IProjectPathsReadonlyService projectPathsService)
        {
            _isTaskBoardVisible = projectPathsService.IsProjectPathChosen;
            projectPathsService.ProjectPathChanged += OnProjectPathChanged;
        }

        private void OnProjectPathChanged()
        {
            IsTaskBoardVisible = true; //TODO: sprawdzic, dlaczego to nie pokazuje taskboarda
        }
    }
}