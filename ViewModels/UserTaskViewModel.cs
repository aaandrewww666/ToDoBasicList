using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ToDoBasicList.ViewModels
{
    /// <summary>
    /// ViewModel for text-view based on user input
    /// </summary>
    public sealed partial class UserTaskViewModel : ViewModelBase
    {
        /// <summary>
        /// Some text (user task)
        /// </summary>
        [ObservableProperty]
        public partial string TaskDescription { get; set; }

        /// <summary>
        /// Whether the task is marked as done
        /// </summary>
        [ObservableProperty]
        public partial bool IsCompleted { get; set; }

        /// <summary>
        /// Delegate for logic of deleting <see cref="UserTaskViewModel"/> from another place
        /// </summary>
        private readonly Action<UserTaskViewModel> _deleteUserTaskAction;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="taskDescription"> Some text to make task </param>
        /// <param name="deleteUserTaskAction"> Delegate for logic of deleting <see cref="UserTaskViewModel"/> from another place </param>
        public UserTaskViewModel(string taskDescription, Action<UserTaskViewModel> deleteUserTaskAction)
        {
            TaskDescription = taskDescription;
            _deleteUserTaskAction = deleteUserTaskAction;
        }
        
        /// <summary>
        /// RelayCommand for invoking delegate
        /// </summary>
        [RelayCommand]
        private void RemoveTask()
        {
            _deleteUserTaskAction?.Invoke(this);
        }
    }
}
