using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class SelectTaskStateViewModel : ViewModelBase
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;

        private TaskState _selectedTaskState;
        public TaskState SelectedTaskState
        {
            get { return _selectedTaskState; }
            set
            {
                _selectedTaskState = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TaskStateChosen");
            }
        }

        public ObservableCollection<TaskState> AllTaskStates { get; }
        public bool TaskStateChosen => _selectedTaskState != null;

        public SelectTaskStateViewModel(IQueryService<TaskState> taskStateQueryService)
        {
            if (taskStateQueryService != null)
            {
                _taskStateQueryService = taskStateQueryService;
                AllTaskStates =
                    new ObservableCollection<TaskState>(_taskStateQueryService.GetAll().OrderBy(taskState => taskState.Position));

                _taskStateQueryService.ElementDeleted += TaskStateQueryServiceOnElementDeleted;
                _taskStateQueryService.ElementAdded += TaskStateQueryServiceOnElementAdded;
                _taskStateQueryService.ElementUpdated += TaskStateQueryServiceOnElementUpdated;
                _taskStateQueryService.ElementsReloaded += TaskStateQueryServiceOnElementsReloaded;
            }
            else
            {
                AllTaskStates = new ObservableCollection<TaskState>();
            }

            AllTaskStates.CollectionChanged += AllTaskStatesOnCollectionChanged;
        }

        private void AllTaskStatesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (SelectedTaskState != null && !AllTaskStates.Contains(SelectedTaskState))
            {
                SelectedTaskState = null;
            }
        }

        private void TaskStateQueryServiceOnElementsReloaded()
        {
            AllTaskStates.Clear();
            foreach (var state in _taskStateQueryService.GetAll())
            {
                AllTaskStates.Add(state);
            }
        }

        private void TaskStateQueryServiceOnElementUpdated(TaskState updatedTaskState)
        {
            var elementsToBeUpdated = AllTaskStates.Where(existingTaskState => existingTaskState.Name == updatedTaskState.Name).ToList();
            foreach (var element in elementsToBeUpdated)
            {
                AllTaskStates.Remove(element);
            }
            AllTaskStates.Add(updatedTaskState);
        }

        private void TaskStateQueryServiceOnElementAdded(TaskState taskState)
        {
            AllTaskStates.Add(taskState);
        }

        private void TaskStateQueryServiceOnElementDeleted(TaskState deletedTaskState)
        {
            var elementsToBeDeleted = AllTaskStates.Where(taskState => taskState.Name == deletedTaskState.Name);
            foreach (var element in elementsToBeDeleted)
            {
                AllTaskStates.Remove(element);
            }
        }
    }
}