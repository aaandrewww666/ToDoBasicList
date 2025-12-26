
using Avalonia;
using Avalonia.Controls;
using System;
using ToDoBasicList.Services.Contracts;

namespace ToDoBasicList.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window _window;

        public WindowService(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public void Show()
        {
            if (_window.WindowState == WindowState.Minimized)
                _window.WindowState = WindowState.Normal;

            _window.Show();
            _window.Activate();
            _window.Focus();
        }

        public void Hide()
        {
            if (_window.WindowState == WindowState.Normal)
                _window.WindowState = WindowState.Minimized;
            _window.Hide();
        }

        public void Toggle()
        {
            if (_window.IsVisible)
                Hide();
            else
                Show();
        }

        public void SetWindowBasePosition() //Base = right left 
        {
            var screens = _window.Screens;
            var primaryScreen = screens.Primary;

            if (primaryScreen == null) return;

            var workingArea = primaryScreen.WorkingArea;
            var left = workingArea.Right - _window.Width;
            var top = workingArea.Bottom - _window.Height;

            _window.Position = new PixelPoint((int)left, (int)top);
        }

        public void Pin()
        {
            _window.Topmost = true;
        }

        public void Unpin()
        {
            _window.Topmost = false;
        }

        public void Close()
        {
            _window.Close();
        }
    }
}
