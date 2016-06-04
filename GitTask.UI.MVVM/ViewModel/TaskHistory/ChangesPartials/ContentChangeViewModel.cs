namespace GitTask.UI.MVVM.ViewModel.TaskHistory.ChangesPartials
{
    public class ContentChangeViewModel : BaseChangeViewModel<string>
    {
        public ContentChangeViewModel(string oldValue, string newValue) : base(oldValue, newValue)
        { }
    }
}