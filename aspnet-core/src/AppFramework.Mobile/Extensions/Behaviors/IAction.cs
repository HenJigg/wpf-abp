using Xamarin.Forms.Internals;

namespace AppFramework.Shared.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}