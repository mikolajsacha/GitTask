using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Enum;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class SelectTaskPriorityViewModel : ViewModelBase
    {
        private static readonly IEnumerable<TaskPriority> AllEnumValues = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>();
        public IEnumerable<TaskPriority> AllTaskPriorities => AllEnumValues;

        private TaskPriority? _selectedTaskPriority;
        public TaskPriority? SelectedTaskPriority
        {
            get { return _selectedTaskPriority; }
            set
            {
                _selectedTaskPriority = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TaskPriorityChosen");
            }
        }

        public bool TaskPriorityChosen => _selectedTaskPriority != null;
    }
}