/*
*
* 文件名    ：UserRepository                             
* 程序说明  : 用户数据接口实现
* 更新时间  : 2020-05-21 16：41
* 更新人    : zhouhaogg789@outlook.com
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.EFCore.Repository
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Common;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.EFCore.Orm;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {

        }


        public override async Task<PaginatedList<User>> GetModelList(QueryParameters parameters)
        {
            var query = consumptionContext.Users.AsQueryable();
            if (parameters.Search != null)
            {
                query = query.Where(q => q.Account.Contains(parameters.Search) ||
                        q.UserName.Contains(parameters.Search) || q.Tel.Contains(parameters.Search)).AsQueryable();
            }
            return await base.GetModelList(query, parameters);
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await consumptionContext.Users.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public async Task<User> LoginAsync(string account, string passWord)
        {
            return await consumptionContext.Users.FirstOrDefaultAsync(t => t.Account == account && t.Password == passWord);
        }
    }
}
