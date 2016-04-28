using System.Windows;
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
            Unloaded += OnUnloaded;
            InitializeComponent();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Messenger.Default.Unregister<DistributeTaskStateColumnsMessage>(this, OnDistributeTaskStateColumnsMessage);
        }

        private void OnDistributeTaskStateColumnsMessage(DistributeTaskStateColumnsMessage message)
        {
            if (((TaskStateColumnViewModel)DataContext).IsOpened)
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