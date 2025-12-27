using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace ToDoBasicList.Views
{
    public partial class MainWindow : Window
    {
        private bool _isDragging;
        private PixelPoint _startMousePosition;
        private WindowEdge? _resizeEdge;
        private Rect _startBounds;

        private const int ResizeMargin = 6;

        public MainWindow()
        {
            InitializeComponent();

            AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
            AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel);
            AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel);
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;

            var pos = e.GetPosition(RootGrid);
            _resizeEdge = GetResizeEdge(pos);

            if (_resizeEdge != null)
            {
                StartDrag(e);
                return;
            }

            if (IsInteractiveElement(e.Source))
                return;

            StartDrag(e);
        }

        private void StartDrag(PointerPressedEventArgs e)
        {
            _startMousePosition = this.PointToScreen(e.GetPosition(this));
            _startBounds = new Rect(Position.X, Position.Y, Width, Height);
            _isDragging = true;
            e.Handled = true;
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            var pos = e.GetPosition(RootGrid);

            Cursor = GetCursor(GetResizeEdge(pos));

            if (!_isDragging) return;

            var currentPos = this.PointToScreen(e.GetPosition(this));
            var deltaX = currentPos.X - _startMousePosition.X;
            var deltaY = currentPos.Y - _startMousePosition.Y;

            if (_resizeEdge == null)
            {
                Position = new PixelPoint(
                    (int)(_startBounds.X + deltaX),
                    (int)(_startBounds.Y + deltaY));
            }
            else
            {
                ApplyResize(_resizeEdge.Value, deltaX, deltaY);
            }

            e.Handled = true;
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _resizeEdge = null;
                e.Handled = true;
            }
        }

        private bool IsInteractiveElement(object? source)
        {
            var element = source as Control;

            while (element != null)
            {
                if (element is TextBox or Button or ScrollBar or Slider or CheckBox or ComboBox or ListBox)
                    return true;

                element = element.Parent as Control;
            }

            return false;
        }

        private WindowEdge? GetResizeEdge(Point pos)
        {
            bool left = pos.X < ResizeMargin;
            bool right = pos.X > RootGrid.Bounds.Width - ResizeMargin;
            bool top = pos.Y < ResizeMargin;
            bool bottom = pos.Y > RootGrid.Bounds.Height - ResizeMargin;

            return (left, right, top, bottom) switch
            {
                (true, _, true, _) => WindowEdge.NorthWest,
                (true, _, _, true) => WindowEdge.SouthWest,
                (_, true, true, _) => WindowEdge.NorthEast,
                (_, true, _, true) => WindowEdge.SouthEast,
                (true, _, _, _) => WindowEdge.West,
                (_, true, _, _) => WindowEdge.East,
                (_, _, true, _) => WindowEdge.North,
                (_, _, _, true) => WindowEdge.South,
                _ => null
            };
        }

        private Cursor GetCursor(WindowEdge? edge) => edge switch
        {
            WindowEdge.North or WindowEdge.South => new Cursor(StandardCursorType.SizeNorthSouth),
            WindowEdge.West or WindowEdge.East => new Cursor(StandardCursorType.SizeWestEast),
            WindowEdge.NorthWest or WindowEdge.SouthEast => new Cursor(StandardCursorType.TopLeftCorner),
            WindowEdge.NorthEast or WindowEdge.SouthWest => new Cursor(StandardCursorType.TopRightCorner),
            _ => Cursor.Default
        };

        private void ApplyResize(WindowEdge edge, double deltaX, double deltaY)
        {
            double x = _startBounds.X;
            double y = _startBounds.Y;
            double w = _startBounds.Width;
            double h = _startBounds.Height;

            if (edge is WindowEdge.East or WindowEdge.NorthEast or WindowEdge.SouthEast)
                w = _startBounds.Width + deltaX;

            if (edge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest)
            {
                w = _startBounds.Width - deltaX;
                x = _startBounds.X + deltaX;
            }

            if (edge is WindowEdge.South or WindowEdge.SouthEast or WindowEdge.SouthWest)
                h = _startBounds.Height + deltaY;

            if (edge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest)
            {
                h = _startBounds.Height - deltaY;
                y = _startBounds.Y + deltaY;
            }

            double clampedW = Math.Clamp(w, MinWidth, MaxWidth);
            double clampedH = Math.Clamp(h, MinHeight, MaxHeight);

            if (w != clampedW && edge.ToString().Contains("West"))
                x = _startBounds.X + _startBounds.Width - clampedW;

            if (h != clampedH && edge.ToString().Contains("North"))
                y = _startBounds.Y + _startBounds.Height - clampedH;

            Width = clampedW;
            Height = clampedH;
            Position = new PixelPoint((int)x, (int)y);
        }
    }
}