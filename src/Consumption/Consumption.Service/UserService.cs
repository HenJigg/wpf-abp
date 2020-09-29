/*
*
* 文件名    ：UserService                             
* 程序说明  : 用户服务
* 更新时间  : 2020-05-30 20：35
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
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
        public async Task<BaseResponse> GetAuthListAsync()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new AuthItemRequest(), Method.GET);
        }

        public async Task<BaseResponse> GetUserPermByAccountAsync(string account)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserPermRequest()
            {
                account = account
            }, Method.GET);
        }

        public async Task<BaseResponse> LoginAsync(string account, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserLoginRequest()
            {
                Parameter = new LoginDto() { Account = account, PassWord = passWord }
            }, Method.POST);
        }
    }
}
