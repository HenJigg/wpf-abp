using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.PC.ViewCenter
{
    using Consumption.PC.Common;
    using Consumption.PC.View;
    using Consumption.Shared.Common;
    using Consumption.Shared.Common.Attributes;
    using Consumption.Shared.Common.Enums;
    using Consumption.Shared.Dto;
    using Consumption.ViewModel;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 用户组
    /// </summary>
    [Module("权限管理", ModuleType.系统配置)]
    public class GroupCenter : ModuleCenter<GroupView, GroupDto>,IGroupCenter
    {
        public GroupCenter(IGroupViewModel viewModel) : base(viewModel)
        { }

        public override void BindDataGridColumns()
        {
            VisualHelper.SetDataGridColumns(view, "Grid", typeof(GroupDto));
        }
    }
}
