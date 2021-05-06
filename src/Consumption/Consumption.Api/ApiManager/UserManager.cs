/*
*
* 文件名    ：UserManager                          
* 程序说明  : 用户数据
* 更新时间  : 2020-05-21 11:44 
*/

namespace Consumption.Api.ApiManager
{
    using AutoMapper;
    using Consumption.EFCore;
    using Consumption.EFCore.Context;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> logger;
        private readonly IUnitOfWork work;
        private readonly IMapper autoMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        /// <param name="autoMapper"></param>
        public UserManager(ILogger<UserManager> logger, IUnitOfWork work, IMapper autoMapper)
        {
            this.logger = logger;
            this.work = work;
            this.autoMapper = autoMapper;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Add(UserDto param)
        {
            try
            {
                var user = autoMapper.Map<User>(param);
                user.CreateTime = DateTime.Now;
                user.LoginCounter = 0;
                user.IsLocked = 0;
                user.FlagAdmin = 0;
                work.GetRepository<User>().Update(user);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(200, "");

                return new ApiResponse(201, "添加用户错误");
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return new ApiResponse(201, "添加用户错误");
            }
        }

        /// <summary>
        /// 删除用户(根据ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var repository = work.GetRepository<User>();
                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (user == null)
                    return new ApiResponse(201, "删除用户异常");
                repository.Delete(user);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(200, "");
                return new ApiResponse(201, "删除数据异常");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new ApiResponse(201, "");
            }
        }

        /// <summary>
        /// 查询用户(根据ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Get(int id)
        {
            try
            {
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                return new ApiResponse(200, model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "获取用户数据异常!");
            }
        }

        /// <summary>
        /// 获取用户(根据条件)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> GetAll(QueryParameters param)
        {
            try
            {
                var models = await work.GetRepository<User>()
                    .GetPagedListAsync(
                    predicate: x =>
                    string.IsNullOrWhiteSpace(param.Search) ? true : x.UserName.Contains(param.Search) ||
                    string.IsNullOrWhiteSpace(param.Search) ? true : x.Account.Contains(param.Search),
                    pageIndex: param.PageIndex,
                    pageSize: param.PageSize);
                return new ApiResponse(200, models);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "获取用户数据异常!");
            }
        }

        /// <summary>
        /// 验证身份(根据条件）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Login(LoginDto param)
        {
            try
            {
                var model = await work.GetRepository<User>()
                    .GetFirstOrDefaultAsync(predicate: x => x.Account == param.Account && x.Password == param.PassWord);
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

                        return new ApiResponse(200, new
                        {
                            User = model,
                            Menus = data.ToList()
                        });
                    }
                    else
                    {
                        var data = from a in context.GroupFuncs
                                   join b in context.GroupUsers on a.GroupCode equals b.GroupCode
                                   join c in context.Groups on a.GroupCode equals c.GroupCode
                                   join d in context.Menus on a.MenuCode equals d.MenuCode
                                   where b.Account.Equals(param.Account)
                                   select new
                                   {
                                       MenuName = d.MenuName,
                                       MenuCaption = d.MenuCaption,
                                       MenuNameSpace = d.MenuNameSpace,
                                       MenuAuth = a.Auth
                                   };
                        return new ApiResponse(200, new
                        {
                            User = model,
                            Menus = data.ToList()
                        });
                    }
                    #endregion
                }
                else
                    return new ApiResponse(201, "用户名或密码错误!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new ApiResponse(201, "验证用户失败");
            }
        }

        /// <summary>
        /// 保存用户(更新/新增)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Save(UserDto param)
        {
            try
            {
                var user = autoMapper.Map<User>(param);
                var repository = work.GetRepository<User>();
                if (param.Id == 0)
                {
                    param.CreateTime = DateTime.Now;
                    param.FlagOnline = string.Empty;
                    repository.Insert(user);
                    if (await work.SaveChangesAsync() > 0)
                        return new ApiResponse(200, "");
                }
                else
                {
                    var dbUser = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == param.Id);
                    if (dbUser == null) return new ApiResponse(201, "该用户已不存在");
                    dbUser.UserName = param.UserName;
                    dbUser.Tel = param.Tel;
                    dbUser.Password = param.Password;
                    dbUser.IsLocked = param.IsLocked;
                    dbUser.Address = param.Address;
                    dbUser.Email = param.Email;
                    dbUser.FlagAdmin = param.FlagAdmin;
                    repository.Update(dbUser);
                    if (await work.SaveChangesAsync() > 0)
                        return new ApiResponse(200, "");
                }
                return new ApiResponse(201, "保存用户信息错误");
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return new ApiResponse(201, "修改用户信息错误");
            }
        }
    }
}
