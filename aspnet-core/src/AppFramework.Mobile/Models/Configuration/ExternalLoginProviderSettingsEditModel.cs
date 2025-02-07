using AppFramework.Authentication;
using System.Collections.Generic;

namespace AppFramework.Shared.Models.Configuration
{
    public class ExternalLoginProviderSettingsEditModel
    {
        public bool Facebook_IsDeactivated { get; set; }

        public FacebookExternalLoginProviderSettings Facebook { get; set; }

        public bool Google_IsDeactivated { get; set; }

        public GoogleExternalLoginProviderSettings Google { get; set; }

        public bool Twitter_IsDeactivated { get; set; }

        public TwitterExternalLoginProviderSettings Twitter { get; set; }

        public bool Microsoft_IsDeactivated { get; set; }

        public MicrosoftExternalLoginProviderSettings Microsoft { get; set; }

        public bool OpenIdConnect_IsDeactivated { get; set; }

        public OpenIdConnectExternalLoginProviderSettings OpenIdConnect { get; set; }

        public List<JsonClaimMapDto> OpenIdConnectClaimsMapping { get; set; }

        public bool WsFederation_IsDeactivated { get; set; }

        public WsFederationExternalLoginProviderSettings WsFederation { get; set; }

        public List<JsonClaimMapDto> WsFederationClaimsMapping { get; set; }
    }
}