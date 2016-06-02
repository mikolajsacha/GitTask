using System;

namespace GitTask.Domain.Services.Interface
{
    public interface IProjectPathsReadonlyService
    {
        event Action ProjectPathChanged;

        string BaseProjectPath { get; }
        string BaseStoragePath { get; }
        string RelativeStoragePath { get; }
        bool IsProjectPathChosen { get; }

        string GetPathForModel(Type modelType);
    }
}