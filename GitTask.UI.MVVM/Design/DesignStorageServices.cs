using System;
using System.Collections.Generic;
using System.Windows.Media;
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
            DesignProjectMemberService = new DesignStorageService<ProjectMember>(new List<ProjectMember>
            {
                new ProjectMember() { Name = "Mikołaj Sacha" },
                new ProjectMember() { Name = "Jan Kowalski" },
                new ProjectMember() { Name = "Marcin Nowak" },
                new ProjectMember() { Name = "Aleksandra Zachwiej" }
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
                new Task() { Id = 2, Title = "Dokonczyc aplikacje", DateCreated = DateTime.Now.AddDays(-100), Priority = TaskPriority.Critical, Content = "Najwyzszy czas to skonczyc!",
                    AssignedMembers = new List<string> {"Mikołaj Sacha", "Jan Kowalski"}, AuthorName = "Marcin Nowak", CommentsIds = new List<int>(), State = "IN PROFRESS"},
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