using System;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GitTask.UI.MVVM.Locator;
using GitTask.UI.MVVM.Messages;
using GitTask.UI.MVVM.ViewModel;

namespace GitTask.UI.MVVM.View
{
    public partial class MainWindow
    {
        private const int WindowBorderWidth = 16;
        private int _openedTaskStateColumnsCount;
        private int _hiddenTaskStateColumnsCount;

        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => IocLocator.Cleanup();
            SizeChanged += OnMainWindowSizeChanged;
            ((MainViewModel)DataContext).PropertyChanged += OnDataContextPropertyChanged;
            ((MainViewModel)DataContext).InitializeTaskStateColumns();
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "HiddenColumnsCount")
            {
                _openedTaskStateColumnsCount = ((MainViewModel) DataContext).OpenedColumnsCount;
                _hiddenTaskStateColumnsCount = ((MainViewModel) DataContext).HiddenColumnsCount;
                SendDistributeTaskStateColumnsMessage();
            }
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
                Width - WindowBorderWidth - allHiddenTaskStateColumnsWidth);
            var newWidthForOpenTaskStateColumns = allOpenedTaskStateColumnsWidth / _openedTaskStateColumnsCount;

            Messenger.Default.Send(new DistributeTaskStateColumnsMessage
            {
                OpenedTaskStateColumnWidth = newWidthForOpenTaskStateColumns
            });
        }
    }
}