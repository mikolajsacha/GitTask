using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.ViewModel.Main;

namespace GitTask.UI.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const int DefaultOpenedColumnsCount = 4;
        public ObservableCollection<TaskStateColumnViewModel> TaskStateColumns { get; }

        private int _openedColumnsCount;
        public int OpenedColumnsCount
        {
            get { return _openedColumnsCount; }
            private set
            {
                _openedColumnsCount = value;
                RaisePropertyChanged();
            }
        }

        private int _hiddenColumnsCount;
        public int HiddenColumnsCount
        {
            get { return _hiddenColumnsCount; }
            private set
            {
                _hiddenColumnsCount = value;
                RaisePropertyChanged();
            }
        }

        private readonly IQueryService<TaskState> _taskStateQueryService;
        private readonly IQueryService<Task> _taskQueryService;

        public MainViewModel(IQueryService<TaskState> taskStateQueryService,
                             IQueryService<Task> taskQueryService)
        {
            _taskStateQueryService = taskStateQueryService;
            _taskQueryService = taskQueryService;

            TaskStateColumns = new ObservableCollection<TaskStateColumnViewModel>();
        }

        public void InitializeTaskStateColumns()
        {

            TaskStateColumns.Clear();
            var counter = 0;
            foreach (var state in _taskStateQueryService.GetAll().OrderBy(x => x.Position))
            {
                var isOpened = counter < DefaultOpenedColumnsCount;
                var taskStateColumn = new TaskStateColumnViewModel(state, _taskQueryService, isOpened);
                taskStateColumn.PropertyChanged += TaskStateColumnOnPropertyChanged;
                TaskStateColumns.Add(taskStateColumn);
                counter++;
            }

            OpenedColumnsCount = Math.Min(counter, DefaultOpenedColumnsCount);
            HiddenColumnsCount = Math.Max(0, counter - OpenedColumnsCount);
        }

        private void TaskStateColumnOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "IsOpened") return;
            var openColumnsCount = 0;
            var hiddenColumnsCount = 0;

            foreach (var column in TaskStateColumns)
            {
                if (column.IsOpened)
                {
                    openColumnsCount++;
                }
                else
                {
                    hiddenColumnsCount++;
                }
            }
            OpenedColumnsCount = openColumnsCount;
            HiddenColumnsCount = hiddenColumnsCount;
        }
    }
}