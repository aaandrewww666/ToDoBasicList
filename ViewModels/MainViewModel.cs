using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ToDoBasicList.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<UserTaskViewModel> UserTasks { get; } = [];

        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(
                execute: AddTask,
                canExecute: CanAddTask
                );

            PinWindowCommand = new RelayCommand(Pin);
            UnpinWindowCommand = new RelayCommand(() => { /* Logic to Unpin */ });
            ResetLocationCommand = new RelayCommand(() => { /* Logic to Reset */ });
            ExitApplicationCommand = new RelayCommand(() => { /* Logic to Exit */ });

            ToggleWindowCommand = new RelayCommand(() =>
            {
                // Logic to Show/Hide window when icon is clicked
                Debug.WriteLine("Tray Icon Clicked!");
            });
        }

        public ICommand PinWindowCommand { get; }
        public ICommand UnpinWindowCommand { get; }
        public ICommand ResetLocationCommand { get; }
        public ICommand ExitApplicationCommand { get; }
        public ICommand ToggleWindowCommand { get; }


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
