﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.TaskBoard
{
    public class TaskBoardViewModel : ViewModelBase
    {
        private readonly IQueryService<TaskState> _taskStateQueryService;
        private readonly IQueryService<Task> _taskQueryService;

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
                                  IQueryService<Task> taskQueryService)
        {
            _taskStateQueryService = taskStateQueryService;
            _taskQueryService = taskQueryService;

            _taskStateQueryService.ElementAdded += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementUpdated += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementDeleted += TaskStateQueryServiceOnElementChanged;
            _taskStateQueryService.ElementsReloaded += TaskStateQueryServiceOnElementsReloaded;

            TaskStateColumns = new ObservableCollection<TaskStateColumnViewModel>();
        }

        private void TaskStateQueryServiceOnElementsReloaded()
        {
            InitializeTaskStateColumns();
        }

        private void TaskStateQueryServiceOnElementChanged(object taskState)
        {
            InitializeTaskStateColumns();
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