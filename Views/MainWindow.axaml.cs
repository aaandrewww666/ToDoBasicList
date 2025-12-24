using Avalonia;
using Avalonia.Controls;
using System;

namespace ToDoBasicList.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
            Opened += OnWindowOpened;
        }

        private void OnWindowOpened(object? sender, EventArgs e)
        {
            // Get screens from the window's Screens property
            var screens = Screens;
            var primaryScreen = screens.Primary;

            if (primaryScreen == null) return;

            var workingArea = primaryScreen.WorkingArea;
            var left = workingArea.Right - Width;
            var top = workingArea.Bottom - Height;

            Position = new PixelPoint((int)left, (int)top);
        }
    }
}