namespace GitTask.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
           /* Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var storageService = new StorageService<Task>(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var queryService = new QueryService<Task>(storageService);

            var member1 = new ProjectMember
            {
                Name = "Mikołaj Sacha"
            };

            var member2 = new ProjectMember
            {
                Name = "Ola Zachwiej"
            };

            var task1 = new Task
            {
                Id = 0,
                Assigned = new[] { member1 },
                Author = member1,
                Comments = new Comment[0],
                Content = "Zadanie bardzo ważne: Zażółć gęślą jaźń",
                DateCreated = DateTime.Now,
                Priority = TaskPriority.Blocker,
                State = new TaskState { Name = "Do zrobienia", Color = new ArgbColor(Color.Azure), Position = 0 },
                Title = "Zadanie 1"
            };

            storageService.Save(task1);
            var task1FromStorage = storageService.GetByKey(task1.Id);
            Console.WriteLine(task1FromStorage.Result.Content);*/
        }
    }
}
