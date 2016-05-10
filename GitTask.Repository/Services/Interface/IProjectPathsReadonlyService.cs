using System;

namespace GitTask.Repository.Services.Interface
{
    public interface IProjectPathsReadonlyService
    {
        event Action ProjectPathChanged;

        string BaseProjectPath { get; }
        string BaseStoragePath { get; }
        bool IsProjectPathChosen { get; }

        string GetPathForModel(Type modelType);
    }
}