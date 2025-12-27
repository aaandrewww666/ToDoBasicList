using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using System;

namespace ToDoBasicList.Views.Controls;

public partial class ExpandingTextBox : UserControl
{
    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<ExpandingTextBox, string?>(nameof(Placeholder), "Enter your task...");

    public static readonly StyledProperty<double> MaxHeightProperty =
        AvaloniaProperty.Register<ExpandingTextBox, double>(nameof(MaxHeight), 65.0);

    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<ExpandingTextBox, string?>(nameof(Message), string.Empty);

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public double MaxHeight
    {
        get => GetValue(MaxHeightProperty);
        set => SetValue(MaxHeightProperty, value);
    }

    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public ExpandingTextBox()
    {
        InitializeComponent();

        PART_TextBox.TextChanged += (s, e) => Dispatcher.UIThread.Post(UpdateHeight, DispatcherPriority.Render);

    }
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(UpdateHeight, DispatcherPriority.Render);
    }

    private void UpdateHeight()
    {
        var availableWidth = PART_TextBox.Bounds.Width;
        if (availableWidth <= 0) return;

        PART_TextBox.Measure(new Size(availableWidth, double.PositiveInfinity));
        var desiredHeight = PART_TextBox.DesiredSize.Height;

        var extra = PART_TextBox.Padding.Top + PART_TextBox.Padding.Bottom + 12;
        desiredHeight += extra;

        var targetHeight = Math.Max(48, Math.Min(MaxHeight, desiredHeight));

        if (Math.Abs(Height - targetHeight) < 2) return;

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(180),
            Easing = new CubicEaseOut(),
            Children =
            {
                new KeyFrame
                {
                    Setters = { new Setter(HeightProperty, targetHeight) },
                    Cue = new Cue(1.0)
                }
            }
        };

        animation.RunAsync(this);
    }
    private void OnClearClick(object sender, RoutedEventArgs e)
    {
        PART_TextBox.Text = string.Empty;
        PART_TextBox.Focus();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateHeight();
    }
}