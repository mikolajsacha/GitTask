using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GitTask.Domain.Enum;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;
using GitTask.Storage.Interface;

namespace GitTask.UI.MVVM.Design
{
    public static class DesignStorageServices
    {
        public static IStorageService<Project> DesignProjectService { get; }
        public static IStorageService<ProjectMember> DesignProjectMemberService { get; }
        public static IStorageService<TaskState> DesignTaskStateService { get; }
        public static IStorageService<Task> DesignTaskService { get; }
        public static IStorageService<Comment> DesignCommentService { get; }

        static DesignStorageServices()
        {
            DesignProjectService = new DesignStorageService<Project>(new List<Project>
            {
                new Project() {Title = "Projekt 1", DateCreated = DateTime.Now.AddYears(-1)}
            });
            var testImage = new BitmapImage(new Uri(@"C:\Users\232000\Documents\Visual Studio 2015\Projects\GitTask\GitTask.UI.MVVM\Design\cat.jpg"));
            DesignProjectMemberService = new DesignStorageService<ProjectMember>(new List<ProjectMember>
            {
                new ProjectMember() { Name = "Mikołaj Sacha", Color = Brushes.DarkGreen, Avatar = testImage},
                new ProjectMember() { Name = "Jan Kowalski", Color = Brushes.LimeGreen, Avatar = testImage},
                new ProjectMember() { Name = "Marcin Nowak", Color = Brushes.DarkGray, Avatar = testImage},
                new ProjectMember() { Name = "Aleksandra Zachwiej", Color = Brushes.CornflowerBlue, Avatar = testImage}
            });
            DesignTaskStateService = new DesignStorageService<TaskState>(new List<TaskState>
            {
                new TaskState() { Name = "TO DO", Color = Brushes.Yellow, Position = 0},
                new TaskState() { Name = "IN PROGRESS", Color = Brushes.Red, Position  = 1},
                new TaskState() { Name = "TO BE TESTED", Color = Brushes.Blue, Position  = 2},
                new TaskState() { Name = "FIXED", Color = Brushes.Green, Position  = 3},
                new TaskState() { Name = "CLOSED", Color = Brushes.Gray, Position  = 4}
            });
            DesignTaskService = new DesignStorageService<Task>(new List<Task>
            {
                new Task() { Id = 1, Title = "Zrob kanapki", DateCreated = DateTime.Now.AddDays(-100), Priority = TaskPriority.Major, Content = "Prosze Cie, zrobze kanapeczki",
                    AssignedMembers = new List<string> {"Aleksandra Zachwiej"}, AuthorName = "Mikołaj Sacha", CommentsIds = new List<int> {1}, State = "TO DO"},
                new Task() { Id = 2, Title = "Dokonczyc aplikacje", DateCreated = DateTime.Now.AddDays(-100), Priority = TaskPriority.Critical, Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    AssignedMembers = new List<string> {"Mikołaj Sacha", "Jan Kowalski"}, AuthorName = "Marcin Nowak", CommentsIds = new List<int>(), State = "IN PROGRESS"},
                new Task() { Id = 3, Title = "Przetestuj jakies cos", DateCreated = DateTime.Now.AddDays(-100), Priority = TaskPriority.Minor, Content = "Testy jednostkowe bla bla bla.",
                    AssignedMembers = new List<string> (), AuthorName = "Jan Kowalski", CommentsIds = new List<int>(), State = "FIXED"}
            });
            DesignCommentService = new DesignStorageService<Comment>(new List<Comment>
            {
                new Comment() { Id = 1, AuthorName = "Jan Kowalski", DateCreated = DateTime.Now.AddDays(-1).AddHours(-2), Content = "Podobno lubi z salami..." }
            });
        }
    }
}