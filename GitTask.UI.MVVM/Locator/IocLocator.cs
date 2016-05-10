using GitTask.UI.MVVM.ViewModel;
using GitTask.UI.MVVM.ViewModel.Footer;
using GitTask.UI.MVVM.ViewModel.Main;
using GitTask.UI.MVVM.ViewModel.Storage;
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

        public static void Cleanup()
        {
        }
    }
}