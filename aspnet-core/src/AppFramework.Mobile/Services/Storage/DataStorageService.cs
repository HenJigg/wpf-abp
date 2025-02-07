using Abp.Runtime.Security;
using AppFramework.Shared.Services.Storage;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Storage
{ 
    public class DataStorageService : IDataStorageService
    {
        private static void SetValue(string key, object value)
        {
            if (value == null)
                CrossSettings.Current.AddOrUpdateValue(key, null);
            else
                CrossSettings.Current.AddOrUpdateValue(key, value.ToString());
        }

        private static void SetJsonValue(string key, object value)
        {
            CrossSettings.Current.AddOrUpdateValue(key, JsonConvert.SerializeObject(value));
        }

        private T GetPrimitive<T>(string key, T defaultValue = default(T))
        {
            if (!ContainsKey(key)) return defaultValue;

            return (T)Convert.ChangeType(CrossSettings.Current.GetValueOrDefault(key, null), typeof(T));
        }

        private T RetrieveObject<T>(string key, T defaultValue = default(T))
        {
            return !ContainsKey(key) ? defaultValue : JsonConvert.DeserializeObject<T>(Convert.ToString(CrossSettings.Current.GetValueOrDefault(key, "")));
        }

        public bool ContainsKey(string key) => CrossSettings.Current.Contains(key);

        public T GetValue<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false)
        {
            var value = TypeHelperExtended.IsPrimitive(typeof(T), false) ?
                GetPrimitive(key, defaultValue) :
                RetrieveObject(key, defaultValue);

            if (!shouldDecrpyt) return value;

            var decrypted = SimpleStringCipher.Instance.Decrypt(Convert.ToString(value));
            return (T)Convert.ChangeType(decrypted, typeof(T));
        }

        public void SetValue<T>(string key, T value, bool shouldEncrypt = false)
        {
            if (TypeHelperExtended.IsPrimitive(typeof(T), false))
            {
                if (shouldEncrypt)
                    SetValue(key, SimpleStringCipher.Instance.Encrypt(Convert.ToString(value)));
                else
                    SetValue(key, value);
            }
            else
                SetJsonValue(key, value);
        }

        public void Remove(string key)
        {
            if (ContainsKey(key)) CrossSettings.Current.Remove(key);
        }
    }
}