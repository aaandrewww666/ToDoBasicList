using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using ToDoBasicList.Services;
using ToDoBasicList.ViewModels;
using ToDoBasicList.Views;

namespace ToDoBasicList
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();

                var windowService = new WindowService(mainWindow);

                var viewModel = new MainViewModel(windowService);

                mainWindow.DataContext = viewModel;

                var trayService = new TrayIconService(viewModel);

                desktop.MainWindow = mainWindow;

                AppDomain.CurrentDomain.ProcessExit += (_, _) => trayService.Dispose();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}