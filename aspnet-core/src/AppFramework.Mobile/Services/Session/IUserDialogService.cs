namespace AppFramework.Shared
{
    public interface IUserDialogService
    {
        void Show(string message);

        void Hide();
    }
}