using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToDoBasicList.Core.Contracts;
using ToDoBasicList.Core.Models;
using ToDoBasicList.Presentation.Extensions;
using ToDoBasicList.Presentation.Services.Contracts;

namespace ToDoBasicList.Presentation.ViewModels
{
    /// <summary>
    /// Main ViewModel for managing MainWindow and another application logic
    /// </summary>
    public sealed partial class MainViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;
        private readonly ITaskStorageService _taskStorage;

        /// <summary>
        /// Debounce timer so editing a task does not write the file on every keystroke.
        /// </summary>
        private readonly DispatcherTimer _saveTimer;

        /// <summary>
        /// Suppresses saving while tasks are being restored at startup, so loading
        /// the file does not immediately schedule a redundant write of the same data.
        /// </summary>
        private bool _isLoading;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        public partial string UserInput { get; set; } = string.Empty;

        public ObservableCollection<UserTaskViewModel> UserTasks { get; } = [];
        public string AddButtonTipText { get; } = "Click the button to add new task";
        public string TasksLabelText { get; } = "Tasks:";

        public MainViewModel(IWindowService windowService, ITaskStorageService taskStorage)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _taskStorage = taskStorage ?? throw new ArgumentNullException(nameof(taskStorage));

            AddTaskCommand = new RelayCommand(
                execute: AddTask,
                canExecute: CanAddTask
                );

            _saveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _saveTimer.Tick += OnSaveTimerTick;

            UserTasks.CollectionChanged += OnTasksCollectionChanged;
            LoadTasksAsync().SafeFireAndForget();

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

        /// <summary>
        /// Restores saved tasks at startup.
        /// </summary>
        private async Task LoadTasksAsync()
        {
            var result = await _taskStorage.LoadAsync();
            if (result.IsFailure)
            {
                // TODO: surface result.Error to the user (e.g. status bar / dialog).
                return;
            }

            _isLoading = true;
            try
            {
                foreach (var dto in result.Value)
                    UserTasks.Add(new UserTaskViewModel(dto.Description, DeleteTask, dto.IsCompleted));
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Keeps per-item change subscriptions in sync and schedules a save whenever
        /// tasks are added or removed.
        /// </summary>
        private void OnTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (UserTaskViewModel item in e.OldItems)
                    item.PropertyChanged -= OnTaskPropertyChanged;

            if (e.NewItems != null)
                foreach (UserTaskViewModel item in e.NewItems)
                    item.PropertyChanged += OnTaskPropertyChanged;

            ScheduleSave();
        }

        /// <summary>
        /// Schedules a save when a task's text or completion status changes.
        /// </summary>
        private void OnTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(UserTaskViewModel.TaskDescription)
                or nameof(UserTaskViewModel.IsCompleted))
            {
                ScheduleSave();
            }
        }

        private void ScheduleSave()
        {
            if (_isLoading)
                return;

            _saveTimer.Stop();
            _saveTimer.Start();
        }

        private void OnSaveTimerTick(object? sender, EventArgs e)
        {
            _saveTimer.Stop();
            SaveTasksAsync().SafeFireAndForget();
        }

        private async Task SaveTasksAsync()
        {
            // Snapshot on the UI thread before awaiting, so serialization never enumerates
            // the ObservableCollection off-thread.
            var snapshot = UserTasks.Select(t => new TaskDto(t.TaskDescription, t.IsCompleted)).ToList();

            var result = await _taskStorage.SaveAsync(snapshot);
            if (result.IsFailure)
            {
                // TODO: surface result.Error to the user (e.g. status bar / dialog).
            }
        }
    }
}
