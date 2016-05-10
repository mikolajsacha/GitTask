using GalaSoft.MvvmLight.Threading;

namespace GitTask.UI.MVVM
{
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}