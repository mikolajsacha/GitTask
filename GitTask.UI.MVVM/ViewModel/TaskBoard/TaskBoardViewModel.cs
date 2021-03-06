﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.ActionBar;
using GitTask.UI.MVVM.ViewModel.TaskDetails;

namespace GitTask.UI.MVVM.ViewModel.TaskBoard
{
    public class TaskBoardViewModel : ViewModelBase
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;
        private readonly IQueryService<Task> _taskQueryService;
        private readonly IRepositoryService _repositoryService;
        private readonly FiltersViewModel _filtersViewModel;

        private const int DefaultOpenedColumnsCount = 4;
        public ObservableCollection<TaskStateColumnViewModel> TaskStateColumns { get; }
        public double CurrentOpenedTaskStateColumnWidth { get; set; }

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

        public TaskBoardViewModel(IQueryService<TaskState> taskStateQueryService,
                                  IQueryService<Task> taskQueryService,
                                  IRepositoryService repositoryService,
                                  FiltersViewModel filtersViewModel)
        {
            _taskStateQueryService = taskStateQueryService;
            _taskQueryService = taskQueryService;
            _repositoryService = repositoryService;
            _filtersViewModel = filtersViewModel;

            _taskQueryService.ElementAdded += TaskQueryServiceOnElementAdded;
            _taskQueryService.ElementUpdated += TaskQueryServiceOnElementUpdated;
            _taskQueryService.ElementDeleted += TaskQueryServiceOnElementDeleted;

            _taskStateQueryService.ElementAdded += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementUpdated += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementDeleted += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementsReloaded += OnElementsReloaded;
            _taskQueryService.ElementsReloaded += OnElementsReloaded;

            TaskStateColumns = new ObservableCollection<TaskStateColumnViewModel>();
            Messenger.Default.Register<MoveTaskToTaskStateMessage>(this, OnMoveTaskToTaskStateMessage);
            Messenger.Default.Register<DeleteTaskStateMessage>(this, OnDeleteTaskStateMessage);
        }

        private async void OnMoveTaskToTaskStateMessage(MoveTaskToTaskStateMessage message)
        {
            var task = _taskQueryService.GetByKey(message.TaskName);
            TaskQueryServiceOnElementDeleted(task);

            task.State = message.NewTaskStateName;
            _taskQueryService.Update(task);
            await _taskQueryService.SaveChanges();
        }

        private async void OnDeleteTaskStateMessage(DeleteTaskStateMessage message)
        {
            _taskStateQueryService.Delete(message.TaskState.Name);
            await _taskStateQueryService.SaveChanges();
        }

        private void TaskQueryServiceOnElementAdded(Task task)
        {
            try
            {
                var taskColumn = TaskStateColumns.First(stateColumn => stateColumn.TaskState.Name == task.State);
                taskColumn.Tasks.Add(new TaskDetailsViewModel(task, _taskQueryService, _taskStateQueryService, _repositoryService));
                var tasks = taskColumn.Tasks.ToList();
                taskColumn.Tasks.Clear();
                foreach (var taskVm in tasks.OrderByDescending(x=> x.Task.Priority).ThenBy(x => x.Task.Title))
                {
                    taskColumn.Tasks.Add(taskVm);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void TaskQueryServiceOnElementDeleted(Task task)
        {
            try
            {
                var taskColumn = TaskStateColumns.First(stateColumn => stateColumn.TaskState.Name == task.State);
                var taskViewModel = taskColumn.Tasks.First(taskVm => taskVm.Task.Title == task.Title);
                taskColumn.Tasks.Remove(taskViewModel);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void TaskQueryServiceOnElementUpdated(Task task)
        {
            TaskQueryServiceOnElementDeleted(task);
            TaskQueryServiceOnElementAdded(task);
        }

        private void OnElementsReloaded()
        {
            Reload();
        }

        private void TaskStateQueryServiceOnElementChanged(object taskState)
        {
            Reload();
        }

        private void Reload()
        {
            InitializeTaskStateColumns();
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (TaskStateColumns == null || !TaskStateColumns.Any()) return;
            foreach (var taskStateColumn in TaskStateColumns)
            {
                taskStateColumn.Tasks.Clear();
            }

            foreach (var task in _taskQueryService.GetAll().OrderByDescending(x => x.Priority).ThenBy(x => x.Title))
            {
                try
                {

                    var taskColumn = TaskStateColumns.First(taskStateColumn => taskStateColumn.TaskState.Name == task.State);
                    taskColumn.Tasks.Add(new TaskDetailsViewModel(task, _taskQueryService,
                        _taskStateQueryService, _repositoryService));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public void InitializeTaskStateColumns()
        {
            TaskStateColumns.Clear();
            var orderedTaskStates = _taskStateQueryService.GetAll().OrderBy(x => x.Position);
            var count = orderedTaskStates.Count();
            var counter = 0;

            foreach (var state in orderedTaskStates)
            {
                var isOpened = counter < DefaultOpenedColumnsCount;
                var taskStateColumn = new TaskStateColumnViewModel(state, _taskStateQueryService, _filtersViewModel, counter,
                                                                   isOpened, counter > 0, counter < count - 1);
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