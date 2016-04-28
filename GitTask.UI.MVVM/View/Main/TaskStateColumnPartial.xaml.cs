using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.Main;

namespace GitTask.UI.MVVM.View.Main
{
    public partial class TaskStateColumnPartial
    {
        public TaskStateColumnPartial()
        {
            Messenger.Default.Register<DistributeTaskStateColumnsMessage>(this, OnDistributeTaskStateColumnsMessage);
            InitializeComponent();
        }

        private void OnDistributeTaskStateColumnsMessage(DistributeTaskStateColumnsMessage message)
        {
            if (((TaskStateColumn)DataContext).IsOpened)
            {
                Width = message.OpenedTaskStateColumnWidth;
            }
            else
            {
                Width = (double)FindResource("HiddenTaskStateColumnWidth");
            }
        }
    }
}