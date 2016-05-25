using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.Git;
using GitTask.Json;
using GitTask.Storage;
using GitTask.Storage.Interface;
using GitTask.UI.MVVM.ViewModel.ButtonsBar;
using GitTask.UI.MVVM.ViewModel.Common;
using GitTask.UI.MVVM.ViewModel.Footer;
using GitTask.UI.MVVM.ViewModel.Main;
using GitTask.UI.MVVM.ViewModel.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.TaskBoard;
using GitTask.UI.MVVM.ViewModel.TaskDetails;
using Ninject;

namespace GitTask.UI.MVVM.Locator
{
    public class IocModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IProjectPathsReadonlyService, IProjectPathsService>().To<ProjectPathsService>().InSingletonScope();
            Bind<IFileService>().To<JsonFileService>().InSingletonScope();

            Bind<IStorageService<Project>>().To<StorageService<Project>>().InSingletonScope();
            Bind<IStorageService<TaskState>>().To<StorageService<TaskState>>().InSingletonScope();
            Bind<IStorageService<Task>>().To<StorageService<Task>>().InSingletonScope();

            Bind<IRepositoryService>().To<RepositoryService>().InSingletonScope();

            Bind<IQueryService<Project>>().To<QueryService<Project>>().InSingletonScope();
            Bind<IQueryService<TaskState>>().To<QueryService<TaskState>>().InSingletonScope();
            Bind<IQueryService<Task>>().To<QueryService<Task>>().InSingletonScope();

            Bind<PendingStorageOperationsViewModel>().ToSelf().InSingletonScope().WithConstructorArgument("storageServices",
                new IStorageService[]
                {
                   Kernel.Get<IStorageService<Project>>(),
                   Kernel.Get<IStorageService<TaskState>>(),
                   Kernel.Get<IStorageService<Task>>(),
                });

            Bind<RegistryViewModel>().ToSelf().InSingletonScope();
            Bind<MainViewModel>().ToSelf().InSingletonScope();
            Bind<ProjectOpeningViewModel>().ToSelf().InSingletonScope();
            Bind<TaskBoardViewModel>().ToSelf().InSingletonScope();
            Bind<FooterViewModel>().ToSelf().InSingletonScope();
            Bind<ProjectSetupViewModel>().ToSelf().InSingletonScope();
            Bind<ButtonsBarViewModel>().ToSelf().InSingletonScope();
            Bind<ProjectMembersViewModel>().ToSelf().InSingletonScope();
            Bind<ProjectMembersSetsViewModel>().ToSelf().InSingletonScope();
            Bind<SetCurrentUserViewModel>().ToSelf().InSingletonScope();
            Bind<AddTaskViewModel>().ToSelf().InSingletonScope();
            Bind<AddTaskStateViewModel>().ToSelf().InSingletonScope();
        }
    }
}