using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;

namespace AppFramework.Authorization.Accounts.Dto
{
    public class ActivateEmailInput: IShouldNormalize
    {
        public long UserId { get; set; }

        public string ConfirmationCode { get; set; }

        /// <summary>
        /// Encrypted values for {TenantId}, {UserId} and {ConfirmationCode}
        /// </summary>
        public string c { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["userId"] != null)
                {
                    UserId = Convert.ToInt32(query["userId"]);
                }

                if (query["confirmationCode"] != null)
                {
                    ConfirmationCode = query["confirmationCode"];
                }
            }
        }
    }
}