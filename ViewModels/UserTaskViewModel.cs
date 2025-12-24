using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ToDoBasicList.ViewModels
{
    public partial class UserTaskViewModel : ViewModelBase
    {
        public UserTaskViewModel(string taskDescription, Action<UserTaskViewModel> DeleteUserTaskAction)
        {
            this.taskDescription = taskDescription;
            _deleteUserTaskAction = DeleteUserTaskAction;
        }

        private Action<UserTaskViewModel> _deleteUserTaskAction;

        [RelayCommand]
        private void RemoveTask()
        {
            _deleteUserTaskAction?.Invoke(this);
        }

        [ObservableProperty]
        private string taskDescription;
    }
}
