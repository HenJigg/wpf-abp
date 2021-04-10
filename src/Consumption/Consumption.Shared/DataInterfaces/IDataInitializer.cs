using System.Threading.Tasks;

namespace Consumption.Shared.DataInterfaces
{
    public interface IDataInitializer
    {
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        /// <returns></returns>
        Task InitSampleDataAsync();
    }
}
