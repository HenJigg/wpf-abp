/*
*
* 文件名    ：HomeViewModel                             
* 程序说明  : 首页模块
* 更新时间  : 2020-05-28 13:27
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
    using Consumption.Core.Entity;
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    /// <summary>
    /// 首页模块
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            Bills = new ObservableCollection<Bill>();
            for (int i = 0; i < 13; i++)
            {
                Bills.Add(new Bill()
                {
                    Remark = "公益活动",
                    Name = "参与社区活动",
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    Amount = 3000
                });
            }
        }

        private ObservableCollection<Bill> bills;

        public ObservableCollection<Bill> Bills
        {
            get { return bills; }
            set { bills = value; RaisePropertyChanged(); }
        }

    }
}
