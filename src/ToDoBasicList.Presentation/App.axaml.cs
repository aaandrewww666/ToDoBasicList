using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using ToDoBasicList.Infrastructure;
using ToDoBasicList.Presentation.Services;
using ToDoBasicList.Presentation.ViewModels;
using ToDoBasicList.Presentation.Views;

namespace ToDoBasicList.Presentation
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
                var taskStorage = new JsonTaskStorageService();

                var viewModel = new MainViewModel(windowService, taskStorage);

                mainWindow.DataContext = viewModel;

                var trayService = new TrayIconService(viewModel);

                desktop.MainWindow = mainWindow;

                AppDomain.CurrentDomain.ProcessExit += (_, _) => trayService.Dispose();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}