using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Elements
{
    public class PendingStorageOperationsViewModel : ViewModelBase
    {
        public ObservableCollection<string> PendingStorageSaveOperations;

        public int PendingStorageOperationsCount => PendingStorageSaveOperations.Count;
        public bool AnyPendingStorageOperations => PendingStorageSaveOperations.Any();

        public PendingStorageOperationsViewModel(IEnumerable<IStorageService> storageServices)
        {
            PendingStorageSaveOperations = new ObservableCollection<string>();
            InitializeStorageServiceEvents(storageServices);
        }

        private void InitializeStorageServiceEvents(IEnumerable<IStorageService> storageServices)
        {
            foreach (var storageService in storageServices)
            {
                storageService.StorageOperationStarted += OnStorageServiceStarted;
                storageService.StorageOperationFinished += OnStorageServiceFinished;
            }
        }

        private void OnStorageServiceStarted(string filePath)
        {
            PendingStorageSaveOperations.Add(filePath);
            RaisePropertiesChanged();
        }

        private void OnStorageServiceFinished(string filePath)
        {
            PendingStorageSaveOperations.Remove(filePath);
            RaisePropertiesChanged();
        }

        private void RaisePropertiesChanged()
        {
            RaisePropertyChanged("PendingStorageOperationsCount");
            RaisePropertyChanged("AnyPendingStorageOperations");
        }
    }
}