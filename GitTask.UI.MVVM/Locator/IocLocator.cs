using GitTask.UI.MVVM.ViewModel.ButtonsBar;
using GitTask.UI.MVVM.ViewModel.Elements;
using GitTask.UI.MVVM.ViewModel.Footer;
using GitTask.UI.MVVM.ViewModel.Main;
using GitTask.UI.MVVM.ViewModel.ProjectSettings;
using GitTask.UI.MVVM.ViewModel.TaskBoard;
using Ninject;

namespace GitTask.UI.MVVM.Locator
{
    public class IocLocator
    {
        private static readonly IKernel Kernel;

        static IocLocator()
        {
            Kernel = new StandardKernel(new IocModule());
        }

        public static MainViewModel MainViewModel => Kernel.Get<MainViewModel>();
        public static ProjectOpeningViewModel ProjectOpeningViewModel => Kernel.Get<ProjectOpeningViewModel>();
        public static TaskBoardViewModel TaskBoardViewModel => Kernel.Get<TaskBoardViewModel>();
        public static FooterViewModel FooterViewModel => Kernel.Get<FooterViewModel>();
        public static ProjectSetupViewModel ProjectSetupViewModel => Kernel.Get<ProjectSetupViewModel>();
        public static PendingStorageOperationsViewModel PendingStorageOperationsViewModel => Kernel.Get<PendingStorageOperationsViewModel>();
        public static ButtonsBarViewModel ButtonsBarViewModel => Kernel.Get<ButtonsBarViewModel>();
        public static ProjectMembersViewModel ProjectMembersViewModel => Kernel.Get<ProjectMembersViewModel>();
        public static SetCurrentUserViewModel SetCurrentUserViewModel => Kernel.Get<SetCurrentUserViewModel>();

        public static void Cleanup()
        {
        }
    }
}