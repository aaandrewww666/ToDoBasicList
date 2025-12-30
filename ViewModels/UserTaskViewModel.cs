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
        /// Default constructor
        /// </summary>
        /// <param name="taskDescription"> Some text to make task </param>
        /// <param name="DeleteUserTaskAction"> Delegate for logic of deleting <see cref="UserTaskViewModel"/> from another place </param>
        public UserTaskViewModel(string taskDescription, Action<UserTaskViewModel> DeleteUserTaskAction)
        {
            this.taskDescription = taskDescription;
            _deleteUserTaskAction = DeleteUserTaskAction;
        }

        /// <summary>
        /// Delegate for logic of deleting <see cref="UserTaskViewModel"/> from another place
        /// </summary>
        private Action<UserTaskViewModel> _deleteUserTaskAction;

        /// <summary>
        /// RelayCommand for invoking delegate
        /// </summary>
        [RelayCommand]
        private void RemoveTask()
        {
            _deleteUserTaskAction?.Invoke(this);
        }

        /// <summary>
        /// Some text (user task)
        /// </summary>
        [ObservableProperty]
        private string taskDescription;
    }
}
