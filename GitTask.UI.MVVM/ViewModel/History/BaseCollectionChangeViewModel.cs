using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GitTask.UI.MVVM.ViewModel.History
{
    public class BaseCollectionChangeViewModel<T> : BaseChangeViewModel<IEnumerable<T>>
    {
        public ObservableCollection<T> AddedObjects { get; }
        public ObservableCollection<T> RemovedObjects { get; }

        public bool AnyObjectsAdded => AddedObjects.Any();
        public bool AnyObjectsRemoved => RemovedObjects.Any();

        public BaseCollectionChangeViewModel(IEnumerable<T> oldValue, IEnumerable<T> newValue)
                                     : base(oldValue, newValue)
        {
            AddedObjects = new ObservableCollection<T>();
            RemovedObjects = new ObservableCollection<T>();
            ResolveRemovedObjects();
            ResolveAddedObjects();
        }

        private void ResolveRemovedObjects()
        {
            RemovedObjects.Clear();
            foreach (var removedMember in OldValue.Where(pm => !NewValue.Contains(pm)))
            {
                RemovedObjects.Add(removedMember);
            }
        }

        private void ResolveAddedObjects()
        {
            AddedObjects.Clear();
            foreach (var addedMember in NewValue.Where(pm => !OldValue.Contains(pm)))
            {
                AddedObjects.Add(addedMember);
            }
        }
    }
}