/*
*
* 文件名    ：UserViewModel                             
* 程序说明  : 用户信息
* 更新时间  : 2020-06-03 20：17
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.ViewModel
{
    using Consumption.Core.Response;
    using Consumption.Core.Entity;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Consumption.Core.Interfaces;
    using Consumption.Common.Contract;
    using Consumption.Core.Common;
    using Consumption.ViewModel.Common;

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserViewModel : BaseDataViewModel<User>
    {
        private readonly IConsumptionService service;
        public UserViewModel()
        {
            NetCoreProvider.Get(out service);
        }

        public override async Task GetPageData(int pageIndex)
        {
            try
            {
                SelectPageTitle = "用户管理";
                var r = await service.GetUserListAsync(new Core.Query.UserParameters()
                {
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    Search = SearchText
                });
                if (r != null && r.success)
                {
                    GridModelList = new System.Collections.ObjectModel.ObservableCollection<User>();
                    this.TotalCount = r.dynamicObj.TotalCount;
                    r.dynamicObj.Items?.ToList().ForEach(arg =>
                    {
                        GridModelList.Add(arg);
                    });
                    base.SetPageCount();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public override void Add()
        {
            GridModel = new User();
            SelectPageTitle = "编辑用户信息";
            SelectPageIndex = 1;
            base.Add();
        }

        public override void Edit()
        {
            if (GridModel == null) return;
            SelectPageTitle = "编辑用户信息";
            SelectPageIndex = 1;
            base.Edit();
        }

        public override void Save()
        {
            SelectPageTitle = "用户管理";
            base.Save();
        }

        public override void Cancel()
        {
            SelectPageTitle = "用户管理";
            base.Save();
        }
    }
}
