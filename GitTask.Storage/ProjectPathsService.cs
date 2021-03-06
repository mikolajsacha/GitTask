﻿using System;
using GitTask.Domain.Services.Interface;

namespace GitTask.Storage
{
    public class ProjectPathsService : IProjectPathsService
    {
        public string RelativeStoragePath => ".gittask";

        private string _baseProjectPath;
        public string BaseProjectPath
        {
            get { return _baseProjectPath; }
            set
            {
                _baseProjectPath = value;
                BaseStoragePath = _baseProjectPath.TrimEnd('\\', '/') + "\\" + RelativeStoragePath;
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