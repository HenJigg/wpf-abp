using Consumption.Core.IService;
using Consumption.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumption.Service
{
    public class UserService : IUserService
    {
        public Task<BaseResponse> LoginAsync(string account, string passWord)
        {

        }
    }
}
