using System.Resources;
using GalaSoft.MvvmLight;
using GitTask.UI.MVVM.ViewModel.ActionBar;
using GitTask.UI.MVVM.ViewModel.Common;
using GitTask.UI.MVVM.ViewModel.Footer;
using GitTask.UI.MVVM.ViewModel.Main;
using GitTask.UI.MVVM.ViewModel.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.TaskBoard;
using GitTask.UI.MVVM.ViewModel.TaskDetails;
using Ninject;

namespace GitTask.UI.MVVM.Locator
{
    public class IocLocator
    {
        private static readonly IKernel Kernel;

        static IocLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic) return;

            Kernel = new StandardKernel(new IocModule());
        }

        public static ResourceManager ResourceManager => Kernel.Get<ResourceManager>();
        public static MainViewModel MainViewModel => Kernel.Get<MainViewModel>();
        public static ProjectOpeningViewModel ProjectOpeningViewModel => Kernel.Get<ProjectOpeningViewModel>();
        public static TaskBoardViewModel TaskBoardViewModel => Kernel.Get<TaskBoardViewModel>();
        public static FooterViewModel FooterViewModel => Kernel.Get<FooterViewModel>();
        public static ProjectSetupViewModel ProjectSetupViewModel => Kernel.Get<ProjectSetupViewModel>();
        public static PendingStorageOperationsViewModel PendingStorageOperationsViewModel => Kernel.Get<PendingStorageOperationsViewModel>();
        public static ButtonsViewModel ButtonsViewModel => Kernel.Get<ButtonsViewModel>();
        public static ProjectMembersViewModel ProjectMembersViewModel => Kernel.Get<ProjectMembersViewModel>();
        public static ProjectMembersSetsViewModel ProjectMembersSetsViewModel => Kernel.Get<ProjectMembersSetsViewModel>();
        public static SetCurrentUserViewModel SetCurrentUserViewModel => Kernel.Get<SetCurrentUserViewModel>();
        public static AddTaskViewModel AddTaskViewModel => Kernel.Get<AddTaskViewModel>();
        public static AddTaskStateViewModel AddTaskStateViewModel => Kernel.Get<AddTaskStateViewModel>();
        public static RegistryViewModel RegistryViewModel => Kernel.Get<RegistryViewModel>();
        public static FiltersViewModel FiltersViewModel => Kernel.Get<FiltersViewModel>();
        public static CurrentUserViewModel CurrentUserViewModel => Kernel.Get<CurrentUserViewModel>();

        public static void Cleanup()
        {
        }
    }
}