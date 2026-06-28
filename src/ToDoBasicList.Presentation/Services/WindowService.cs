
using Avalonia;
using Avalonia.Controls;
using System;
using ToDoBasicList.Presentation.Services.Contracts;

namespace ToDoBasicList.Presentation.Services
{
    /// <summary>
    /// Service to manage Window properties
    /// </summary>
    public sealed class WindowService : IWindowService
    {
        private readonly Window _window;

        public WindowService(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            // Position only sticks once the window is actually shown
            _window.Opened += OnWindowOpened;
        }

        private void OnWindowOpened(object? sender, EventArgs e)
        {
            _window.Opened -= OnWindowOpened;
            SetWindowBasePosition();
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

        /// <summary>
        /// Base window position is the bottom right corner
        /// </summary>
        public void SetWindowBasePosition()
        {
            var primaryScreen = _window.Screens.Primary;

            if (primaryScreen == null) return;

            var workingArea = primaryScreen.WorkingArea;
            var scaling = primaryScreen.Scaling;

            var size = _window.FrameSize ?? new Size(_window.Width, _window.Height);

            var left = workingArea.Right - (int)(size.Width * scaling);
            var top = workingArea.Bottom - (int)(size.Height * scaling);

            _window.Position = new PixelPoint(left, top);
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
