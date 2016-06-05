using GalaSoft.MvvmLight;

namespace GitTask.UI.MVVM.ViewModel.History
{
    public class BaseChangeViewModel<T> : ViewModelBase
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }

        public BaseChangeViewModel(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}