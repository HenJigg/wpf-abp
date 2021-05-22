/*
*
* 文件名    ：BasicManager                          
* 程序说明  : 基础数据
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
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class BasicManager : IBasicManager
    {
        private readonly ILogger<BasicManager> logger;
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        public BasicManager(ILogger<BasicManager> logger, IUnitOfWork work, IMapper mapper)
        {
            this.logger = logger;
            this.work = work;
            this.mapper = mapper;
        }

        /// <summary>
        /// 添加基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Add(BasicDto param)
        {
            try
            {
                var basic = mapper.Map<Basic>(param);
                await work.GetRepository<Basic>().InsertAsync(basic);
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

        /// <summary>
        /// 删除基础数据(根据ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var repository = work.GetRepository<Basic>();
                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (user == null)
                    return new ApiResponse(201, "");
                repository.Delete(user);
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
                var models = await work.GetRepository<Basic>().GetPagedListAsync(
                    predicate: x =>
                    string.IsNullOrWhiteSpace(param.Search) ? true : x.DataCode.Contains(param.Search),
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
        /// 保存基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Save(BasicDto param)
        {
            try
            {
                var repository = work.GetRepository<Basic>();
                var dbmodel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == param.Id);
                if (dbmodel == null) return new ApiResponse(201, "");
                dbmodel.NativeName = param.NativeName;
                dbmodel.EnglishName = param.EnglishName;
                dbmodel.LastUpdate = param.LastUpdate;
                dbmodel.DataCode = param.DataCode;
                dbmodel.LastUpdateBy = param.LastUpdateBy;
                repository.Update(dbmodel);
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
