using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Authorization.Users.Profile.Dto
{
    public class UploadFileOutput : ErrorInfo
    {
        public UploadFileOutput()
        {
        }

        public UploadFileOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

        public string AppName { get; set; }

        public string Version { get; set; }

        public string FileName { get; set; }
    }
}
