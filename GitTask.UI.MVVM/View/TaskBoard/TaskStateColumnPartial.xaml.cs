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
            //TODO: dodać strzałki do przesuwania task state'a
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
            if (double.IsInfinity(message.OpenedTaskStateColumnWidth) ||
                double.IsNaN(message.OpenedTaskStateColumnWidth))
            {
                return;
            }
            if (((TaskStateColumnViewModel)DataContext).IsOpened)
            {
                Width = message.OpenedTaskStateColumnWidth;
            }
            else
            {
                Width = (double)FindResource("HiddenTaskStateColumnWidth");
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var dataContext = DataContext as TaskStateColumnViewModel;
            if (dataContext == null) return;

            if (!e.Data.GetDataPresent(DataFormats.StringFormat)) return;

            var dataTask = e.Data.GetData(DataFormats.StringFormat) as string;
            if (dataTask == null) return;

            Messenger.Default.Send(new MoveTaskToTaskStateMessage() { TaskName = dataTask, NewTaskStateName = dataContext.TaskState.Name });
        }
    }
}