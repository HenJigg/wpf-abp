using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Dto
{
    public class VersionDto
    {
        public string AppName { get; set; }

        public string Version { get; set; }

        public string FileName { get; set; }

        public bool IsEnable { get; set; }

        public bool IsForced { get; set; }
    }
}
