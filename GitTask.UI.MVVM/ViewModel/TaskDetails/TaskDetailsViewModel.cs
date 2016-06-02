using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.UI.MVVM.View.TaskDetails;
using GitTask.UI.MVVM.ViewModel.Common;

namespace GitTask.UI.MVVM.ViewModel.TaskDetails
{
    public class TaskDetailsViewModel : ViewModelBase
    {
        private readonly IQueryService<Task> _taskQueryService;
        private readonly IQueryService<TaskState> _taskStateQueryService;
        private readonly IRepositoryService _repositoryService;
        private readonly CurrentUserViewModel _currentUserViewModel;
        public Task Task { get; }
        public ObservableCollection<string> Comments { get; }

        private string _addedComment;
        public string AddedComment
        {
            get { return _addedComment; }
            set
            {
                _addedComment = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsAddingCommentEnabled");
            }
        }

        private readonly RelayCommand _addCommentCommand;
        public ICommand AddCommentCommand => _addCommentCommand;

        private readonly RelayCommand _editTaskCommand;
        public ICommand EditTaskCommand => _editTaskCommand;

        private readonly RelayCommand<int> _removeCommentCommand;
        public ICommand RemoveCommentCommand => _removeCommentCommand;

        public bool IsAddingCommentEnabled => !string.IsNullOrWhiteSpace(AddedComment);
        public bool AnyComments => Comments != null && Comments.Any();

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }

        private string _creationDate;
        public string CreationDate
        {
            get { return _creationDate; }
            set
            {
                _creationDate = value;
                RaisePropertyChanged();
            }
        }

        public TaskDetailsViewModel(Task task,
            IQueryService<Task> taskQueryService,
            IQueryService<TaskState> taskStateQueryService,
            IRepositoryService repositoryService,
            CurrentUserViewModel currentUserViewModel)
        {
            Task = task;

            Comments = new ObservableCollection<string>(Task.Comments);
            Comments.CollectionChanged += (sender, args) => RaisePropertyChanged("AnyComments");

            _isVisible = true;
            _taskQueryService = taskQueryService;
            _taskStateQueryService = taskStateQueryService;
            _repositoryService = repositoryService;
            _currentUserViewModel = currentUserViewModel;
            _editTaskCommand = new RelayCommand(OnEditTaskCommand);
            _addCommentCommand = new RelayCommand(OnAddCommentCommand);
            _removeCommentCommand = new RelayCommand<int>(OnRemoveCommentCommand);

            //TODO: dodać przycisk do znajdowania historii (a nie robic to dla kazdego taska)
            System.Threading.Tasks.Task.Run(() => ResolveHistory());
        }

        private async void ResolveHistory()
        {
            var creationDate = await _repositoryService.GetCreationDate(Task);
            Application.Current.Dispatcher.Invoke(() =>
            {
                CreationDate = creationDate.ToString("g");
            }, DispatcherPriority.Normal);
        }

        private async void OnAddCommentCommand()
        {
            if (Task.Comments == null)
            {
                Task.Comments = new List<string>();
            }

            var signedComment = $"{AddedComment} ({_currentUserViewModel.CurrentUser.Name}, {DateTime.Now})";

            Task.Comments.Add(signedComment);
            Comments.Add(signedComment);
            AddedComment = "";
            _taskQueryService.Update(Task);
            await _taskQueryService.SaveChanges();
        }

        private async void OnRemoveCommentCommand(int index)
        {
            if (Task.Comments == null || Task.Comments.Count <= index) return;
            Task.Comments.RemoveAt(index);

            _taskQueryService.Update(Task);
            await _taskQueryService.SaveChanges();

            Comments.Clear();
            foreach (var comment in Task.Comments)
            {
                Comments.Add(comment);
            }
        }

        private void OnEditTaskCommand()
        {
            var editTaskWindow =
                new EditTaskWindow(new EditTaskViewModel(Task, _taskQueryService, _taskStateQueryService))
                {
                    Owner = Application.Current.MainWindow
                };
            editTaskWindow.Show();
        }
    }
}