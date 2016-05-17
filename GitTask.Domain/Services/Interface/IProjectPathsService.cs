namespace GitTask.Domain.Services.Interface
{
    public interface IProjectPathsService : IProjectPathsReadonlyService
    {
        new string BaseProjectPath { get; set; }
    }
}