/*
*
* 文件名    ：MainCenter                             
* 程序说明  : 首页控制类 
* 更新时间  : 2020-05-21 17：31
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.Core.Response;
    using Consumption.Core.Interfaces;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Runtime;
    using System.Threading.Tasks;
    using Module = ViewModel.Common.Module;
    using Consumption.PC.Common;
    using Consumption.Core.Common;
    using Consumption.Common.Contract;
    using Consumption.PC.Template;
    using MaterialDesignThemes.Wpf;
    using System.Linq;
    using Consumption.ViewModel.Common;

    /// <summary>
    /// 首页控制类
    /// </summary>
    public class MainCenter : BaseDialogCenter<MaterialDesignMainWindow, MainViewModel>
    {
        public override void SubscribeMessenger()
        {
            //更新消息
            Messenger.Default.Register<MsgInfo>(View, "UpdateDialog", m =>
              {
                  if (m.IsOpen)
                      _ = DialogHost.Show(new SplashScreenView()
                      {
                          DataContext = new { Msg = m.Msg }
                      }, "Root");
                  else
                      ViewModel.DialogIsOpen = false;
              });
            //执行菜单模块动画
            Messenger.Default.Register<string>(View, "WindowMinimize", arg =>
            {
                View.WindowState = System.Windows.WindowState.Minimized;
            });
            Messenger.Default.Register<string>(View, "WindowMaximize", arg =>
            {
                if (View.WindowState == System.Windows.WindowState.Maximized)
                    View.WindowState = System.Windows.WindowState.Normal;
                else
                    View.WindowState = System.Windows.WindowState.Maximized;
            });
            
            //菜单执行相关动画及模板切换
            Messenger.Default.Register<string>(View, "ExpandMenu", arg =>
            {
                if (View.MENU.Width < 200)
                    AnimationHelper.CreateWidthChangedAnimation(View.MENU, 60, 200, new TimeSpan(0, 0, 0, 0, 300));
                else
                    AnimationHelper.CreateWidthChangedAnimation(View.MENU, 200, 60, new TimeSpan(0, 0, 0, 0, 300));
                for (int i = 0; i < ViewModel.ModuleManager.ModuleGroups.Count; i++)
                    ViewModel.ModuleManager.ModuleGroups[i].ContractionTemplate = View.MENU.Width < 200 ? false : true;

                //由于...
                var template = View.IC.ItemTemplateSelector;
                View.IC.ItemTemplateSelector = null;
                View.IC.ItemTemplateSelector = template;
            });
            //执行返回首页
            Messenger.Default.Register<string>(View, "GoHomePage", arg =>
            {
                InitHomeView();
            });
            //打开页面
            Messenger.Default.Register<string>(View, "OpenPage", async name =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(name)) return;
                    var m = ViewModel.ModuleManager.Modules.FirstOrDefault(t => t.Name.Equals(name));
                    if (m == null) return;
                    var module = ViewModel.ModuleList.FirstOrDefault(t => t.Name == m.Name);
                    if (module == null)
                    {
                        NetCoreProvider.Get<IModule>(m.TypeName, out IModule dialog);
                        if (dialog == null)
                        {
                            //404
                            return;
                        }
                        _ = DialogHost.Show(new SplashScreenView() { DataContext = new { Msg = "正在打开页面..." } }, "Root");
                        ViewModel.DialogIsOpen = true;
                        await Task.Delay(100);
                        //将数据库中获取的菜单Namespace在容器当中查找依赖关系的实例
                        await dialog.BindDefaultModel(m.Auth);
                        ViewModel.ModuleList.Add(new ModuleUIComponent()
                        {
                            Code = m.Code,
                            Auth = m.Auth,
                            Name = m.Name,
                            TypeName = m.TypeName,
                            Body = dialog.GetView()
                        });
                        ViewModel.CurrentModule = ViewModel.ModuleList[ViewModel.ModuleList.Count - 1];
                    }
                    else
                        ViewModel.CurrentModule = module;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
                finally
                {
                    ViewModel.DialogIsOpen = false; //关闭等待窗口
                    GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                }
            });
            //关闭页面
            Messenger.Default.Register<string>(View, "ClosePage", m =>
           {
               try
               {
                   var module = ViewModel.ModuleList.FirstOrDefault(t => t.Name.Equals(m));
                   if (module != null)
                   {
                       ViewModel.ModuleList.Remove(module);
                       if (ViewModel.ModuleList.Count > 0)
                           ViewModel.CurrentModule = ViewModel
                           .ModuleList[ViewModel.ModuleList.Count - 1];
                       else
                           ViewModel.CurrentModule = null;
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex.Message);
               }
               finally
               {
                   GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                   GC.Collect();
               }
           });
        }

        /// <summary>
        /// 首页重写弹窗-
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> ShowDialog()
        {
            await ViewModel.InitDefaultView();
            InitHomeView();
            return await base.ShowDialog();
        }

        /// <summary>
        /// 临时固定,后期修改动态绑定 2020-07-19
        /// </summary>
        void InitHomeView()
        {
            NetCoreProvider.Get("HomeCenter", out IModule dialog);
            dialog.BindDefaultModel();
            ModuleUIComponent component = new ModuleUIComponent();
            component.Name = "首页";
            component.Body = dialog.GetView();
            ViewModel.ModuleList.Add(component);
            ViewModel.ModuleManager.Modules.Add(component);
            ViewModel.CurrentModule = ViewModel.ModuleList[ViewModel.ModuleList.Count - 1];
        }
    }
}
