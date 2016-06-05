namespace GitTask.UI.MVVM.ViewModel.History.TaskHistory.ChangesPartials
{
    public class ContentChangeViewModel : BaseChangeViewModel<string>
    {
        public ContentChangeViewModel(string oldValue, string newValue) : base(oldValue, newValue)
        { }
    }
}