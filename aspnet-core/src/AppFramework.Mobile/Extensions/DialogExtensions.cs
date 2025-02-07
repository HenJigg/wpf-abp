using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Shared
{
    public static class DialogExtensions
    {
        /// <summary>
        /// 确认删除方法
        /// </summary>
        /// <param name="dialogService">Prism IDialogService</param>
        /// <returns></returns>
        public static async Task<bool> DeleteConfirm(this IDialogService dialogService)
        {
            var dialogResult = await dialogService.ShowDialogAsync("MessageBoxView");
            if (dialogResult.Parameters!=null&&dialogResult.Parameters.ContainsKey("DialogResult"))
            {
                var result = dialogResult.Parameters.GetValue<bool>("DialogResult");
                return result;
            }
            return false;
        }
    }
}