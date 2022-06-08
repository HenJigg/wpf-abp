using System.IO;
using System.Runtime.InteropServices;

namespace AppFramework
{
    public class IniFile
    {
        public IniFile(string iniPath)
        {
            this.iniPath = iniPath;
            if (!File.Exists(this.iniPath))
                CreateIniFile();
        }

        private string iniPath;
        public string IniPath { get { return iniPath; } set { iniPath = value; } }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
            public string szText;
        }

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string def, out STRINGBUFFER retVal, int size, string filePath);

        public void SetValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.iniPath);
        }

        public string GetValue(string Section, string Key)
        {
            STRINGBUFFER RetVal;
            GetPrivateProfileString(Section, Key, null, out RetVal, 4096, this.iniPath);
            string temp = RetVal.szText;
            return temp.Trim();
        }

        public void RemoveValue(string Section, string Key)
        {
            WritePrivateProfileString(Section, Key, null, this.iniPath);
        }

        private void CreateIniFile()
        {
            StreamWriter w = File.CreateText(iniPath);
            w.Write("");
            w.Flush();
            w.Close();
        }
    }
}
