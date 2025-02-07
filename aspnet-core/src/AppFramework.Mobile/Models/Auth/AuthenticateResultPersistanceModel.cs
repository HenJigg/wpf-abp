using System;
using System.Collections.Generic;

namespace AppFramework.Shared.Models
{
    public class AuthenticateResultPersistanceModel
    {
        public string AccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }

        public long UserId { get; set; }

        public bool ShouldResetPassword { get; set; }

        public bool RequiresTwoFactorVerification { get; set; }

        public IList<string> TwoFactorAuthProviders { get; set; }
    }
}