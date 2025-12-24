using Avalonia.Controls;
using Avalonia.Platform;
using System;
using ToDoBasicList.ViewModels;

namespace ToDoBasicList.Services
{
    public interface ITrayIconService
    {
        void Initialize(MainViewModel mainViewModel, Window mainWindow);
    }

    public class TrayIconService : ITrayIconService
    {
        private TrayIcon _trayIcon;
        private MainViewModel _mainViewModel;
        private Window _mainWindow;

        public void Initialize(MainViewModel mainViewModel, Window mainWindow)
        {
            _mainViewModel = mainViewModel;
            _mainWindow = mainWindow;

            CreateTrayIcon();
        }

        private void CreateTrayIcon()
        {
            _trayIcon = new TrayIcon();

            _trayIcon.Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://ToDoBasicList/Assets/icons8-task-16.png")));
            _trayIcon.ToolTipText = "ToDo's app";

            _trayIcon.Clicked += (s, e) => ToggleWindowStatus(); // left click makes window hide or show

            CreateContextMenu();
            _trayIcon.IsVisible = true;
        }

        private void ToggleWindowStatus()
        {
            if (_mainWindow.IsVisible)
            {
                _mainWindow.Hide();
            }
            else
            {
                _mainWindow.Show();
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
            }
        }

        private void CreateContextMenu()
        {
            var contextMenu = new NativeMenu();

            // Pin
            var pinItem = new NativeMenuItem("Pin");
            contextMenu.Add(pinItem);

            // Unpin
            var unpinItem = new NativeMenuItem("Unpin");
            contextMenu.Add(unpinItem);

            // Reset
            var resetLocationItem = new NativeMenuItem("Reset window location to default");
            contextMenu.Add(resetLocationItem);

            contextMenu.Add(new NativeMenuItemSeparator());

            // Exit
            var exitItem = new NativeMenuItem("Close application");
            exitItem.Click += (s, e) =>
            {
                _mainWindow.Close();
            };
            contextMenu.Add(exitItem);

            _trayIcon.Menu = contextMenu;
        }

    }
}
