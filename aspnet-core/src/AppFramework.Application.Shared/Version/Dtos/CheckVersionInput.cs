using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Version.Dtos
{
    public class CheckVersionInput
    {
        public string ApplicationName { get; set; }

        public string Version { get; set; }
    }
}
