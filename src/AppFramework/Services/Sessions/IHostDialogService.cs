using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace AppFramework.Services
{
    /// <summary>
    /// 对话主机服务接口
    /// </summary>
    public interface IHostDialogService : IDialogService
    {
        Task<IDialogResult> ShowDialogAsync(
            string name,
            IDialogParameters parameters = null,
            string IdentifierName = "Root");

        IDialogResult ShowWindow(string name);
    }
}