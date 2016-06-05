using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;

namespace GitTask.UI.MVVM.Design
{
    public class DesignTaskDetailsViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.TaskDetails.TaskDetailsViewModel
    {
        public Task Task { get; }
        public bool IsVisible => true;
        public string CreationDate => "01/11/2016 10:23";
        public ObservableCollection<string> Comments { get; }
        public string AddedComment { get; set; }

        public ICommand ResolveHistoryCommand { get; }
        public ICommand AddCommentCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand RemoveCommentCommand { get; }

        public bool IsAddingCommentEnabled => true;
        public bool AnyComments => true;
        public bool IsFullContentVisible => true;
        public bool CommentsVisible => AnyComments && IsFullContentVisible;
        public bool IsHistoryBeingResolved => false;

        public DesignTaskDetailsViewModel(Task task)
        {
            Task = task;
            EditTaskCommand = new RelayCommand(() => { });
            AddCommentCommand = new RelayCommand(() => { });
            ResolveHistoryCommand = new RelayCommand(() => { });
            RemoveCommentCommand = new RelayCommand(() => { });
            Comments = new ObservableCollection<string>();
        }

        public DesignTaskDetailsViewModel()
        {
            EditTaskCommand = new RelayCommand(() => { });
            AddCommentCommand = new RelayCommand(() => { });
            ResolveHistoryCommand = new RelayCommand(() => { });
            RemoveCommentCommand = new RelayCommand(() => { });
            Comments = new ObservableCollection<string>();
            Task = new Task
            {
                AssignedMembers = new[] { new ProjectMember("Jan Kowalski", "jankowalski@o2.pl"), new ProjectMember("Maciej Łoś", "losmaciej@hotmail.com"), },
                Title = "Zrobić to i owo.",
                Content = "Trzeba koniecznie porobić to oraz owo!",
                Priority = TaskPriority.Blocker,
               State = "TO DO"
            };
        }
    }
}