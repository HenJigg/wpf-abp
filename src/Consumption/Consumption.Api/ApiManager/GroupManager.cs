/*
*
* 文件名    ：GroupManager                          
* 程序说明  : 组数据
* 更新时间  : 2020-05-21 11:44 
*/

namespace Consumption.Api.ApiManager
{
    using AutoMapper;
    using Consumption.EFCore;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class GroupManager : IGroupManager
    {
        private readonly ILogger<GroupManager> logger;
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        /// <param name="mapper"></param>
        public GroupManager(ILogger<GroupManager> logger, IUnitOfWork work, IMapper mapper)
        {
            this.logger = logger;
            this.work = work;
            this.mapper = mapper;
        }

        /// <summary>
        /// 删除组数据(根据ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var gpWork = work.GetRepository<Group>();
                var group = await gpWork.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (group == null)
                    return new ApiResponse(201, "");
                gpWork.Delete(group);
                var guWork = work.GetRepository<GroupUser>();
                var gfWork = work.GetRepository<GroupFunc>();
                //移除所有的组用户
                var groupUsers = await guWork.GetAllAsync(predicate: x => x.GroupCode == group.GroupCode);
                for (int i = 0; i < groupUsers.Count; i++)
                    guWork.Delete(groupUsers);
                //移除所有的组模块
                var groupFuncs = await gfWork.GetAllAsync(predicate: x => x.GroupCode == group.GroupCode);
                for (int i = 0; i < groupFuncs.Count; i++)
                    gfWork.Delete(groupFuncs);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(200, "");
                return new ApiResponse(201, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new ApiResponse(201, "");
            }
        }

        /// <summary>
        /// 获取所有数据(根据条件)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> GetAll(QueryParameters param)
        {
            try
            {
                var models = await work.GetRepository<Group>().GetPagedListAsync(
                    predicate: x =>
                    string.IsNullOrWhiteSpace(param.Search) ? true : x.GroupCode.Contains(param.Search) ||
                    string.IsNullOrWhiteSpace(param.Search) ? true : x.GroupName.Contains(param.Search),
                    pageIndex: param.PageIndex,
                    pageSize: param.PageSize);
                return new ApiResponse(200, models);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "");
            }
        }

        /// <summary>
        /// 获取组数据(根据ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse> GetGroupData(int id)
        {
            try
            {
                var g = await work.GetRepository<Group>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (g != null)
                {
                    GroupDataDto header = new GroupDataDto();
                    header.group = g;
                    var groupUserList = work.GetRepository<GroupUser>()
                          .GetAll(predicate: x => x.GroupCode == g.GroupCode).ToList();
                    var userDtoList = mapper.Map<List<GroupUserDto>>(groupUserList);
                    header.GroupUsers = new ObservableCollection<GroupUserDto>(userDtoList);
                    header.GroupFuncs = work.GetRepository<GroupFunc>()
                        .GetAll(predicate: x => x.GroupCode == g.GroupCode).ToList();
                    return new ApiResponse(200, header);
                }
                else
                    return new ApiResponse(201, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "");
            }
        }

        /// <summary>
        /// 获取菜单列表模块
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetMenuModuleList()
        {
            try
            {
                var menuItmes = await work.GetRepository<Menu>().GetAllAsync();
                var authItems = await work.GetRepository<AuthItem>().GetAllAsync();

                if (menuItmes.Count > 0)
                {
                    List<MenuModuleGroup> menuGroups = new List<MenuModuleGroup>();
                    for (int i = 0; i < menuItmes.Count; i++)
                    {
                        var m = menuItmes[i];
                        MenuModuleGroup group = new MenuModuleGroup();
                        group.MenuCode = m.MenuCode;
                        group.MenuName = m.MenuName;
                        group.Modules = new ObservableCollection<MenuModule>();
                        for (int j = 0; j < authItems.Count; j++)
                        {
                            var au = authItems[j];
                            if ((m.MenuAuth & au.AuthValue) == au.AuthValue)
                            {
                                group.Modules.Add(new MenuModule() { Name = au.AuthName, Value = au.AuthValue });
                            }
                        }
                        menuGroups.Add(group);
                    }
                    return new ApiResponse(200, menuGroups);
                }
                return new ApiResponse(200, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "");
            }
        }

        /// <summary>
        /// 保存组数据(更新/新增)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Save(GroupDataDto param)
        {
            try
            {
                var g = param.group;
                var groupRepository = work.GetRepository<Group>();
                if (g.Id > 0)
                {
                    //ID存在为更新,需要处理是否真实存在？
                    var group = await groupRepository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id == g.Id);
                    if (group == null)
                        return new ApiResponse(201, "");
                    //查询组下已存在的用户
                    var groupUsers = await work.GetRepository<GroupUser>()
                        .GetAllAsync(predicate: x => x.GroupCode == group.GroupCode);
                    for (int i = 0; i < groupUsers.Count; i++)
                        work.GetRepository<GroupUser>().Delete(groupUsers[i]);

                    //查询组下已存在的模块
                    var groupFuncs = await work.GetRepository<GroupFunc>()
                        .GetAllAsync(predicate: x => x.GroupCode == group.GroupCode);
                    for (int i = 0; i < groupFuncs.Count; i++)
                        work.GetRepository<GroupFunc>().Delete(groupFuncs[i]);

                    group.GroupCode = g.GroupCode;
                    group.GroupName = g.GroupName;
                    groupRepository.Update(group);
                }
                else
                {
                    var group = await groupRepository
                .GetFirstOrDefaultAsync(predicate: x => x.GroupCode == g.GroupCode ||
                x.GroupName == g.GroupName);
                    if (group != null)
                        return new ApiResponse(201, "组编号/名称已重复,请勿重复添加!");
                    work.GetRepository<Group>().Insert(g);

                }
                //添加新增组用户信息
                param.GroupUsers?.ToList().ForEach(u =>
                {
                    work.GetRepository<GroupUser>().Insert(new GroupUser
                    {
                        GroupCode = g.GroupCode,
                        Account = u.Account
                    });
                });
                //添加新增组模块信息
                param.GroupFuncs?.ForEach(f =>
                {
                    if (f.Auth > 0)
                        work.GetRepository<GroupFunc>().Insert(new GroupFunc()
                        {
                            GroupCode = g.GroupCode,
                            MenuCode = f.MenuCode,
                            Auth = f.Auth,
                        });
                });
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(200, "");
                return new ApiResponse(201, "");
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return new ApiResponse(201, "");
            }
        }
    }
}
