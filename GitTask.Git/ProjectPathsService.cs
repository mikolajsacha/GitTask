using System;
using GitTask.Repository.Services.Interface;

namespace GitTask.Git
{
    public class ProjectPathsService : IProjectPathsService
    {
        private string _baseProjectPath;
        public string BaseProjectPath
        {
            get { return _baseProjectPath; }
            set
            {
                _baseProjectPath = value;
                BaseStoragePath = _baseProjectPath.TrimEnd('\\', '/') + "\\gittask";
                IsProjectPathChosen = true;
                ProjectPathChanged?.Invoke();
            }
        }

        public string BaseStoragePath { get; private set; }
        public bool IsProjectPathChosen { get; private set; }

        public string GetPathForModel(Type modelType)
        {
            return BaseStoragePath + "\\" + modelType.Name;
        }

        public event Action ProjectPathChanged;

        public ProjectPathsService()
        {
            IsProjectPathChosen = false;
        }
    }
}