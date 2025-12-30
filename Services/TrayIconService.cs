using Avalonia.Controls;
using Avalonia.Platform;
using System;
using ToDoBasicList.ViewModels;

namespace ToDoBasicList.Services
{
    /// <summary>
    /// Service for managing the tray icon of application in the Windows tray
    /// After use, it should be cleared
    /// </summary>
    public sealed class TrayIconService : IDisposable
    {
        private TrayIcon? _trayIcon;
        private readonly MainViewModel _mainViewModel;

        public TrayIconService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(MainViewModel));

            CreateTrayIcon();
        }

        private void CreateTrayIcon()
        {
            _trayIcon = new TrayIcon();

            _trayIcon.Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://ToDoBasicList/Assets/icons8-task-16.png")));
            _trayIcon.ToolTipText = "ToDo's app";

            _trayIcon.Clicked += (s, e) => _mainViewModel.ToggleWindowCommand.Execute(null); // left click makes window hide or show

            CreateContextMenu();
            _trayIcon.IsVisible = true;
        }

        private void CreateContextMenu()
        {
            var contextMenu = new NativeMenu();

            // Pin
            var pinItem = new NativeMenuItem("Pin");
            pinItem.Click += (s, e) => _mainViewModel.PinWindowCommand.Execute(null);
            contextMenu.Add(pinItem);

            // Unpin
            var unpinItem = new NativeMenuItem("Unpin");
            unpinItem.Click += (s, e) => _mainViewModel.UnpinWindowCommand.Execute(null);
            contextMenu.Add(unpinItem);

            // Reset
            var resetLocationItem = new NativeMenuItem("Reset window location to default");
            resetLocationItem.Click += (s, e) => _mainViewModel.SetWindowBasePositionCommand.Execute(null);
            contextMenu.Add(resetLocationItem);

            contextMenu.Add(new NativeMenuItemSeparator());

            // Exit
            var exitItem = new NativeMenuItem("Close application");
            exitItem.Click += (s, e) =>
            {
                _mainViewModel.ExitApplicationCommand.Execute(null);
            };
            contextMenu.Add(exitItem);

            if (_trayIcon != null)
                _trayIcon.Menu = contextMenu;
        }

        public void Dispose()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Dispose();
                _trayIcon = null;
            }
        }
    }
}
