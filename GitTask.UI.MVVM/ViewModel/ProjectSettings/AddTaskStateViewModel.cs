using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Exception;

namespace GitTask.UI.MVVM.ViewModel.ProjectSettings
{
    public class AddTaskStateViewModel : ViewModelBase
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;

        private readonly RelayCommand _okCommand;
        public ICommand OkCommand => _okCommand;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        private Brush _brush;
        public Brush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsOkButtonEnabled");
            }
        }

        public bool IsOkButtonEnabled =>
            !string.IsNullOrWhiteSpace(_name) &&
            _brush != null;

        public AddTaskStateViewModel(IQueryService<TaskState> taskStateQueryService)
        {
            _taskStateQueryService = taskStateQueryService;
            _okCommand = new RelayCommand(OnOkClick);
        }

        private async void OnOkClick()
        {
            try
            {
                _taskStateQueryService.AddNew(new TaskState
                {
                    Name = _name,
                    Color = _brush,
                    Position = GetLastTaskStatePosition() + 1,
                });
            }
            catch (KeyAlreadyExistsException)
            {
                _taskStateQueryService.Update(new TaskState
                {
                    Name = _name,
                    Color = _brush,
                    Position = GetLastTaskStatePosition() + 1,
                });
            }
            await _taskStateQueryService.SaveChanges();
        }

        private double GetLastTaskStatePosition()
        {
            return _taskStateQueryService.GetAll().Any() ? _taskStateQueryService.GetAll().Select(taskState => taskState.Position).Max() : 0;
        }
    }
}