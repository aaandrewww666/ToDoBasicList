namespace ToDoBasicList.Services.Contracts
{
    public interface IWindowService
    {
        void Show();
        void Hide();
        void Toggle();
        void SetWindowBasePosition();
        void Pin();
        void Unpin();
        void Close();
    }
}
