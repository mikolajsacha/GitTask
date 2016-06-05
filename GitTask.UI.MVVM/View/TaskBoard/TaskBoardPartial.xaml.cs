using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.TaskBoard;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskBoardPartial
    {
        private int _openedTaskStateColumnsCount;
        private int _hiddenTaskStateColumnsCount;

        public TaskBoardPartial()
        {
            InitializeComponent();
            SizeChanged += OnTaskBoardSizeChanged;

            var model = DataContext as TaskBoardViewModel;
            if (model == null) return;

            model.PropertyChanged += OnDataContextPropertyChanged;
            model.TaskStateColumns.CollectionChanged += TaskStateColumnsOnCollectionChanged;

            _openedTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).OpenedColumnsCount;
            _hiddenTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).HiddenColumnsCount;
            model.InitializeTaskStateColumns();
        }

        private void TaskStateColumnsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _openedTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).OpenedColumnsCount;
            _hiddenTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).HiddenColumnsCount;

            SendDistributeTaskStateColumnsMessage(RenderSize.Width);
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "HiddenColumnsCount") return;

            _openedTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).OpenedColumnsCount;
            _hiddenTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).HiddenColumnsCount;
            SendDistributeTaskStateColumnsMessage(RenderSize.Width);
        }

        private void OnTaskBoardSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs?.NewSize != null)
            {
                SendDistributeTaskStateColumnsMessage(sizeChangedEventArgs.NewSize.Width);
            }
        }

        private void SendDistributeTaskStateColumnsMessage(double newWidth)
        {
            if (!(DataContext is TaskBoardViewModel)) return;

            var hiddenTaskStateColumnWidth = (double)FindResource("HiddenTaskStateColumnWidth");
            var minimumOpenedTaskStateColumnWidth = (double)FindResource("MinimumOpenedTaskStateColumnWidth");

            var allHiddenTaskStateColumnsWidth = hiddenTaskStateColumnWidth * _hiddenTaskStateColumnsCount;
            var minimumWidthOfAllOpenedTaskStateColumns = minimumOpenedTaskStateColumnWidth * _openedTaskStateColumnsCount;
            var allOpenedTaskStateColumnsWidth = Math.Max(minimumWidthOfAllOpenedTaskStateColumns,
                newWidth - 8 - allHiddenTaskStateColumnsWidth);
            var newWidthForOpenTaskStateColumns = allOpenedTaskStateColumnsWidth / _openedTaskStateColumnsCount;

            Messenger.Default.Send(new DistributeTaskStateColumnsMessage
            {
                OpenedTaskStateColumnWidth = newWidthForOpenTaskStateColumns
            });
            
            ((TaskBoardViewModel)DataContext).CurrentOpenedTaskStateColumnWidth = newWidthForOpenTaskStateColumns;
        }
    }
}