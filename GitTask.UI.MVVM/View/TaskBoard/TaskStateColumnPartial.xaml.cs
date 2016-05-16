using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.TaskBoard;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskStateColumnPartial
    {
        public TaskStateColumnPartial()
        {
            Messenger.Default.Register<DistributeTaskStateColumnsMessage>(this, OnDistributeTaskStateColumnsMessage);
            Unloaded += OnUnloaded;
            InitializeComponent();
            OnDataContextChanged(null, new DependencyPropertyChangedEventArgs());
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(DataContext is TaskStateColumnViewModel)) return;
            var taskBoardPartialViewModel = Locator.IocLocator.TaskBoardViewModel;
            OnDistributeTaskStateColumnsMessage(new DistributeTaskStateColumnsMessage
            {
                OpenedTaskStateColumnWidth = taskBoardPartialViewModel.CurrentOpenedTaskStateColumnWidth
            });
            DataContextChanged += OnDataContextChanged;
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