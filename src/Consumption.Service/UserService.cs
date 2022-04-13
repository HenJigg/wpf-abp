/*
*
* 文件名    ：UserService                             
* 程序说明  : 用户服务
* 更新时间  : 2020-05-30 20：35 
*/

namespace Consumption.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Consumption.Core.Request;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;
    using Consumption.ViewModel.Interfaces;
    using RestSharp;

    public class UserService : BaseService<UserDto>, IUserRepository
    {
        public async Task<WebResult<List<AuthItem>>> GetAuthListAsync()
        {
            return await new BaseServiceRequest().GetRequest<List<AuthItem>>(new AuthItemRequest(), Method.GET);
        }

        public async Task<WebResult> GetUserPermByAccountAsync(string account)
        {
            return await new BaseServiceRequest().GetRequest<WebResult>(new UserPermRequest()
            {
                account = account
            }, Method.GET);
        }

        public async Task<WebResult<UserInfoDto>> LoginAsync(string account, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<UserInfoDto>(new UserLoginRequest()
            {
                Parameter = new LoginDto() { Account = account, PassWord = passWord }
            }, Method.POST);
        }
    }
}
