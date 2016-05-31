using System;

namespace GitTask.UI.MVVM.View.Main
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AdjustSize();
        }

        private void AdjustSize()
        {
            Height = Math.Min(System.Windows.SystemParameters.PrimaryScreenHeight - 100, Height);
            Width = Math.Min(System.Windows.SystemParameters.PrimaryScreenWidth - 100, Width);
        }
    }
}