/*
*
* 文件名    ：UserController                            
* 程序说明  : 用户数据控制器
* 更新时间  : 2020-05-21 10：35
* 更新人    : zhouhaogg789@outlook.com
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Api.Controllers
{
    using Consumption.Core.Response;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.EFCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.EFCore.Context;
    using Microsoft.AspNetCore.Server.HttpSys;

    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private readonly IUnitOfWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        public UserController(ILogger<UserController> logger, IUnitOfWork work)
        {
            this.logger = logger;
            this.work = work;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string account, string passWord)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(passWord))
                    return Ok(new ConsumptionResponse() { success = false, message = "Request failed" });

                var model = await work.GetRepository<User>()
                    .GetFirstOrDefaultAsync(predicate: x => x.Account == account && x.Password == passWord);
                if (model != null)
                {
                    #region 获取所属权限
                    var context = work.GetDbContext<ConsumptionContext>();
                    if (model.FlagAdmin == 1)
                    {
                        var data = from a in context.Menus
                                   select new
                                   {
                                       MenuName = a.MenuName,
                                       MenuCaption = a.MenuCaption,
                                       MenuNameSpace = a.MenuNameSpace,
                                       MenuAuth = a.MenuAuth
                                   };

                        return Ok(new ConsumptionResponse()
                        {
                            success = true,
                            dynamicObj = new
                            {
                                User = model,
                                Menus = data.ToList()
                            }
                        });
                    }
                    else
                    {
                        var data = from a in context.GroupFuncs
                                   join b in context.GroupUsers on a.GroupCode equals b.GroupCode
                                   join c in context.Groups on a.GroupCode equals c.GroupCode
                                   join d in context.Menus on a.MenuCode equals d.MenuCode
                                   where b.Account.Equals(account)
                                   select new
                                   {
                                       MenuName = d.MenuName,
                                       MenuCaption = d.MenuCaption,
                                       MenuNameSpace = d.MenuNameSpace,
                                       MenuAuth = a.Auth
                                   };
                        return Ok(new ConsumptionResponse()
                        {
                            success = true,
                            dynamicObj = new
                            {
                                User = model,
                                Menus = data.ToList()
                            }
                        });
                    }
                    #endregion
                }
                else
                    return Ok(new ConsumptionResponse() { success = false, message = "用户名或密码错误！" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "验证用户失败" });
            }
        }

        /// <summary>
        /// 获取用户数据信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                return Ok(new ConsumptionResponse()
                {
                    success = true,
                    dynamicObj = model
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse()
                {
                    success = false,
                    message = "获取用户数据异常!"
                });
            }
        }

        /// <summary>
        /// 获取用户数据列表信息
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserParameters parameters)
        {
            try
            {
                var models = await work.GetRepository<User>()
                    .GetPagedListAsync(
                    predicate: x =>
                    string.IsNullOrWhiteSpace(parameters.Search) ? true : x.UserName.Contains(parameters.Search) ||
                    string.IsNullOrWhiteSpace(parameters.Search) ? true : x.Account.Contains(parameters.Search),
                    pageIndex: parameters.PageIndex,
                    pageSize: parameters.PageSize);
                return Ok(new ConsumptionResponse()
                {
                    success = true,
                    dynamicObj = models
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse()
                {
                    success = false,
                    message = "获取用户数据异常!"
                });
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return Ok(new ConsumptionResponse() { success = false, message = "数据非法" });
                user.CreateTime = DateTime.Now;
                user.LoginCounter = 0;
                user.IsLocked = 0;
                user.FlagAdmin = 0;
                work.GetRepository<User>().Update(user);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });

                return Ok(new ConsumptionResponse() { success = false, message = "添加用户错误" });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "添加用户错误" });
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            if (user == null)
                return Ok(new ConsumptionResponse() { success = false, message = "数据非法" });
            try
            {
                //check?
                var repository = work.GetRepository<User>();
                if (user.Id == 0)
                {
                    user.CreateTime = DateTime.Now;
                    user.FlagOnline = string.Empty;
                    repository.Insert(user);
                    if (await work.SaveChangesAsync() > 0)
                        return Ok(new ConsumptionResponse() { success = true });
                }
                else
                {
                    var dbUser = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == user.Id);
                    if (dbUser == null) return Ok(new ConsumptionResponse()
                    {
                        success = false,
                        message = "该用户已不存在"
                    });
                    dbUser.UserName = user.UserName;
                    dbUser.Tel = user.Tel;
                    dbUser.Password = user.Password;
                    dbUser.IsLocked = user.IsLocked;
                    dbUser.Address = user.Address;
                    dbUser.Email = user.Email;
                    dbUser.FlagAdmin = user.FlagAdmin;
                    repository.Update(dbUser);
                    if (await work.SaveChangesAsync() > 0)
                        return Ok(new ConsumptionResponse() { success = true });
                }
                return Ok(new ConsumptionResponse() { success = false, message = $"保存用户信息错误" });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "修改用户信息错误" });
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var repository = work.GetRepository<User>();

                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (user == null)
                    return Ok(new ConsumptionResponse() { success = false, message = "删除用户异常" });
                repository.Delete(user);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });
                return Ok(new ConsumptionResponse() { success = false, message = $"删除数据异常" });
            }
            catch (Exception ex)
            {
                return Ok(new ConsumptionResponse() { success = false, message = ex.Message });
            }
        }

    }
}
