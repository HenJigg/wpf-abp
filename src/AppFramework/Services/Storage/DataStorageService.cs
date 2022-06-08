using Abp.Runtime.Security;
using AppFramework.Common.Services.Storage;
using Newtonsoft.Json;
using System;

namespace AppFramework.Services
{
    public class DataStorageService : IDataStorageService
    {
        private IniFile iniFile;
        private string Section = "App";
        private string appInitPath = AppDomain.CurrentDomain.BaseDirectory + "user.ini";

        public DataStorageService()
        {
            iniFile = new IniFile(appInitPath);
        }

        private void InternalSetValue(string key, object value)
        {
            if (value == null)
                iniFile.SetValue(Section, key, "");
            else
                iniFile.SetValue(Section, key, value.ToString());
        }

        private void SetJsonValue(string key, object value)
        {
            iniFile.SetValue(Section, key, JsonConvert.SerializeObject(value));
        }

        private T GetPrimitive<T>(string key, T defaultValue = default(T))
        {
            return (T)Convert.ChangeType(iniFile.GetValue(key, null), typeof(T));
        }

        private T RetrieveObject<T>(string key, T defaultValue = default(T))
        {
            var json = iniFile.GetValue(Section, key);
            return JsonConvert.DeserializeObject<T>(json);
        }

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
                    InternalSetValue(key, SimpleStringCipher.Instance.Encrypt(Convert.ToString(value)));
                else
                    InternalSetValue(key, value);
            }
            else
                SetJsonValue(key, value);
        }

        public void Remove(string key)
        {
            iniFile.RemoveValue(Section, key);
        }
    }
}