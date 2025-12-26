using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using ToDoBasicList.Services.Contracts;

namespace ToDoBasicList.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;
        public ObservableCollection<UserTaskViewModel> UserTasks { get; } = [];

        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(IWindowService));
            AddTaskCommand = new RelayCommand(
                execute: AddTask,
                canExecute: CanAddTask
                );
        }

        [RelayCommand]
        private void PinWindow() => _windowService.Pin();

        [RelayCommand]
        private void UnpinWindow() => _windowService.Unpin();

        [RelayCommand]
        private void SetWindowBasePosition() => _windowService.SetWindowBasePosition();

        [RelayCommand]
        private void ToggleWindow() => _windowService.Toggle();

        [RelayCommand]
        private void ExitApplication() => _windowService.Close();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        private string userInput = string.Empty;

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(UserInput) && !IsAddingTask;

        public IRelayCommand AddTaskCommand { get; }

        private bool IsAddingTask = false;

        public void Pin()
        {

        }

        public void AddTask()
        {
            IsAddingTask = true;

            try
            {
                UserTasks.Add(new UserTaskViewModel(UserInput, DeleteTask));

                UserInput = string.Empty;
            }
            finally
            {
                IsAddingTask = false;
            }
        }

        private void DeleteTask(UserTaskViewModel userTaskVMToDelete)
        {
            UserTasks.Remove(userTaskVMToDelete);
        }

        public string AddButtonTipText { get; } = "Click the button to add new task";
        public string TasksLabelText { get; } = "Tasks:";
    }
}
