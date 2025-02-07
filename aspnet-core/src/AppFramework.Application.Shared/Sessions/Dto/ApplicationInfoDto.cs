using System;
using System.Collections.Generic;

namespace AppFramework.Sessions.Dto
{
    public class ApplicationInfoDto
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Currency { get; set; }

        public string CurrencySign { get; set; }

        public bool AllowTenantsToChangeEmailSettings { get; set; }

        public bool UserDelegationIsEnabled { get; set; }
        
        public double TwoFactorCodeExpireSeconds { get; set; }

        public Dictionary<string, bool> Features { get; set; }
    }
}
