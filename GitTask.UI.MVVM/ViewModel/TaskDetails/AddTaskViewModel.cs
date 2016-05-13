using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;

        public bool IsOkButtonEnabled => true;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        public AddTaskViewModel(IQueryService<Task> taskQueryService)
        {
            _taskQueryService = taskQueryService;
            _okCommand = new RelayCommand(OnOkClick);
        }

        private async void OnOkClick()
        {
            _taskQueryService.AddNew(new Task());
            await _taskQueryService.SaveChanges();
        }
    }
}