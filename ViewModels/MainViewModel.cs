using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        private string userInput = string.Empty;

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(UserInput) && !IsAddingTask;

        public IRelayCommand AddTaskCommand { get; }

        private bool IsAddingTask = false;

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
