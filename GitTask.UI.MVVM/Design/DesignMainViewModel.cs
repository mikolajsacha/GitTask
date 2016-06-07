using GalaSoft.MvvmLight;

namespace GitTask.UI.MVVM.Design
{
    public class DesignMainViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.Main.MainViewModel
    {

        public bool IsTaskBoardVisible => true;
        public bool IsProjectInitializerVisible => false;
        public bool IsMergingToolVisible => false;
    }
}