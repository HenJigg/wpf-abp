/*
*
* 文件名    ：GroupRequest                             
* 程序说明  : 用户组列表请求
* 更新时间  : 2020-08-04 17：26
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Request
{
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    /// <summary>
    /// 用户组模板信息请求
    /// </summary>
    public class GroupModuleRequest : BaseRequest
    {
        public override string route { get => "api/Group/GetMenuModules"; }
    }

    /// <summary>
    /// 组明细数据请求
    /// </summary>
    public class GroupInfoRequest : BaseRequest
    {
        public override string route { get => "api/Group/GetGroupInfo"; }

        public int id { get; set; }
    }

    /// <summary>
    /// 保存组数据请求
    /// </summary>
    public class GroupSaveRequest : BaseRequest
    {
        public override string route { get => "api/Group/Save"; }

        public GroupHeader parameter { get; set; }
    }


    /// <summary>
    /// 组及成员和模块信息
    /// </summary>
    public class GroupHeader : ViewModelBase
    {
        public Group group { get; set; } = new Group();

        private ObservableCollection<GroupUser> groupUsers =
            new ObservableCollection<GroupUser>();
        private List<GroupFunc> groupFuncs = new List<GroupFunc>();

        /// <summary>
        /// 组所包含用户
        /// </summary>
        public ObservableCollection<GroupUser> GroupUsers
        {
            get { return groupUsers; }
            set
            {
                groupUsers = value; RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 组所包含的模块清单
        /// </summary>
        public List<GroupFunc> GroupFuncs
        {
            get { return groupFuncs; }
            set { groupFuncs = value; }
        }
    }
}
