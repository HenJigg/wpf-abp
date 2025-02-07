using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Storage
{
    /// <summary>
    /// ���ݴ洢���� 
    /// </summary>
    public interface IDataStorageService
    {
        T GetValue<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false);

        void SetValue<T>(string key, T value, bool shouldEncrypt = false);

        void Remove(string key);
    }
}