using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace GitTask.UI.MVVM.Design
{
    public class DesignPendingStorageOperationsViewModel : ViewModelBase //Based on GitTask.UI.MVVM.ViewModel.Elements.PendingStorageOperationsViewModel
    {
        public ObservableCollection<string> PendingStorageSaveOperations;
        public int PendingStorageOperationsCount => PendingStorageSaveOperations.Count;
        public bool AnyPendingStorageOperations => PendingStorageSaveOperations.Any();

        public DesignPendingStorageOperationsViewModel()
        {
            PendingStorageSaveOperations = new ObservableCollection<string>
            {
                "C://folderprojektu/gittask/task/5.json",
                "C://folderprojektu/gittask/taskstate/1.json",
                "C://folderprojektu/gittask/comment/3.json"
            };
        }
    }
}