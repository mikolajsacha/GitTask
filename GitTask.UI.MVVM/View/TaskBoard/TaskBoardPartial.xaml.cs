using System;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel.TaskBoard;

namespace GitTask.UI.MVVM.View.TaskBoard
{
    public partial class TaskBoardPartial
    {
        private const int WindowBorderWidth = 16;
        private int _openedTaskStateColumnsCount;
        private int _hiddenTaskStateColumnsCount;

        public TaskBoardPartial()
        {
            InitializeComponent();
            SizeChanged += OnMainWindowSizeChanged;

            var model = DataContext as TaskBoardViewModel;
            if (model == null) return;

            model.PropertyChanged += OnDataContextPropertyChanged;
            model.InitializeTaskStateColumns();
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "HiddenColumnsCount") return;

            _openedTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).OpenedColumnsCount;
            _hiddenTaskStateColumnsCount = ((TaskBoardViewModel)DataContext).HiddenColumnsCount;
            SendDistributeTaskStateColumnsMessage();
        }

        private void OnMainWindowSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            SendDistributeTaskStateColumnsMessage();
        }

        private void SendDistributeTaskStateColumnsMessage()
        {
            var hiddenTaskStateColumnWidth = (double)FindResource("HiddenTaskStateColumnWidth");
            var minimumOpenedTaskStateColumnWidth = (double)FindResource("MinimumOpenedTaskStateColumnWidth");

            var allHiddenTaskStateColumnsWidth = hiddenTaskStateColumnWidth * _hiddenTaskStateColumnsCount;
            var minimumWidthOfAllOpenedTaskStateColumns = minimumOpenedTaskStateColumnWidth * _openedTaskStateColumnsCount;
            var allOpenedTaskStateColumnsWidth = Math.Max(minimumWidthOfAllOpenedTaskStateColumns,
                RenderSize.Width - WindowBorderWidth - allHiddenTaskStateColumnsWidth);
            var newWidthForOpenTaskStateColumns = allOpenedTaskStateColumnsWidth / _openedTaskStateColumnsCount;

            Messenger.Default.Send(new DistributeTaskStateColumnsMessage
            {
                OpenedTaskStateColumnWidth = newWidthForOpenTaskStateColumns
            });
        }
    }
}