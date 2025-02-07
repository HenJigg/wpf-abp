using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Models.Configuration
{
    public class GeneralSettingsEditModel
    {
        public string Timezone { get; set; }

        /// <summary>
        /// This value is only used for comparing user's timezone to default timezone
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}
