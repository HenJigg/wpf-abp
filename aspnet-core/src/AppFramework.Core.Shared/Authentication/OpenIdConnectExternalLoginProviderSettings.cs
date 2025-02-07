using System;
using Abp.Extensions;
using Abp.UI;

namespace AppFramework.Authentication
{
    public class OpenIdConnectExternalLoginProviderSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string LoginUrl { get; set; }
        public bool ValidateIssuer { get; set; }

        public bool IsValid()
        {
            bool valid = !ClientId.IsNullOrWhiteSpace() ||
                         !Authority.IsNullOrWhiteSpace();

            if (valid && !Authority.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new UserFriendlyException("Property name \"Authority\" must start with \"https://\"");
            }

            return valid;
        }
    }
}