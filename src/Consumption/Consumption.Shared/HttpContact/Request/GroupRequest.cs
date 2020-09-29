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
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;

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

        public GroupDataDto groupDto { get; set; }

    }
}
