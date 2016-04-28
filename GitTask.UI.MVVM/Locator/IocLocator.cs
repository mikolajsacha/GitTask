using GalaSoft.MvvmLight.Ioc;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Model.Task;
using GitTask.Domain.Services.Interface;
using GitTask.Storage;
using GitTask.UI.MVVM.Design;
using GitTask.UI.MVVM.ViewModel;
using GitTask.UI.MVVM.ViewModel.Footer;
using GitTask.UI.MVVM.ViewModel.TaskDetails;
using Microsoft.Practices.ServiceLocation;

namespace GitTask.UI.MVVM.Locator
{
    public class IocLocator
    {
        static IocLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            SimpleIoc.Default.Register(() => DesignStorageServices.DesignProjectService);
            SimpleIoc.Default.Register(() => DesignStorageServices.DesignProjectMemberService);
            SimpleIoc.Default.Register(() => DesignStorageServices.DesignTaskStateService);
            SimpleIoc.Default.Register(() => DesignStorageServices.DesignTaskService);
            SimpleIoc.Default.Register(() => DesignStorageServices.DesignCommentService);
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IFileService, JsonFileService>();
            //
            //    var baseStoragePath = System.Reflection.Assembly.GetExecutingAssembly().Location; //TODO: ma byc w .git/git_task czy jakos tak
            //    SimpleIoc.Default.Register(() => new StoragePath(baseStoragePath));
            //}

            SimpleIoc.Default.Register<IQueryService<Project>, QueryService<Project>>();
            SimpleIoc.Default.Register<IQueryService<ProjectMember>, QueryService<ProjectMember>>();
            SimpleIoc.Default.Register<IQueryService<TaskState>, QueryService<TaskState>>();
            SimpleIoc.Default.Register<IQueryService<Task>, QueryService<Task>>();
            SimpleIoc.Default.Register<IQueryService<Comment>, QueryService<Comment>>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FooterViewModel>();
        }

        public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public static FooterViewModel FooterViewModel => ServiceLocator.Current.GetInstance<FooterViewModel>();

        public static void Cleanup()
        {
        }
    }
}