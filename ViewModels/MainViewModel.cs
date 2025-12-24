using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ToDoBasicList.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

        }

        public ObservableCollection<UserTaskViewModel> UserTasks { get; } = [];


        private string _userInput = string.Empty;
        public string UserInput
        {
            get => _userInput;
            set
            {
                OnPropertyChanging(nameof(UserInput));
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }

        public ICommand AddTaskCommand { get; }
        public void AddTask(string taskDescription)
        {
            UserTasks.Add(new UserTaskViewModel());
        }

        public string AddButtonTipText { get; } = "Click the button to add task";
        public string TasksLabelText { get; } = "Tasks:";
    }
}
