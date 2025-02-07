 using System.Threading.Tasks;

namespace AppFramework.Shared.Services.App
{
    public interface IUpdateService
    {
        Task CheckVersion();
    }
}
