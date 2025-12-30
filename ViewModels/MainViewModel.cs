using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using ToDoBasicList.Services.Contracts;

namespace ToDoBasicList.ViewModels
{
    /// <summary>
    /// Main ViewModel for managing MainWindow and another application logic
    /// </summary>
    public sealed partial class MainViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        private string userInput = string.Empty;

        public ObservableCollection<UserTaskViewModel> UserTasks { get; } = [];
        public string AddButtonTipText { get; } = "Click the button to add new task";
        public string TasksLabelText { get; } = "Tasks:";


        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(IWindowService));
            AddTaskCommand = new RelayCommand(
                execute: AddTask,
                canExecute: CanAddTask
                );

            SetWindowBasePosition();
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


        private bool CanAddTask() => !string.IsNullOrWhiteSpace(UserInput) && !IsAddingTask;

        /// <summary>
        /// RelayCommand for adding task to ObservableCollection
        /// </summary>
        public IRelayCommand AddTaskCommand { get; }

        private bool IsAddingTask = false;

        /// <summary>
        /// A method that adds a task to an ObservableCollection
        /// </summary>
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

        /// <summary>
        /// A method that removes a task from an ObservableCollection
        /// </summary>
        private void DeleteTask(UserTaskViewModel userTaskVMToDelete)
        {
            UserTasks.Remove(userTaskVMToDelete);
        }
    }
}
