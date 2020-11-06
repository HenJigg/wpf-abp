/*
*
* 文件名    ：BaseCenter                             
* 程序说明  : View/ViewModel 控制基类
* 更新时间  : 2020-05-21 17：25
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
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    /// <summary>
    /// View/ViewModel 控制基类(模块)
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class ModuleCenter<TView, TEntity>
        where TView : UserControl, new()
        where TEntity : BaseDto
    {
        public ModuleCenter() { }

        public ModuleCenter(IBaseViewModel<TEntity> viewModel)
        {
            this.viewModel = viewModel;
        }

        public TView view = new TView();
        public IBaseViewModel<TEntity> viewModel;

        public async Task BindDefaultModel(int AuthValue)
        {
            viewModel.InitPermissions(AuthValue);
            await viewModel.GetPageData(0);
            BindDataGridColumns();
            view.DataContext = viewModel;
        }

        public void BindDefaultModel()
        {
            view.DataContext = viewModel;
        }

        public virtual void BindDataGridColumns() { }

        public object GetView()
        {
            return view;
        }
    }

    /// <summary>
    ///  View/ViewModel 控制基类(组件)
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class ComponentCenter<TView>
        where TView : UserControl, new()
    {
        public ComponentCenter() { }

        public ComponentCenter(IComponentViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public TView view = new TView();
        public IComponentViewModel viewModel;

        public void BindDataGridColumns()
        {
        }

        public void BindDefaultModel()
        {
            view.DataContext = viewModel;
        }

        public Task BindDefaultModel(int AuthValue = 0)
        {
            this.BindDefaultModel();
            return Task.FromResult(true);
        }

        public object GetView()
        {
            return view;
        }
    }
}
